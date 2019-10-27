Time Complexity
***************
The most efficient time complexity is O(1), using a hash-code based dictionary. That's the path I took. 
Each dictionary's key is a sorted version of a 5 characters combination. 
This ensures there're less than 1000000 entries in the dictionary (file lines with the same characters will be stored as a list under the same key). 

Other option was: build all the possible combinations from each file's line (upper and lower cases, all possible orders), and store them in a sorted list.
Search using that list would be a O(log n) complexity (and the sorted list would take a lot of memory - about 1000000 * 5! strings - minus the recurring combinations).