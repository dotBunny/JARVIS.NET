#!/bin/bash
cd "$(dirname "$0")"


# Get PID 

# Remove PID file

# Kill active process (launchd wont restart)

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