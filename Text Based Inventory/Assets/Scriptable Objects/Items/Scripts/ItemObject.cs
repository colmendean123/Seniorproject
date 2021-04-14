using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion,
    Weapon,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public string name;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
}
