using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{ 
    Default,
    Material,
    Food,
    Potion,
    Relic
}
public abstract class ItemObject : ScriptableObject
{
    public GameObject itemPrefab;
    public Sprite itemSprite;
    public ItemType itemType;
    [TextArea(10, 15)]
    public string description;
    public int sellPrice;
    public int restoreHealthValue;
}
