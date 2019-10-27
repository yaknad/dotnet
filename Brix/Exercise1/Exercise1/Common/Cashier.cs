using System;

namespace Exercise1.Common
{
    public class Cashier
    {
        private readonly ISleepService sleepService;
        public string Name { get; set; }
        
        public Cashier(string name, ISleepService sleepService)
        {
            this.sleepService = sleepService;
            Name = name;
        }

        public void ProcessCustomer(Customer customer, int processTimeInSeconds)
        {
            sleepService.Sleep(processTimeInSeconds * 1000);
            customer.Finished = DateTime.Now;
        }
    }
}
