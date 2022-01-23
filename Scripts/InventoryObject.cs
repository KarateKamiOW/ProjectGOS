using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "InventorySys/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public ItemsDatabase database;
    public string savePath;

    public void AddItem(ItemObject _item, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                return; //Ends function here
            }
        }

        Container.Add(new InventorySlot(database.GetId[_item], _item, _amount));
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Count; i++)
            Container[i].item = database.GetItem[Container[i].ID];

    }

    public void OnBeforeSerialize()
    {

    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    private void OnEnable()
    {
#if UNITY_EDITOR        
        database = (ItemsDatabase)AssetDatabase.LoadAssetAtPath("Assets/Resources/ItemsDatabase.asset", typeof(ItemsDatabase));
#else
        database = Resources.Load<ItemsDatabase>("Database"); 
#endif
    }

    [System.Serializable]
    public class InventorySlot
    {
        public int ID;
        public ItemObject item;
        public int amount;
        public InventorySlot(int _ID, ItemObject _item, int _amount)
        {
            ID = _ID;
            item = _item;
            amount = _amount;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }

    }
}
