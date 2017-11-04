#!/bin/bash
cd "$(dirname "$0")"

# Publish application in release mode
dotnet publish /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS/ -f netcoreapp2.0 -c Release --output /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/JARVIS

# Copy startup script into folder
cp ./resources/server-start.sh /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/start.sh
cp ./resources/server-upgrade.sh /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/upgrade.sh

# Syncronize with server
rsync -rzP --delete /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/ reapazor@jarvis:/Users/reapazor/JARVIS/staging

# Remove deployment folder
rm -rf /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/

# Exit script
exit $?