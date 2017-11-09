#!/bin/bash
cd "$(dirname "$0")"
dotnet /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS.Shard/bin/Debug/netcoreapp2.0/JARVIS.Shard.dll --host localhost --encrypt --wirecast --counters --output /Users/reapazor/Documents/StreamingData
exit $?