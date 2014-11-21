# README #

[![Build status](https://ci.appveyor.com/api/projects/status/b6eydara8rqxt1ed/branch/master?svg=true)](https://ci.appveyor.com/project/TChatzigiannakis/tinyreflectivetoolkit/branch/master)

### What is this repository for? ###

* Tiny Reflective Toolkit, which is the main project.
* NUnit tests for the main project.

### How do I get set up? ###

* Install .NET Framework 4 or an equivalent version of Mono.
* Open the solution file using Visual Studio or Xamarin Studio.
* Fetch latest NuGet packages by running `nuget restore` (if your environment doesn't fetch them automatically).
* Build Solution.

### Contribution guidelines ###

* If you would like to contribute to this project in any way, you can fork this project, make your changes and then create a pull request. 
* Every new feature should include one or more tests related to it. These tests should fail to build or run in the absence of the new feature and they should successfully build and run in the presence of the new feature. 
* Every change to an existing feature should make sure not to introduce new failures in the tests.
* The public API of the main project should remain backwards compatible. If a breaking change arises, there should be a good reason for it and even then it will only be included in the next major version.

### Who do I talk to? ###

* TChatzigiannakis

### License ###

* Copyright (C) 2014  Theodoros Chatzigiannakis
* Distributed as free and open source software. For more information, see LICENSE file.
* If you would like to receive this software under a different license, feel free to contact me.
