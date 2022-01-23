using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLogObject : MonoBehaviour
{
    List<QuestSlot> sideQuestsContainer = new List<QuestSlot>();
    List<QuestSlot> mainQuestsContainer = new List<QuestSlot>();

    public List<QuestSlot> SideQuestsContainer => sideQuestsContainer;
    public List<QuestSlot> MainQuestsContainer => mainQuestsContainer;

    public void AddQuest(QuestObject quest) 
    {
        bool hasQuest = false;

        for (int i = 0; i < sideQuestsContainer.Count; i++)
        {
            if (sideQuestsContainer[i].quest == quest)
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
                sideQuestsContainer.Add(new QuestSlot(quest));
                Debug.Log(sideQuestsContainer[0].quest.questTitle);
            }
            else 
            {
                mainQuestsContainer.Add(new QuestSlot(quest));
                Debug.Log(mainQuestsContainer[0].quest.questTitle);
            }   
        }
    }
}

public class QuestSlot
{
    public QuestObject quest;

    public QuestSlot(QuestObject _quest)
    {
        quest = _quest;
    }
        

}
