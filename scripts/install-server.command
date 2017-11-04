#!/bin/bash
cd "$(dirname "$0")"

# SSH to server
ssh -t reapazor@jarvis "/Users/reapazor/JARVIS/staging/upgrade.sh"

# Exit script
exit $?