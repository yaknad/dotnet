using Exercise1.Common;
using System;
using System.Collections.Concurrent;

namespace Exercise1.Common
{
    // I created this wrapper for the qeueue (BlockingCollection) in order to make the CashierManager class testable, 
    // since it's using a timer and in order to make the timer mockable, I need to extract the TimerCallback function 
    // (that is required in the Timer's constructor) from the CashierManager.
    public class CustomersQueue : ICustomersQueue
    {
        private BlockingCollection<Customer> customers;

        public CustomersQueue()
        {
            customers = new BlockingCollection<Customer>();
        }

        public void EnqueueCustomer()
        {
            var customer = new Customer { Enqueued = DateTime.Now };
            customers.Add(customer);
        }

        public Customer DequeueCustomer()
        {
            var customer = customers.Take();
            customer.Dequeued = DateTime.Now;
            return customer;
        }

        public int GetCustomersCount()
        {
            return customers.Count;
        }
    }
}
