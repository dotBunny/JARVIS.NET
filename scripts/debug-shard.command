#!/bin/bash
cd "$(dirname "$0")"
mono /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS.Shard/bin/Debug/JARVIS.Shard.exe --wirecast --counters --output /Users/reapazor/Documents/StreamingData
exit $?