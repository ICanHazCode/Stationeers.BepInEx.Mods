# Stationeers Workshop Mod Load Order
This fixes the issue where Recipe mods can't change recipes found in the core game. This is because Stationeers **_always_** loads Core first, regardless of where it is located in the Workshop mod list.

This fixes that behavior to conform to the workshop load order. Core will now load in it's proper location of the list.

Note: The Load order is **_reverse_** of list order. This means that the last item on the list gets loaded first, and the first item, last.
