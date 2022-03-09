using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Database", menuName ="Database/Items Database")]
public class ItemsDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemDataObject[] Items;
    public Dictionary<ItemDataObject, int> GetId = new Dictionary<ItemDataObject, int>();
    public Dictionary<int, ItemDataObject> GetItem = new Dictionary<int, ItemDataObject>();

    //public Dictionary<int, ItemDataObject> UniqueIDGetItem = new Dictionary<int, ItemDataObject>();

    public void OnAfterDeserialize() 
    {
        GetId = new Dictionary<ItemDataObject, int>();
        GetItem = new Dictionary<int, ItemDataObject>();
        for (int i = 0; i < Items.Length; i++) 
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);

            //UniqueIDGetItem.Add(Items[i].IDNUM, Items[i]);

            //GetId.Add(Items[i], i);
            //GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize() 
    { 
        
    }
}
