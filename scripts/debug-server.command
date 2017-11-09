#!/bin/bash
cd "$(dirname "$0")"
dotnet /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS/bin/Debug/netcoreapp2.0/JARVIS.dll --settings "/Users/reapazor/Dropbox/dotBunny - Projects/JARVIS/Build/jarvis-settings.json" --host localhost
exit $?