#!/bin/bash
cd "$(dirname "$0")"
Botnet /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS.Shard/bin/Debug/JARVIS.Shard.dll --host localhost --encrypt --wirecast --counters --output /Users/reapazor/Documents/StreamingData
exit $?