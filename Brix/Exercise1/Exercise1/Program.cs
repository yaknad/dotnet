using Exercise1.Option1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("************** Store is now open! **************");
            RunOption1().Wait();
            Console.WriteLine("************** Store is closed! **************");
            Console.ReadKey();

            //RunOption2();
        }

        private async static Task RunOption1()
        {
            StoreManagerFactory factory = new StoreManagerFactory();
            StoreManager storeManager = factory.GetStoreManager(1, 1, 5);
            storeManager.Start();

            await Task.Delay(20000);
            storeManager.Stop();
        }

        private static void RunOption2()
        {

        }
    }
}
