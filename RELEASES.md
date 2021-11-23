# Release Notes

# v2

## v2.0.0 - 2021-03-01

### Improvements

* Add support for .Net Core 5.0
* Add support for .Net Core 3.1
* Implicit scan assemblies if not already scanned when dispatching the first event

### Breaking changes

* Remove support for .Net Core 2.x and .Net Core 3.0
* Replace log4net with Serilog
* Replace StructureMap with Lamar

# v1

## v1.0.2 - 2020-11-13

### Bugfixes

* If an exception occurs during dispatch, pass the exception to the logger

## v1.0.1 - 2020-05-01

### Bugfixes

* fixes a typo in nuspec which caused nupkg to be invalid

## v1.0.0 - 2020-05-01 (withdrawn)

* Initial release