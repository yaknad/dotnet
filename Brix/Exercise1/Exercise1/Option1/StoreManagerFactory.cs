using Exercise1.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1.Option1
{
    public class StoreManagerFactory
    {
        public StoreManager GetStoreManager(int customerEnqueueRateInSeconds, int minCashierProcessingTime, int maxCashierProcessingTime)
        {
            var customersQueue = new CustomersQueue();
            Func<Timer> enqueueTimerFactory = () => GetCustomerEnqueueTimer(customersQueue, customerEnqueueRateInSeconds);
            var dequeueTaskScheduler = GetCustomerDequeueTaskScheduler();
            // Spec was 5 cashiers. These is an optimazation to set the cashiers count by calculation.
            var cashiersCount = GetCashiersCount(customerEnqueueRateInSeconds, minCashierProcessingTime, maxCashierProcessingTime);
            return new StoreManager(customersQueue, enqueueTimerFactory, dequeueTaskScheduler, new SleepService(), cashiersCount, 
                minCashierProcessingTime, maxCashierProcessingTime, new Logger());
        }

        private Timer GetCustomerEnqueueTimer(CustomersQueue customersQueue, int customerEnqueueRateInSeconds)
        {
            return new Timer(state => customersQueue.EnqueueCustomer(), 
                             null, 
                             0, 
                             customerEnqueueRateInSeconds * 1000);
        }

        private int GetCashiersCount(int customerEnqueueRateInSeconds, int minCashierProcessingTime, int maxCashierProcessingTime)
        {
            return StoreManager.GetOptimalCashiersCount(customerEnqueueRateInSeconds, minCashierProcessingTime, maxCashierProcessingTime);
        }

        private TaskScheduler GetCustomerDequeueTaskScheduler()
        {
            // REQUIERD in order to make StoreManager class testable (enables to mock the async scheduling and run any required tasks synchronously)
            return Task.Factory.Scheduler;
        }        
    }
}
