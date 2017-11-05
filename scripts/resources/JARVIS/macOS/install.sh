#!/bin/bash
cd "$(dirname "$0")"

# Get PID 
PIDFile="/Users/reapazor/JARVIS/production/JARVIS/JARVIS.pid"
CurrentPID=(<$PIDFile)

# Remove PID file
rm -rf $PIDFile

# Kill active process (launchd wont restart)
kill -9 $CurrentPID

# Detect db? - use database version file

# not db?

# copy over 

# remove script y/n?



# Copy plist to directory
# cp /System/Library/LaunchAgents

# Verify loading of script 
launchctl load /System/Library/LaunchAgents/com.dotBunny.JARVIS.plist

# Exit script
exit $?