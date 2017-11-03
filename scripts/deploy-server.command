#!/bin/bash
cd "$(dirname "$0")"
dotnet publish /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS/ -f netcoreapp2.0 -c Release --output /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/JARVIS
cp ./resources/server-start.sh /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/JARVIS/start.sh
rsync -rzP --delete /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/JARVIS reapazor@jarvis:/Users/reapazor/JARVIS/staging
rm -rf /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/
exit $?