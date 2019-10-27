Hello,

Sorry for the very basic exception handling (lack of time).
I tried to build the classes as testable as possible.
This is the reason for extracting some components to separate classes (e.g. SleepService, CustomerQueue, Logger),
in order to be able to inject some test-problematic components and make them mockable.

Option1
********
Working form:
-------------
The cashiers are responsible for pulling customers from the queue.
Uses a fixed set of cashiers that reschedule themselves for a new customer process after finishing the previous one.

Option2
*******
Working form:
-------------
The "store manager" is responsible to push customers to a vacant cashier.
Uses a SemaphoreSlim to control the number of customers that are processed simultaneously.





Complexity (same for both options)
**********************************

Space complexity:
The trade off is the customers in-memory queue size vs. number of cashiers (threads).
The cashiers use the threadpool threads, but with many cashiers, the threadpool will need to instanciate new threads, increasing memory footprint.
Reducing the number of cashiers will result more customers waiting. Hence, the customers queue would consume more memory.

If we set the number of cashiers to be the same as the maximum cashier process time (e.g. 5 cashiers when a cashier process between 1-5 seconds), and the 
customers join rate is 1 second (as specified in the exercise), the queue size will remain fixed (0-1 customers) since there's always a vacant cashier to process a new customer. 

Since the queue size is fixed, it's memory consumption is of O(1), no matter how many customers join the queue.
Same with the memory usage of the cashier threads, since we fix the count of cashiers, the max memory usage will be also fixed - not dependent in the customers count,
i.e. O(1).

Time complexity:
As explained before, if queue size is fixed, no matter the customers joining it, the time for each customer will be constant - i.e. O(1).

If we decrease the cashiers count, the queue size will grow and a late customer will wait longer then an early one, 
meaning that a customer processing time is O(n) and the queue size (space complexity) will also be O(n).

In StoreManager.GetOptimalCashiersCount function I commented the most balanced way: set the cashiers number to the average time of cashier processing (3 seconds = 
3 cashiers), causing the queue size to not be fixed (it will grow and shrink), but in the long run it will not overflow, so the average time for customer will
be fixed - O(1). Indeed, using 5 cashiers will shorten and fix the time for ALL customers.





