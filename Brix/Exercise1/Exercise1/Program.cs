using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exercise1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("************** Store is now open! **************");

            RunOption1().Wait();
            //RunOption2();
            
            Console.ReadKey();           
        }

        private async static Task RunOption1()
        {
            Option1.StoreManagerFactory factory = new Option1.StoreManagerFactory();
            Option1.StoreManager storeManager = factory.GetStoreManager(1, 1, 5);
            storeManager.Start();

            // the Store flow runs in other threads, so the flow of this function continues, and we may use storeMAnager.Stop().
            await Task.Delay(20000);
            storeManager.Stop();
            Console.WriteLine("************** Store is closed! **************");
        }

        private static void RunOption2()
        {
            Option2.StoreManagerFactory factory = new Option2.StoreManagerFactory();
            Option2.StoreManager storeManager = factory.GetStoreManager(1, 1, 5);

            // the Store flow runs in the main threads, so this function's flow is blocked and we can't use storeMAnager.Stop() in the current thread.
            Task.Delay(20000).ContinueWith(task =>
            {
                storeManager.Stop();
                Console.WriteLine("************** Store is closed! **************");
            });

            storeManager.Start(); // blocking operartion
        }
    }
}
