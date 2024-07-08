#!/bin/sh

# Cleanup old output
dotnet clean 
rm -rf **/{bin,obj}

# Terminate the build server to prevent caching
dotnet build-server shutdown

# Build it
dotnet restore
dotnet build --configuration Debug --no-restore

# Show the generated output as a sanity check
find -L Sample/obj/Debug/net8.0/generated/
