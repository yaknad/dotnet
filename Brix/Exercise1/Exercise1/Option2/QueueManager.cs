using Exercise1.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1.Option2
{
    class QueueManager
    {
        private ConcurrentQueue<Customer> customersQueue;
        private Timer enqueueCustomersTimer;
        //private SemaphoreSlim maxCashiersSemaphore;

        public QueueManager(ConcurrentQueue<Customer> customersQueue, Timer enqueueCustomersTimer/*, SemaphoreSlim maxCashiersSemaphore*/)
        {
            this.customersQueue = customersQueue;
            this.enqueueCustomersTimer = enqueueCustomersTimer;

            //BlockingCollection<Customer> 

        }       

    }
}
