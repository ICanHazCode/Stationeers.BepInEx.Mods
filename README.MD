# My Stationeers Mods #

This requires BepInEx 5.4 or better.

## BepInEx ##
![Github All Releases](https://img.shields.io/github/downloads/bepinex/bepinex/total.svg)
[![GitHub release](https://img.shields.io/github/release/bepinex/bepinex.svg)](https://github.com/BepInEx/BepInEx/releases/latest)
[![BepInEx Discord](https://user-images.githubusercontent.com/7288322/34429117-c74dbd12-ecb8-11e7-896d-46369cd0de5b.png)](https://discord.gg/MpFEDAg)

[Get it here!](https://github.com/BepInEx/BepInEx/releases/latest)

Instructions to install BepInEx: [Stationeers.BepInEx](https://github.com/ICanHazCode/Stationeers.BepInEx#bepinex-install-instructions)
---

## Difficulty Starting Conditions
Mod That allows mod-added DifficultySettings with their own StartingConditions to apply the conditions to a new game world. This means that any difficulty that has StartingConditions that aren't "Default" will make the new game use those conditions instead of the "Default" or world's StartingConditions settings that Stationeers enforces. If both the Difficulty Setting **_and_** the selected world contain non-"Default" conditions, this mod will ask you which one to use. If the world has non-"Default" and the selected difficulty is "Default" then the world applies, and if the world is "Default" and the difficulty is not, then the difficulty setting applies.

## Workshop Mod Load Order
This mod fixes the issue with certain mods configurations not working (mainly recipes). This is because Stationeers **always** loads Core first, no matter where it is in the Workshop list. This mod enforces the load order, even for Core, so that mods that change default settings will work. The mods are loaded in reverse order, like original, so this means that the last in the list is loaded first, and the first, last.
