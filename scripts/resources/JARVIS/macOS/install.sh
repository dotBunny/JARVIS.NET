#!/bin/bash
cd "$(dirname "$0")"

# Unload Service
sudo launchctl stop com.dotBunny.JARVIS
sudo launchctl unload /Library/LaunchDaemons/com.dotBunny.JARVIS.plist

# Get PID 
PIDFile="/Users/reapazor/JARVIS/production/JARVIS/JARVIS.pid"

if [ ! -f $PIDFile ]; then
    echo "No PID file found, not killing process."
else
    CurrentPID=`cat $PIDFile`

    # Remove PID file
    rm -rf $PIDFile
fi

# Sleep for 1 minute for port clarity
echo Sleeping for 60 seconds to clear process 
sleep 60

# Detect db? - use database version file
ProductionDBVersionFile="/Users/reapazor/JARVIS/production/JARVIS/JARVIS.db.version"
if [ ! -f $ProductionDBVersionFile ]; then
    echo "No existing database version file found, just replacing."
else
    ProductionDBVersion=`cat $ProductionDBVersionFile`
    StagingDBVersionFile="/Users/reapazor/JARVIS/staging/JARVIS/JARVIS.db.version"
    StagingDBVersion=`cat $StagingDBVersionFile`

    echo "Found database version $ProductionDBVersion"

    # Handle DB
    if [ $StagingDBVersion \> $ProductionDBVersion ]; then
        echo "MANUAL UPGRADE REQUIRED"
        echo "-----------------------"
        echo "The current production database is in need of upgrading in order to proceed with the install."
        exit 1
    else
        rm -rf /Users/reapazor/JARVIS/staging/JARVIS/JARVIS.db
    fi
fi

# Copy Over JARVIS
cp -Rf /Users/reapazor/JARVIS/staging/JARVIS/* /Users/reapazor/JARVIS/production/JARVIS/

# Remove Quarantines
sudo xattr -dr com.apple.quarantine /Users/reapazor/JARVIS/production

# Copy plist to directory
sudo install -o root -g wheel -m 644 /Users/reapazor/JARVIS/staging/com.dotBunny.JARVIS.plist /Library/LaunchDaemons/com.dotBunny.JARVIS.plist
sudo xattr -dr com.apple.quarantine /Library/LaunchDaemons/com.dotBunny.JARVIS.plist

# Set to load at boot
sudo launchctl load -w /Library/LaunchDaemons/com.dotBunny.JARVIS.plist
sudo launchctl start com.dotBunny.JARVIS

# Exit script
exit $?