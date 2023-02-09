#!/bin/bash
# This script sets up the directory links for BepInEx
echo off
echo "This script sets directory links in BepInEx to the steam workshop for Stationeers."
echo "This will allow BepInEx-enabled workshop items to be run by just Subscribing from the Steam workshop."
BEPINEXDIR=$(pwd)
PATCHERSDIR="$BEPINEXDIR/patchers/workshop"
PLUGINSDIR="$BEPINEXDIR/plugins/workshop"
cd ..\..\..\workshop\content\544550
Workshop=$(pwd)
sudo ln -s "$Workshop" "$PATCHERSDIR"
sudo ln -s "$Workshop" "$PLUGINSDIR"

echo "Completed link setup."
rm -- "$0"



