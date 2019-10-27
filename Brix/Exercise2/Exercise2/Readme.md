Time Complexity
***************
The most efficient time complexity is O(1), using a hash-code based dictionary. That's the path I took. 
Each dictionary's key is a sorted version of a 5 chararcters combination. 
This ensures there're less than 1000000 entries in the dictionary (file lines with the same characters will be stored under the same key). 

Other option was to use a sorted list, build form each file line all the possible combinations (upper and lower cases, all possible orders),
and then search in that list, but this would be a O(log n) complexity (and the sorted list would take a lot of memory - about 1000000 * 5! - minus the 
reoccuring combinations).