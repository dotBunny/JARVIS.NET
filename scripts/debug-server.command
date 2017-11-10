#!/bin/bash
cd "$(dirname "$0")"
dotnet /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS/bin/Debug/netcoreapp2.0/JARVIS.dll --sql "/Users/reapazor/Dropbox/dotBunny - Projects/JARVIS/Build/jarvis.sql" --host localhost
exit $?