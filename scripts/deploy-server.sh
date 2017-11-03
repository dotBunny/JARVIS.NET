#!/bin/bash
dotnet publish /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS/ -f netcoreapp2.0 -c Release --output /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/JARVIS
rsync -rzP --delete /Users/reapazor/Repositories/dotBunny/JARVIS.NET/deploy/JARVIS reapazor@jarvis:/Users/reapazor/JARVIS/staging