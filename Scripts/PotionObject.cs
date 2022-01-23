using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inv Object", menuName = "InventorySys/Items/Potion")]
public class PotionObject : ItemObject
{
    private void Awake()
    {
        itemType = ItemType.Potion;
    }
}
