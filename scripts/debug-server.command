#!/bin/bash
cd "$(dirname "$0")"
mono /Users/reapazor/Repositories/dotBunny/JARVIS.NET/JARVIS/bin/Debug/JARVIS.exe --host localhost
exit $?