using Exercise1.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1.Option1
{
    public class StoreManager : IStoreManager, IDisposable
    {
        private readonly ICustomersQueue customersQueue;
        private Timer enqueueCustomersTimer;
        private readonly Func<Timer> enqueueCustomerTimerFactory;
        private readonly TaskScheduler customerDequeueTaskScheduler;
        private readonly ISleepService sleepService;
        private readonly int cashiersCount;
        private readonly int minCashierProcessingTime;
        private readonly int maxCashierProcessingTime;
        private readonly ILogger logger;
        private readonly TaskFactory taskFactory;
        private readonly CancellationTokenSource cancellationTokenSource;
        private bool disposedValue = false;

        public StoreManager(ICustomersQueue customersQueue, Func<Timer> enqueueCustomerTimerFactory, TaskScheduler customerDequeueTaskScheduler, 
            ISleepService sleepService, int maxCashiersCount, int minCashierProcessingTime, int maxCashierProcessingTime, ILogger logger)
        {
            this.customersQueue = customersQueue;
            this.enqueueCustomerTimerFactory = enqueueCustomerTimerFactory;
            this.customerDequeueTaskScheduler = customerDequeueTaskScheduler;
            this.sleepService = sleepService;
            this.cashiersCount = maxCashiersCount;
            this.minCashierProcessingTime = minCashierProcessingTime;
            this.maxCashierProcessingTime = maxCashierProcessingTime;
            this.logger = logger;
            this.taskFactory = new TaskFactory(customerDequeueTaskScheduler);
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        #region Static methods

        public static int GetOptimalCashiersCount(int customerEnqueueRateInSeconds, int minCashierProcessingTime, int maxCashierProcessingTime)
        {
            // this will ensure that the queue size will never exceed the number of cashiers
            return Convert.ToInt32(Math.Ceiling((double)(maxCashierProcessingTime / customerEnqueueRateInSeconds)));

            // a more efficient option is to use less cashiers (threads), according to the average cashier processing time, 
            // but then the customers queue size may grow and shrink, so some customers will wait a bit longer
            // (but in the long run, the queue will never overflow).            

            //return Convert.ToInt32(Math.Ceiling((double)(maxCashierProcessingTime + minCashierProcessingTime)) / 2 / customerEnqueueRateInSeconds);
        }

        #endregion

        #region Public methods

        public void Start()
        {
            EnqueueCustomers();
            ProcessCustomers();
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            enqueueCustomersTimer.Dispose();
            enqueueCustomersTimer = null;
        }

        #endregion

        #region Private methods

        private void EnqueueCustomers()
        {
            enqueueCustomersTimer = enqueueCustomerTimerFactory();
        }

        private void ProcessCustomers()
        {
            var cancellationToken = cancellationTokenSource.Token;

            for (int i = 0; i < cashiersCount; i++)
            {
                var cashier = new Cashier($"Cashier{i}", sleepService);
                taskFactory.StartNew(GetProcessCustomerAction(cashier, cancellationToken), cancellationToken);
            }
        }

        private Action GetProcessCustomerAction(Cashier cashier, CancellationToken token)
        {
            return () =>
            {
                try
                {
                    var customer = customersQueue.DequeueCustomer(); // blocking operation!
                    var rnd = new Random();
                    var processDurationInSeconds = rnd.Next(minCashierProcessingTime, maxCashierProcessingTime + 1);
                    cashier.ProcessCustomer(customer, processDurationInSeconds); // blocking operation!
                    logger.Info($"{cashier.Name} finished processing customer at: {customer.Finished}. Number of waiting customers: {customersQueue.GetCustomersCount()}");
                }
                catch (Exception e)
                {
                    logger.Error($"Error in {cashier.Name} work: {e}");
                }
                finally
                {
                    // I issue a new task instaed of just calling this Action recursively, beacuse it will cause 
                    // the call stack to chain and eventually to cause an out of memory error.
                    // The child task is, by deafult, detached from its parent task and therefore will not chain a call stack.
                    if (!token.IsCancellationRequested)
                    {
                        taskFactory.StartNew(GetProcessCustomerAction(cashier, token), token);
                    }
                    else
                    {
                        logger.Info($"{cashier.Name} work was cancelled. Stopping.");
                    }
                }
            };
        }

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cancellationTokenSource.Dispose();
                }

                if (enqueueCustomersTimer != null)
                {
                    try
                    {
                        enqueueCustomersTimer.Dispose();
                    }
                    catch (Exception) { }
                }

                disposedValue = true;
            }
        }

         ~StoreManager()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
