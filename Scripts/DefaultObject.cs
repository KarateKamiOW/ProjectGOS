using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inv Object", menuName ="InventorySys/Items/Default")]
public class DefaultObject : ItemObject
{
    // Start is called before the first frame update
    public void Awake()
    {
        itemType = ItemType.Default;
    }
}
