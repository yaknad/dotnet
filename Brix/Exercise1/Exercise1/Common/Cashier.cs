using System;

namespace Exercise1.Common
{
    public class Cashier
    {
        private readonly SleepService sleepService;
        public string Name { get; set; }
        public DateTime ShiftStart { get; set; }

        public Cashier(string name, SleepService sleepService)
        {
            this.sleepService = sleepService;
            Name = name;
            ShiftStart = DateTime.Now;
        }

        public void ProcessCustomer(Customer customer, int processTimeInSeconds)
        {
            customer.Dequeued = DateTime.Now;
            sleepService.Sleep(processTimeInSeconds * 1000);
            customer.Finished = DateTime.Now;
        }
    }
}
