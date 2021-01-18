#!/bin/bash

# do not honor failure - abort early
set -e

# Restore, build, test
dotnet restore
dotnet build
dotnet test ./*Test*/*.csproj

# Verify the presence of the nuget api key
if [[ -z "${NUGET_API_KEY}" ]]; then
  echo "Set NUGET_API_KEY environment variable for the api key used to publish to the Nuget gallery"
  exit 1
fi

# Build, package and push the NuGet
dotnet clean
rm -f ./GlobalElements.EventDispatcher/bin/Release/GlobalElements.EventDispatcher*.nupkg
dotnet build -c Release
dotnet pack ./GlobalElements.EventDispatcher/GlobalElements.EventDispatcher.csproj -c Release -p:NuspecFile=EventDispatcher.nuspec
dotnet nuget push ./GlobalElements.EventDispatcher/bin/Release/GlobalElements.EventDispatcher*.nupkg -s https://www.nuget.org -k ${NUGET_API_KEY}
