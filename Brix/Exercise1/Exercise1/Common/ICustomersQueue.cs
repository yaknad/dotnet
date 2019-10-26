namespace Exercise1.Common
{
    public interface ICustomersQueue
    {
        void EnqueueCustomer();
        Customer DequeueCustomer();
        int GetCustomersCount();
    }
}
