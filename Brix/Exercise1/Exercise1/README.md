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
Uses a fixed set of cashiers that reschedule themselves for a new customer process after finishing the prevoius one.

Option2
*******
Working form:
-------------
The "store manager" is responsible to send customers to a vacant cashier.
Uses a SemaphoreSlim to control the amount of customers that are processed simultaneously.

Complexity (same for both options)
**********************************

Space complexity:
-----------------
The trade off is the customers in-memory queue size vs. number of cashiers (threads).
The cashiers use the threadpool threads, but with many cashiers, the threadpool will need to instanciate new threads, increasing memory foorprint.
Reducing the number of cashiers will result more customers waiting, hence, the customers queue would consume more memory.
If there are 5 cashiers when a cashier finishes to process a customer in max 5 seconds, and the customers join rate is 1 second (as specified in the exercise),
The queue size will remain fixed (0-1 customres) since there's always a vacant cashier to process a new customer. 
Since the queue size is not dependant of the customers count, it's memory consumption is of O(1).
Same with the memory usage of the cashiers threads, since we fix the count of cashiers, the max memory usage will be fixed - not dependant in the customers count,
i.e. O(1).

Time complexity
---------------
Considering the exercise parameters (5 cashiers etc.), the customers queue will stay at minimum capacity (as explained before), meaning the time complexity does not depend on 
the customers count, and therefore it's a O(1).
If we decrease the cashiers count, the queue size will grow and a late customer will wait longer then an early one, meaning that a customer processing time is O(n).





