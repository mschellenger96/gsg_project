Project to update code and documentation for PI AF SDK Getting Started Guide for purposes of increasing clarity.

Below is a log of changes that have been made:

	- Changed variable names to align marketing terms with code objects
		- (i.e. changed names instances of PISystem objects to assetServer)

	- Updated general program formatting to be in line with Console app conventions and variable naming convention
		- i.e. elementquery should be elementQuery to increase readability 

	- Changed database names to match current example database names

	- Updated FindBuildingInfo method from Lesson 2 to use the AFElementSearch.FineElements method rather than AFElement.FindElemetns
		- Updated method to fit format and conventions of other methods in solution