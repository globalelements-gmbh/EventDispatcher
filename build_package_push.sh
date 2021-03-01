#!/bin/bash

# do not honor failure - abort early
set -e

echo "-- Verify config"
# Verify the presence of the nuget api key
if [[ -z "${NUGET_API_KEY}" ]]; then
  echo "Set NUGET_API_KEY environment variable for the api key used to publish to the Nuget gallery"
  exit 1
fi
if [[ -z "${NUGET_VERSION}" ]]; then
  echo "Set NUGET_VERSION environment variable for the desired release version"
  exit 1
fi

# Restore, build, test
echo "-- Prepare"
dotnet restore
dotnet build
dotnet test ./*Test*/*.csproj

# Build, package and push the NuGet
echo "-- Pack and publish for version: ${NUGET_VERSION}"
dotnet clean
rm -f ./GlobalElements.EventDispatcher/bin/R elease/GlobalElements.EventDispatcher*.nupkg
sed -i "s/VERSIONSTRING/${NUGET_VERSION}/g" ./GlobalElements.EventDispatcher/EventDispatcher.nuspec
dotnet build -c Release
dotnet pack ./GlobalElements.EventDispatcher/GlobalElements.EventDispatcher.csproj -c Release -p:NuspecFile=EventDispatcher.nuspec
dotnet nuget push ./GlobalElements.EventDispatcher/bin/Release/GlobalElements.EventDispatcher*.nupkg -s https://www.nuget.org -k ${NUGET_API_KEY}
