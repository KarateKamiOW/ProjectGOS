using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using UnityEditor;

[CreateAssetMenu(fileName = "New QuestLog", menuName = "QuestLog")]
public class QuestLogObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    public List<QuestSlot> SideQuestsContainer = new List<QuestSlot>();
    public List<QuestSlot> MainQuestsContainer = new List<QuestSlot>();
    List<LocationMapObject> allUnlockedLocationMaps = new List<LocationMapObject>();
    List<LorePageSlot> allLorePagesAndProgress = new List<LorePageSlot>();
    //public List<InventorySlot> Container = new List<InventorySlot>();

    QuestsDatabase questsDB;
    LocationMapDatabase nlmDB;
    LorePagesDatabase lorePageDB;

    public List<LocationMapObject> AllUnlockedLocationMaps => allUnlockedLocationMaps;
    public List<LorePageSlot> AllLorePagesAndProgress => allLorePagesAndProgress;

    public int EarthLoreScore { get; set; }
    public int EarthLoreCurrentTier { get; set; }
    public int SunLoreScore { get; set; }
    public int SunLoreCurrentTier { get; set; }
    public int MoonLoreScore { get; set; }
    public int MoonLoreCurrentTier { get; set; }
    //public Dictionary<LorePageObject, int> GetLorePageProgressNum => getLorePageProgressNum;

    //NLMs stands for New Location Maps
    //Location Maps are the scriptable objects needed to traverse to different maps
    public void AddQuest(QuestObject quest) 
    {
        bool hasQuest = false;

        for (int i = 0; i < SideQuestsContainer.Count; i++)
        {
            if (SideQuestsContainer[i].quest == quest)
            {
                hasQuest = true;
                Debug.Log("Player already has quest");
                break;
            }
        }
        for (int i = 0; i < MainQuestsContainer.Count; i++)
        {
            if (MainQuestsContainer[i].quest == quest)
            {
                hasQuest = true;
                Debug.Log("Player already has quest");
                break;
            }
        }
        if (!hasQuest)
        {
            if (quest.mainOrSideQuest == MainOrSideQuest.Side) 
            {
                //Container.Add(new InventorySlot(newItemsdatabase.GetId[_item], _item, _amount));
                SideQuestsContainer.Add(new QuestSlot(questsDB.GetId[quest], quest));
                //sideQuestsContainer.Add(new QuestSlot(quest.questID, quest));

                PlayerController.instance.overworldEventEffectsMan.ShowQuestBanner(quest.questTitle, false);
                Save();
                
            }
            else 
            {
                MainQuestsContainer.Add(new QuestSlot(questsDB.GetId[quest], quest));
                //mainQuestsContainer.Add(new QuestSlot(quest.questID, quest));

                PlayerController.instance.overworldEventEffectsMan.ShowQuestBanner(quest.questTitle, true);
                Save();
                
            }   
        }
    }

    public void ResetAll() 
    {
        for (int i = 0; i < MainQuestsContainer.Count; i++) 
        {
            MainQuestsContainer[i].quest.questDetailsGameObj.GetComponent<QuestDetails>().ResetAllStepProgress();
        }
        for (int i = 0; i < SideQuestsContainer.Count; i++) 
        {
            SideQuestsContainer[i].quest.questDetailsGameObj.GetComponent<QuestDetails>().ResetAllStepProgress();
        }
        MainQuestsContainer.Clear();
        SideQuestsContainer.Clear();
        Save();
    }

    public void RemoveQuest(bool isMain, int pos) 
    {
        if (isMain)
        {
            MainQuestsContainer[pos].quest.questDetailsGameObj.GetComponent<QuestDetails>().ResetAllStepProgress();
            MainQuestsContainer.RemoveAt(pos);
        }
        else 
        {
            SideQuestsContainer[pos].quest.questDetailsGameObj.GetComponent<QuestDetails>().ResetAllStepProgress();
            SideQuestsContainer.RemoveAt(pos);
        }
        Save();
    }

    public void UnlockNLM(LocationMapObject NLMToUnlock) 
    {
        for (int i = 0; i < allUnlockedLocationMaps.Count; i++)
        {
            if (allUnlockedLocationMaps[i] == NLMToUnlock)
            {
                Debug.Log("Location Map already Unlocked!");
                return;
            }
        }
        allUnlockedLocationMaps.Add(NLMToUnlock);
        PlayerController.instance.overworldEventEffectsMan.ShowLocationMapBanner(NLMToUnlock.LocationMapName);
        Save();

    }

    public void UnlockLorePage(LorePageObject lorePageToUnlock) 
    {
        Debug.Log(lorePageToUnlock + " Should be unlocked");
        for (int i = 0; i < allLorePagesAndProgress.Count; i++) 
        {
            if (allLorePagesAndProgress[i].lorePage == lorePageToUnlock) 
            {
                Debug.Log("Lore Page found, set to unlocked");
                if (allLorePagesAndProgress[i].progressNum == 0)
                    allLorePagesAndProgress[i].progressNum = 1;
                //Apply Effect Here
            }
        }
        /*
        if (getLorePageProgressNum[lorePageToUnlock] == 0)
            getLorePageProgressNum[lorePageToUnlock] = 1;*/
        
        Save();
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
            //Debug.Log("File Exists!");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
        
    }

    public void OnAfterDeserialize()
    {
        //for (int i = 0; i < allUnlockedLocationMaps.Count; i++)
        //allUnlockedLocationMaps[i] = nlmDB.GetMap[nlmDB.GetId];

        for (int i = 0; i < MainQuestsContainer.Count; i++)
            MainQuestsContainer[i].quest = questsDB.GetQuest[MainQuestsContainer[i].ID];

        for (int i = 0; i < SideQuestsContainer.Count; i++)
            SideQuestsContainer[i].quest = questsDB.GetQuest[SideQuestsContainer[i].ID];

    }

    public void OnBeforeSerialize()
    {
        
    }

    private void OnEnable()
    {
        questsDB = Resources.Load<QuestsDatabase>("QuestsDatabase");
        nlmDB = Resources.Load<LocationMapDatabase>("NLMDatabase");
        lorePageDB = Resources.Load<LorePagesDatabase>("LorePagesDatabase");
        if (allLorePagesAndProgress.Count < lorePageDB.LorePages.Length)
        {
            Debug.Log("Initialized");
            //allLorePagesAndProgress.Clear();
            for (int i = 0; i < lorePageDB.LorePages.Length; i++)
            {
                allLorePagesAndProgress.Add(new LorePageSlot(lorePageDB.LorePages[i], 0));
                //getLorePageProgressNum.Add(lorePageDB.LorePages[i], 0);
            }
        }
        
    }

    #region Testing Functions
    public void UnlockAllNLMs()
    {
        //Debug.Log("All Stylists Unlocked!");
        /*for (int i = 0; i < nlmDB.NLMs.Length; i++)
        {
            allUnlockedLocationMaps.Add(nlmDB.NLMs[i]);
        }*/
    }

    public void ClearAllLocationMaps()
    {
        allUnlockedLocationMaps.Clear();
        ClearAllLoreCollectionData();
    }

    public void ClearAllLoreCollectionData() 
    {
        allLorePagesAndProgress.Clear();
    }
    #endregion

    [System.Serializable]
    public class QuestSlot
    {
        public QuestObject quest;
        public int ID;

        public QuestSlot(int questID, QuestObject _quest)
        {
            ID = questID;
            quest = _quest;
        }
    }

    [System.Serializable]
    public class LorePageSlot 
    {
        public LorePageObject lorePage;
        public int progressNum;

        public LorePageSlot(LorePageObject _lorePage, int unlockNum) 
        { 
            lorePage = _lorePage;
            progressNum = unlockNum;
        }
    }
}


