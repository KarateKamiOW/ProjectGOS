using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestBoardPanelUI : MonoBehaviour
{
    public TextMeshProUGUI locationMapNameText;
    public TextMeshProUGUI mapDescriptionText;
    public Image backgroundImage;
    public List<Sprite> panelBGSprites;

    [Header("Panel GameObjects")]
    public GameObject detailsPanel;
    public GameObject descriptionPanel;
    public GameObject readyUpPanel;
    public GameObject imagePanel;

    [Header("Containers")]
    public Transform enemiesContainer;
    public Transform rewardsContainer;
    public Transform environmentsContainer;

    [Header("Panel Flipping Behaviors")]
    public PanelFlipBehavior detailsFlipper;
    public PanelFlipBehavior descriptionFlipper;

    List<Sprite> encounterableEnemiesList = new List<Sprite>();
    List<Sprite> rewardList = new List<Sprite>();

    List<GameObject> tempListOfEnemyImagesToDestroy = new List<GameObject>();
    List<GameObject> tempListOfRewardImagesToDestroy = new List<GameObject>();
    List<GameObject> tempListOfEnvironmentImagesToDestroy = new List<GameObject>();
    public QuestBoardUI QuestBoardUserInterface { get; set; }

    public void Start()
    {
        //detailsFlipper.questBoardPanelUI = this;
        //descriptionFlipper.questBoardPanelUI = this;
        QuestBoardUserInterface = GetComponentInParent<QuestBoardUI>();
        detailsFlipper.readyToFlip = false;
        descriptionFlipper.readyToFlip = false;
    }



    public void UpdatePanelDifficulty(LocationMapObject.LMDifficulty locationDifficulty)
    {
        switch (locationDifficulty)
        {
            case LocationMapObject.LMDifficulty.Yellow:
                backgroundImage.sprite = panelBGSprites[1];
                break;
            case LocationMapObject.LMDifficulty.Green:
                backgroundImage.sprite = panelBGSprites[2];
                break;
            case LocationMapObject.LMDifficulty.Blue:
                backgroundImage.sprite = panelBGSprites[3];
                break;
            case LocationMapObject.LMDifficulty.Red:
                backgroundImage.sprite = panelBGSprites[4];
                break;
            case LocationMapObject.LMDifficulty.Black:
                backgroundImage.sprite = panelBGSprites[5];
                break;
            default:
                backgroundImage.sprite = panelBGSprites[0];
                break;

        }
    }

    public void UpdateEncounterableEnemiesList(LocationMap locaMap)
    {
        for (int i = 0; i < locaMap.Base.T1EnemiesList.Count; i++)
        {
            encounterableEnemiesList.Add(locaMap.Base.T1EnemiesList[i].EnemySprite);
        }
        for (int i = 0; i < locaMap.Base.T2EnemiesList.Count; i++)
        {
            encounterableEnemiesList.Add(locaMap.Base.T2EnemiesList[i].EnemySprite);
        }
        for (int i = 0; i < locaMap.Base.T3EnemiesList.Count; i++)
        {
            encounterableEnemiesList.Add(locaMap.Base.T3EnemiesList[i].EnemySprite);
        }

    }
    void InstantiateListOfEnemies() 
    {
        for (int i = 0; i < encounterableEnemiesList.Count; i++)
        {
            GameObject spriteObj = Instantiate(imagePanel);
            spriteObj.transform.SetParent(enemiesContainer.gameObject.transform, false);

            var objIMG = spriteObj.GetComponent<Image>();

            objIMG.sprite = encounterableEnemiesList[i];

            tempListOfEnemyImagesToDestroy.Add(spriteObj);
        }

    }

    public void UpdatePossibleRewardsList(LocationMap locaMap)
    {
        if (locaMap.Base.T3EnemiesList.Count != 0)
        {
            for (int i = 0; i < locaMap.Base.T3EnemiesList.Count; i++)
            {
                for (int j = 0; j < locaMap.Base.T3EnemiesList[j].EnemyDrops.Count; j++)
                {
                    rewardList.Add(locaMap.Base.T3EnemiesList[j].EnemyDrops[j].itemDrop.itemSprite);
                }
            }
            return;

        }

        if (locaMap.Base.T2EnemiesList.Count != 0)
        {
            for (int i = 0; i < locaMap.Base.T2EnemiesList.Count; i++)
            {
                for (int j = 0; j < locaMap.Base.T2EnemiesList[j].EnemyDrops.Count; j++)
                {
                    rewardList.Add(locaMap.Base.T2EnemiesList[j].EnemyDrops[j].itemDrop.itemSprite);
                }
            }
            return;
        }


        for (int i = 0; i < locaMap.Base.T1EnemiesList.Count; i++)
        {
            for (int j = 0; j < locaMap.Base.T1EnemiesList[i].EnemyDrops.Count; j++)
            {
                rewardList.Add(locaMap.Base.T1EnemiesList[i].EnemyDrops[j].itemDrop.itemSprite);
            }
        }
    }
    void InstantiateListOfRewards() 
    {
        for (int i = 0; i < rewardList.Count; i++)
        {
            GameObject spriteObj = Instantiate(imagePanel);
            spriteObj.transform.SetParent(rewardsContainer.gameObject.transform, false);

            var objIMG = spriteObj.GetComponent<Image>();

            objIMG.sprite = rewardList[i];

            tempListOfRewardImagesToDestroy.Add(spriteObj);
        }
    }

    #region Open And Close Button Functions
    public void OpenDetailsPanel()
    {
        if (QuestBoardUserInterface.BoardState == QuestBoardPanelState.Base)
        {
            Debug.Log("Open The Details!");
            InstantiateListOfEnemies();
            InstantiateListOfRewards();

            QuestBoardUserInterface.BoardState = QuestBoardPanelState.DetailsPanelState;
            detailsPanel.transform.SetParent(QuestBoardUserInterface.contentContainer.gameObject.transform, false);
            LeanTween.scale(detailsPanel, Vector3.one, .1f);

            QuestBoardUserInterface.APanelIsOpen = true;

            StartCoroutine(WaitBeforeFlipDetailsAgain());
        }
        /*
        else 
        {
            if (QuestBoardUserInterface.BoardState == QuestBoardPanelState.DescriptionPanelState) 
            {
                Debug.Log("Open The Details!");
                InstantiateListOfEnemies();
                InstantiateListOfRewards();

                QuestBoardUserInterface.BoardState = QuestBoardPanelState.DetailsPanelState;
                detailsPanel.transform.SetParent(QuestBoardUserInterface.contentContainer.gameObject.transform, false);
                LeanTween.scale(detailsPanel, Vector3.one, .1f);

                StartCoroutine(WaitBeforeFlipDetailsAgain());

            }
        }*/
        /*if (QuestBoardUserInterface.BoardState == QuestBoardPanelState.Base)
        {
            InstantiateListOfEnemies();
            InstantiateListOfRewards();

            QuestBoardUserInterface.BoardState = QuestBoardPanelState.DetailsPanelState;
            detailsPanel.transform.SetParent(QuestBoardUserInterface.contentContainer.gameObject.transform, false);
            LeanTween.scale(detailsPanel, Vector3.one, .1f);

            detailsFlipper.readyToFlip = true;

        }*/
    }

    public void CloseDetailsPanel()
    {
        DestroyDetailsImages();
        detailsPanel.transform.SetParent(this.gameObject.transform, false);
        LeanTween.scale(detailsPanel, Vector3.zero, .15f);

    }

    public void DestroyDetailsImages() 
    {
        for (int i = 0; i < tempListOfEnemyImagesToDestroy.Count; i++) 
        { 
            Destroy(tempListOfEnemyImagesToDestroy[i]);
        }

        for (int i = 0; i < tempListOfRewardImagesToDestroy.Count; i++) 
        {
            Destroy(tempListOfRewardImagesToDestroy[i]);
        }
        /*for (int i = 0; i < tempListOfEnvironmentImagesToDestroy.Count; i++) 
        { 
            Destroy(tempListOfEnvironmentImagesToDestroy[i]);
        }*/ //Uncomment once this feature is added!
    }
    public void OpenDescriptionPanel()
    {
        if (QuestBoardUserInterface.BoardState == QuestBoardPanelState.DetailsPanelState)
        {


            Debug.Log("Description!");
            QuestBoardUserInterface.BoardState = QuestBoardPanelState.DescriptionPanelState;
            descriptionPanel.transform.SetParent(QuestBoardUserInterface.contentContainer.gameObject.transform, false);
            LeanTween.scale(descriptionPanel, Vector3.one, .1f);

            StartCoroutine(WaitBeforeFlipDescAgain());
        }

        /*if (QuestBoardUserInterface.BoardState == QuestBoardPanelState.DetailsPanelState)
        {
            Debug.Log("Description!");
            QuestBoardUserInterface.BoardState = QuestBoardPanelState.DescriptionPanelState;
            descriptionPanel.transform.SetParent(QuestBoardUserInterface.contentContainer.gameObject.transform, false);
            LeanTween.scale(descriptionPanel, Vector3.one, .1f);

            descriptionFlipper.readyToFlip = true;
        }*/
    }

    public void CloseDescriptionPanel() 
    {
        descriptionPanel.transform.SetParent(this.gameObject.transform, false);
        LeanTween.scale(descriptionPanel, Vector3.zero, .15f);
    }
    public void OpenReadyUpPanel()
    {
        if (QuestBoardUserInterface.BoardState == QuestBoardPanelState.Base)
        {
            QuestBoardUserInterface.BoardState = QuestBoardPanelState.ReadyUpState;
            readyUpPanel.transform.SetParent(QuestBoardUserInterface.contentContainer.gameObject.transform, false);
            LeanTween.scale(readyUpPanel, Vector3.one, .1f);
        }
    }

    public void CloseReadyUpPanel() 
    {
        readyUpPanel.transform.SetParent(this.gameObject.transform, false);
        LeanTween.scale(readyUpPanel, Vector3.zero, .1f);
    }

    public void EmbarkOnQuest() { }
    #endregion

    public IEnumerator WaitBeforeFlipDescAgain() 
    {
        yield return new WaitForSeconds(.5f);
        descriptionFlipper.readyToFlip = true;
    }
    public IEnumerator WaitBeforeFlipDetailsAgain()
    {
        yield return new WaitForSeconds(.5f);
        detailsFlipper.readyToFlip = true;
    }
}
