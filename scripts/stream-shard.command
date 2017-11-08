#!/bin/bash
cd "$(dirname "$0")"
dotnet /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS.Shard/bin/Debug/JARVIS.Shard.dll --host 192.168.1.114 --encrypt --wirecast --counters --output /Users/reapazor/Documents/StreamingData
exit $?