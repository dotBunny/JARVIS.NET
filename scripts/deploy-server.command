#!/bin/bash
cd "$(dirname "$0")"

# Publish application in release mode
dotnet publish /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS/ -f netcoreapp2.0 -c Release --output /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/JARVIS

# Copy startup script into folder
cp ./resources/JARVIS/macOS/start.sh /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/start.sh
cp ./resources/JARVIS/macOS/install.sh /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/install.sh
cp ./resources/JARVIS/macOS/launchd.plist /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/com.dotBunny.JARVIS.plist

# Syncronize with server
rsync -rzP --delete /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/ reapazor@jarvis:/Users/reapazor/JARVIS/staging

# Remove deployment folder
rm -rf /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/

# Exit script
exit $?