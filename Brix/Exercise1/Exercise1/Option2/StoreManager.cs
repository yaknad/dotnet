using Exercise1.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1.Option2
{
    public class StoreManager : IStoreManager, IDisposable
    {
        private readonly ICustomersQueue customersQueue;
        private readonly SemaphoreSlim maxCashiersSemaphore;
        private readonly Func<Timer> enqueueTimerFactory;
        private readonly TaskScheduler customerDequeueTaskScheduler;
        private readonly SleepService sleepService;
        private readonly int minCashierProcessingTime;
        private readonly int maxCashierProcessingTime;
        private readonly ILogger logger;
        private readonly TaskFactory taskFactory;
        private Timer enqueueCustomersTimer;
        private volatile bool stopped;
        private bool disposedValue = false;

        public StoreManager(ICustomersQueue customersQueue, Func<Timer> enqueueTimerFactory, TaskScheduler customerDequeueTaskScheduler,
            SleepService sleepService, int maxCashiersCount, int minCashierProcessingTime, int maxCashierProcessingTime, ILogger logger)
        {
            this.customersQueue = customersQueue;
            this.enqueueTimerFactory = enqueueTimerFactory;
            this.customerDequeueTaskScheduler = customerDequeueTaskScheduler;
            this.sleepService = sleepService;
            this.minCashierProcessingTime = minCashierProcessingTime;
            this.maxCashierProcessingTime = maxCashierProcessingTime;
            this.logger = logger;
            this.taskFactory = new TaskFactory(customerDequeueTaskScheduler);
            this.maxCashiersSemaphore = new SemaphoreSlim(maxCashiersCount);
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
            stopped = true;
            enqueueCustomersTimer.Dispose();
            FinishRemainingCustomers();
        }

        #endregion

        #region Private methods

        private void EnqueueCustomers()
        {
            stopped = false;
            enqueueCustomersTimer = enqueueTimerFactory();
        }

        private void ProcessCustomers()
        {
            var rnd = new Random();            

            while (!stopped)
            {
                maxCashiersSemaphore.Wait(); // blocking operation
                ProcessSingleCustomer(rnd);
            }
        }

        private void FinishRemainingCustomers()
        {
            logger.Info("Store was closed. Finishing with the last remaining customers.");

            Random rnd = new Random();

            while (customersQueue.GetCustomersCount() > 0)
            {
                maxCashiersSemaphore.Wait(); // blocking operation
                ProcessSingleCustomer(rnd);
            }
        }

        private void ProcessSingleCustomer(Random rnd)
        {
            Customer customer = customersQueue.DequeueCustomer(); // blocking operation

            int processDurationInSeconds = rnd.Next(minCashierProcessingTime, maxCashierProcessingTime + 1);
            taskFactory.StartNew(() =>
            {
                Cashier cashier = new Cashier($"Cashier-{Thread.CurrentThread.ManagedThreadId}", sleepService);

                try
                {
                    cashier.ProcessCustomer(customer, processDurationInSeconds); // blocking operation!
                    logger.Info($"{cashier.Name} finished processing customer at: {customer.Finished}. Number of waiting customers: {customersQueue.GetCustomersCount()}");
                }
                catch (Exception e)
                {
                    logger.Error($"Error in {cashier.Name} work: {e}");
                }
                finally
                {
                    maxCashiersSemaphore.Release();
                }                
            });            
        }

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    maxCashiersSemaphore.Dispose();                                       
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
