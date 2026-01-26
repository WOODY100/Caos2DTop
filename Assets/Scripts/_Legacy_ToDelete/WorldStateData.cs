using System;
using System.Collections.Generic;

[Serializable]
public class WorldStateData
{
    public HashSet<string> openedChests = new();
}