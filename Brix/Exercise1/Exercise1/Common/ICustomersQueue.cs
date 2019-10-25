using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Common
{
    public interface ICustomersQueue
    {
        void EnqueueCustomer();
        Customer DequeueCustomer();
        int GetCustomersCount();
    }
}
