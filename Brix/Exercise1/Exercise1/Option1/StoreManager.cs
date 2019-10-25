using Exercise1.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1.Option1
{
    public class StoreManager : IStoreManager, IDisposable
    {
        private readonly CustomersQueue customersQueue;
        private Timer enqueueTimer;
        private readonly Func<Timer> enqueueTimerFactory;
        private readonly TaskScheduler customerDequeueTaskScheduler;
        private readonly SleepService sleepService;
        private readonly int maxCashiersCount;
        private readonly int minCashierProcessingTime;
        private readonly int maxCashierProcessingTime;
        private readonly ILogger logger;
        private readonly TaskFactory taskFactory;
        private readonly CancellationTokenSource tokenSource;
        private bool disposedValue = false;

        public static int GetOptimalCashiersCount(int customerEnqueueRateInSeconds, int minCashierProcessingTime, int maxCashierProcessingTime)
        {
            // this will ensure that the queue size will never exceed the number of cashiers
            return Convert.ToInt32(Math.Ceiling((double)(maxCashierProcessingTime / customerEnqueueRateInSeconds)));

            // a more efficient option is to use less cashiers (threads), according to the average cashier processing time, 
            // but then the customers queue size may grow and shrink, so some customers will wait a bit longer
            // (but in the long run, the queue will never overflow).            

            //return Convert.ToInt32(Math.Ceiling((double)(maxCashierProcessingTime + minCashierProcessingTime)) / 2 / customerEnqueueRateInSeconds);
        }

        public StoreManager(CustomersQueue customersQueue, Func<Timer> enqueueTimerFactory, TaskScheduler customerDequeueTaskScheduler, 
            SleepService sleepService, int maxCashiersCount, int minCashierProcessingTime, int maxCashierProcessingTime, ILogger logger)
        {
            this.customersQueue = customersQueue;
            this.enqueueTimerFactory = enqueueTimerFactory;
            this.customerDequeueTaskScheduler = customerDequeueTaskScheduler;
            this.sleepService = sleepService;
            this.maxCashiersCount = maxCashiersCount;
            this.minCashierProcessingTime = minCashierProcessingTime;
            this.maxCashierProcessingTime = maxCashierProcessingTime;
            this.logger = logger;
            this.taskFactory = new TaskFactory(customerDequeueTaskScheduler);
            this.tokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            StartEnqueuingCustomers();
            StartProcessingCustomers();
        }

        public void Stop()
        {
            tokenSource.Cancel();
        }

        private void StartEnqueuingCustomers()
        {
            enqueueTimer = enqueueTimerFactory();
        }

        private void StartProcessingCustomers()
        {
            var token = tokenSource.Token;
            var rnd = new Random();

            for (int i = 0; i < maxCashiersCount; i++)
            {
                Cashier cashier = new Cashier($"Cashier{i}", sleepService);
                taskFactory.StartNew(GetProcessCustomerAction(cashier, rnd, token), token);
            }
        }

        private Action GetProcessCustomerAction(Cashier cashier, Random rnd, CancellationToken token)
        {
            return () =>
            {
                try
                {
                    Customer customer = customersQueue.DequeueCustomer(); // blocking operation!

                    int processDurationInSeconds = rnd.Next(minCashierProcessingTime, maxCashierProcessingTime + 1);
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
                        taskFactory.StartNew(GetProcessCustomerAction(cashier, rnd, token), token);
                    }
                    else
                    {
                        logger.Info($"{cashier.Name} work was cancelled. Stopping.");
                    }
                }
            };
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    tokenSource.Dispose();
                    enqueueTimer.Dispose();
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
