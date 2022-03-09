using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestLogUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject questPanel1Task;
    [SerializeField] GameObject questPanel2Task;
    [SerializeField] GameObject questPanel3Task;
    [SerializeField] Transform mainQuestsContentContainer;
    [SerializeField] Transform sideQuestsContentContainer;
    [SerializeField] GameObject uiDivider;
    [SerializeField] GameObject mainQuestsGO;
    [SerializeField] GameObject sideQuestsGO;
    public QuestLogObject playerQuestLog;

    float questSpacing = .5f;
    List<GameObject> mainQuestsPanelsList = new List<GameObject>();
    List<GameObject> sideQuestsPanelsList = new List<GameObject>();

    private void Start()
    {
        //playerQuestLog = FindObjectOfType<PlayerController>().QuestLog;
        //playerQuestLog = PlayerController.instance.QuestLog;    //Both should work
    }


    public void GenerateQuestPanelsUI()
    {
        OpenUI();

        //For Main Quests
        for (int i = 0; i < playerQuestLog.MainQuestsContainer.Count; i++)
        {
            GameObject obj = Instantiate(TypeOfPanel(playerQuestLog.MainQuestsContainer[i].quest.numOfSteps));
            obj.transform.SetParent(mainQuestsContentContainer.gameObject.transform, false);
            var objInfo = obj.GetComponent<QuestPanel>();

            objInfo.questTitle.text = playerQuestLog.MainQuestsContainer[i].quest.questTitle;
            objInfo.questTask.text = playerQuestLog.MainQuestsContainer[i].quest.task;
            objInfo.questGivingNpcSprite.sprite = playerQuestLog.MainQuestsContainer[i].quest.questGiverSprite;
            objInfo.progressDescriptions[0].text = playerQuestLog.MainQuestsContainer[i].quest.questDetails.QuestStepDetailed(0);

            if(playerQuestLog.MainQuestsContainer[i].quest.questDetails.questTasks[1].taskComplete)
            objInfo.questCompleteSymbol[0].gameObject.SetActive(true);

            if (playerQuestLog.MainQuestsContainer[i].quest.numOfSteps == NumOfSteps._2) 
            {
                objInfo.progressDescriptions[1].text = playerQuestLog.MainQuestsContainer[i].quest.questDetails.QuestStepDetailed(1);
                if (playerQuestLog.MainQuestsContainer[i].quest.questDetails.questTasks[1].taskComplete)
                    objInfo.questCompleteSymbol[1].gameObject.SetActive(true);
            }
  
            if (playerQuestLog.MainQuestsContainer[i].quest.numOfSteps == NumOfSteps._3) 
            {
                objInfo.progressDescriptions[1].text = playerQuestLog.MainQuestsContainer[i].quest.questDetails.QuestStepDetailed(1);
                objInfo.progressDescriptions[2].text = playerQuestLog.MainQuestsContainer[i].quest.questDetails.QuestStepDetailed(2);

                if (playerQuestLog.MainQuestsContainer[i].quest.questDetails.questTasks[1].taskComplete)
                    objInfo.questCompleteSymbol[1].gameObject.SetActive(true);

                if (playerQuestLog.MainQuestsContainer[i].quest.questDetails.questTasks[2].taskComplete)
                    objInfo.questCompleteSymbol[2].gameObject.SetActive(true);
            }
                
            mainQuestsPanelsList.Add(obj);
        }

        //For Side Quests
        for (int i = 0; i < playerQuestLog.SideQuestsContainer.Count; i++)
        {
            //Debug.Log("SideQuestCount = " + playerQuestLog.SideQuestsContainer.Count);
            GameObject obj = Instantiate(TypeOfPanel(playerQuestLog.SideQuestsContainer[i].quest.numOfSteps));
            obj.transform.SetParent(sideQuestsContentContainer.gameObject.transform, false);
            var objInfo = obj.GetComponent<QuestPanel>();

            objInfo.questTitle.text = playerQuestLog.SideQuestsContainer[i].quest.questTitle;
            objInfo.questTask.text = playerQuestLog.SideQuestsContainer[i].quest.task;
            objInfo.questGivingNpcSprite.sprite = playerQuestLog.SideQuestsContainer[i].quest.questGiverSprite;

            objInfo.progressDescriptions[0].text = playerQuestLog.SideQuestsContainer[i].quest.questDetails.QuestStepDetailed(0);

            if (playerQuestLog.SideQuestsContainer[i].quest.numOfSteps == NumOfSteps._2)
                objInfo.progressDescriptions[1].text = playerQuestLog.SideQuestsContainer[i].quest.questDetails.QuestStepDetailed(1);
            if (playerQuestLog.SideQuestsContainer[i].quest.numOfSteps == NumOfSteps._3)
            {
                objInfo.progressDescriptions[1].text = playerQuestLog.SideQuestsContainer[i].quest.questDetails.QuestStepDetailed(1);
                objInfo.progressDescriptions[2].text = playerQuestLog.SideQuestsContainer[i].quest.questDetails.QuestStepDetailed(2);
            }

            sideQuestsPanelsList.Add(obj);
        }
    }

    public void DestroyPanels()
    {

        for (int i = 0; i < mainQuestsPanelsList.Count; i++) 
        {
            Destroy(mainQuestsPanelsList[i]);
        }

        for (int i = 0; i < sideQuestsPanelsList.Count; i++) 
        {
            Destroy(sideQuestsPanelsList[i]);
        }
    }

    public GameObject TypeOfPanel(NumOfSteps amtOfSteps) 
    {
        if (amtOfSteps == NumOfSteps._1)
            return questPanel1Task;
        else if (amtOfSteps == NumOfSteps._2)
            return questPanel2Task;
        else if (amtOfSteps == NumOfSteps._3)
            return questPanel3Task;
        else 
        {
            Debug.Log("Error. Too Many/Little steps");
            return null;
        }
        
    }

    public void CloseUI() 
    {
        LeanTween.scale(uiDivider, Vector3.zero, .1f);
        LeanTween.scale(mainQuestsGO, Vector3.zero, .1f);
        LeanTween.scale(sideQuestsGO, Vector3.zero, .1f);

        DestroyPanels();
    }

    void OpenUI() 
    {
        LeanTween.scale(uiDivider, Vector3.one, .25f);
        LeanTween.scale(mainQuestsGO, Vector3.one, .25f);
        LeanTween.scale(sideQuestsGO, Vector3.one, .25f);
    }

    void CheckForTaskCompletion(int taskNum) 
    { 
        
    }
}
