Getting Started with AF SDK
===============================

#### Project to update code and documentation for PI AF SDK Getting Started Guide for purposes of increasing clarity.

Below is a log of changes that have been made:
-------------------------------------------------

- Changed variable names to align marketing terms with code objects
	- (i.e. changed names instances of PISystem objects to assetServer)

- Updated general program formatting to be in line with Console app conventions and variable naming convention
	- i.e. elementquery should be elementQuery to increase readability 

- Changed database names to match current example database names

- Updated FindBuildingInfo method from Lesson 2 to use the AFElementSearch.FineElements method rather than AFElement.FindElemetns
	- Updated method to fit format and conventions of other methods in solution

- Changed string[] to List<string> and updated rest of body for Lesson 1 Excercise 1
	- Note that using List<string> requires users to add the following reference: using System.Collections.Generic;

- Updated to the proper query string format in Lesson 2 example

- Removed Contains() and added null check to shorten and simplify code in Lesson 4

- Made correct call for power.DataReferencePlugin in Lesson 4 example

- Removed else clause in Exercise 1

- Simplified CreateCategories method in Bonus Exercise 2

- Re-wrote Bonus Exercise 4 and broke down into smaller methods for clarity and better programming practice

- Added conditional CheckIns where appropriate

- Removed XML documentation

- Lesson 5:
	- Cleaned up Exercise 1, 3, 4 for clarity
	- Implemented AFSearch in Exercise 2

- Added server-side caching for search objects in lesson 2 and 5 with using() {} blocks
	- Added cache timeout for any potential issues

- Simplified Lesson 1 PrintElementTemplates() to just use AFElementTemplate data type instead of a list

- Used the correct search method (AFElementSearch) in Lesson 3

- Removed new line characters from print statements in Lesson 1 for consistency

- Added comment for clarification to GetAttributes() helper method in Lesson 3

- Added null checks and ?? syntax in Lesson 4 and Bonus to remove exceptions when code is run more than once