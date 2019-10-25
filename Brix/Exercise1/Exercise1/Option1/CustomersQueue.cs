using Exercise1.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Option1
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
            // TODO: handle exceptions
            customers.Add(customer);
        }

        public Customer DequeueCustomer()
        {
            // TODO: handle exceptions
            return customers.Take();
        }

        public int GetCustomersCount()
        {
            // TODO: handle exceptions
            return customers.Count;
        }
    }
}
