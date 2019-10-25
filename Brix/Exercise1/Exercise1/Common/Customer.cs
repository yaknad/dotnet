using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Common
{
    public class Customer
    {
        public DateTime Enqueued { get; set; }
        public DateTime Dequeued { get; set; }
        public DateTime Finished { get; set; }
    }
}
