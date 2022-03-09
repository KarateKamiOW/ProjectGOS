using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PostMatchScreen : MonoBehaviour
{
    [SerializeField] GameObject statsBoxGO;
    [SerializeField] GameObject itemsDropBoxGO;
    [SerializeField] Transform statsContainer;
    [SerializeField] Transform dropsAndRewardsContainer;
    [SerializeField] QuestLogObject playerQuestLogObj;
    [SerializeField] InventoryObject playerInventoryObj;
    [SerializeField] Image continueButtonIMG;
    [SerializeField] List<Sprite> continueButtonHints;
    [SerializeField] BattleSystemDatabases databases;
    public BattleUnit p1Unit;
    public BattleUnit p2Unit;

    ItemsDatabase ItemsDB;
    QuestsDatabase QuestsDB;


    InventoryObject excessDropsList;
    public bool PlayerMayProceed { get; set; }
    bool ReadyToRevealHint = false;

    // Start is called before the first frame update
    void Start()
    {
        ItemsDB = Resources.Load<ItemsDatabase>("ItemsDatabase");
        PlayerMayProceed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMayProceed) 
        {
            if (ReadyToRevealHint) 
            {
                StartCoroutine(ButtonPressingAnim());
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter)) 
            {
                string sceneName = PlayerPrefs.GetString("CurrentLocationMap");
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    public void GeneratePostMatchStats()
    {
        //StatsDoneGenerating = false;
        StatTotalSuccessfullyCastedRohkan(SetBasicStatData());
        StatTotalRohkanSW(SetBasicStatData());
        StatTotalRohkanBB(SetBasicStatData());

        StatTotalSuccessfullyCastedPaperious(SetBasicStatData());
        StatTotalPaperiousSW(SetBasicStatData());
        StatTotalPaperiousBB(SetBasicStatData());

        StatTotalSuccessfullyCastedScissora(SetBasicStatData());
        StatTotalScissoraSW(SetBasicStatData());
        StatTotalScissoraBB(SetBasicStatData());

        StatTotalClangs(SetBasicStatData());

        DetermineDroppedItems();

        StartCoroutine(WaitToProceed());

        //StatsDoneGenerating = true;

    }
    #region All Stats To Generate
    PostMatchStatsBoxUI SetBasicStatData()
    {
        GameObject obj = Instantiate(statsBoxGO);
        obj.transform.SetParent(statsContainer.gameObject.transform, false);

        var objData = obj.GetComponent<PostMatchStatsBoxUI>();

        objData.caster1Sprite.sprite = p1Unit.TheCaster.CasterBase.CasterSprite;
        if (p2Unit.SoloPlayerEnemyBase != null)
            objData.caster2Sprite.sprite = p2Unit.TheEnemyCaster.EnemyCasterBase.EnemySprite;
        else
            objData.caster2Sprite.sprite = p2Unit.TheCaster.CasterBase.CasterSprite;

        return objData;
    }
    void StatTotalSuccessfullyCastedRohkan(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Rohkan Spells Casted";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells.ToString();

    }
    void StatTotalSuccessfullyCastedPaperious(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Paperious Spells Casted";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalSuccessfullyCastedPaperiousSpells.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalSuccessfullyCastedPaperiousSpells.ToString();
    }

    void StatTotalSuccessfullyCastedScissora(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Scissora Spells Casted";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells.ToString();
    }

    void StatTotalRohkanSW(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Rohkan SW";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalRohkanSW.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalRohkanSW.ToString();
    }
    void StatTotalRohkanBB(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Rohkan BB";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalRohkanBB.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalRohkanBB.ToString();
    }

    void StatTotalPaperiousSW(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Paperious SW";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalPaperiousSW.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalPaperiousSW.ToString();
    }
    void StatTotalPaperiousBB(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Paperious BB";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalPaperiousBB.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalPaperiousBB.ToString();
    }

    void StatTotalScissoraSW(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Scissora SW";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalScissoraSW.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalScissoraSW.ToString();
    }
    void StatTotalScissoraBB(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Scissora BB";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalScissoraBB.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalScissoraBB.ToString();
    }
    void StatTotalClangs(PostMatchStatsBoxUI objData)
    {
        objData.statTitle.text = "Total Clangs";
        objData.caster1StatNum.text = p1Unit.BonusPlayerStats.TotalClangs.ToString();
        objData.caster2StatNum.text = p2Unit.BonusPlayerStats.TotalClangs.ToString();
    }

    #endregion
    #region All Dropped Items To Generate

    public void DetermineDroppedItems()
    {
        playerInventoryObj.Load();
        if (p2Unit.SoloPlayerEnemyBase != null)
        {
            for (int i = 0; i < p2Unit.SoloPlayerEnemyBase.EnemyDrops.Count; i++)
            {
                var dropNum = Random.Range(0, 100f);
                if (dropNum <= p2Unit.SoloPlayerEnemyBase.EnemyDrops[i].dropPercentage)
                {
                    int itemDropNum = p2Unit.SoloPlayerEnemyBase.EnemyDrops[i].itemDrop.IDNUM;
                    ItemDataObject droppedItem = p2Unit.SoloPlayerEnemyBase.EnemyDrops[i].itemDrop; //databases.RetreiveItemData(itemDropNum).Base;
                    playerInventoryObj.AddItem(droppedItem, 1);
                    playerInventoryObj.Save();

                    GameObject obj = Instantiate(itemsDropBoxGO);
                    obj.transform.SetParent(dropsAndRewardsContainer.gameObject.transform, false);

                    obj.GetComponent<ObtainedItemSprite>().ItemSprite.sprite = droppedItem.itemSprite;
                    var descriptionPanel = obj.GetComponentInChildren<InventoryItemDescrPanelUI>();

                    descriptionPanel.itemNameText.text = droppedItem.ItemName;
                    descriptionPanel.itemDescription.text = droppedItem.description;
                    descriptionPanel.itemSprite.sprite = droppedItem.itemSprite;
                    descriptionPanel.carryCapacityText.text = droppedItem.MaxCarryPerSlot.ToString();
                    descriptionPanel.sellPriceText.text = droppedItem.SellPrice.ToString();

                    //Save to PlayerPrefs
                }

            }
        }

        //DetermineSolcReward();
    }

    void DetermineSolcReward()
    {
        playerInventoryObj.Load();

        if (p2Unit.SoloPlayerEnemyBase != null) 
        {
            //int minReward = p2Unit.SoloPlayerEnemyBase.;
            int solcReward = Random.Range(p2Unit.SoloPlayerEnemyBase.MinSolcs, p2Unit.SoloPlayerEnemyBase.MaxSolcs);

            

            playerInventoryObj.playerTotalSolcs += solcReward;
            playerInventoryObj.Save();
            /*if (PlayerPrefs.HasKey("PlayerSolcs"))
                currentPlayerSolcsAmt = PlayerPrefs.GetInt("PlayerSolcs");

            currentPlayerSolcsAmt += solcReward;

            PlayerPrefs.SetInt("PlayerSolcs", currentPlayerSolcsAmt);*/


            //Gonna Instantiate an Itembox to show amt specifics
        }
    }

    void DetermineQuestProgress() 
    { 
        playerQuestLogObj.Load();

        if (p2Unit.SoloPlayerEnemyBase != null)
        {
            for (int i = 0; i < playerQuestLogObj.MainQuestsContainer.Count; i++)
            {
                playerQuestLogObj.MainQuestsContainer[i].quest.questDetails.CheckSlayQuestProgress(p2Unit.SoloPlayerEnemyBase);   
            }

            playerQuestLogObj.Save();
        }
    }

    IEnumerator WaitToProceed() 
    {
        yield return new WaitForSeconds(6f);
        PlayerMayProceed = true;
        ReadyToRevealHint = true;
        continueButtonIMG.gameObject.SetActive(true);
    }
    IEnumerator ButtonPressingAnim()
    {
        ReadyToRevealHint = false;
        continueButtonIMG.sprite = continueButtonHints[0];
        yield return new WaitForSeconds(.6f);
        continueButtonIMG.sprite = continueButtonHints[1]; 
        yield return new WaitForSeconds(.6f);
        ReadyToRevealHint = true;
    }

    void PullPlayerInventoryData() 
    {
        int invCount;
        if (PlayerPrefs.HasKey("CurrentInvTotal"))
            invCount = PlayerPrefs.GetInt("CurrentInvTotal");
        else 
        {
            PlayerPrefs.SetInt("CurrentInvTotal", 0);
            invCount = PlayerPrefs.GetInt("CurrentInvTotal");
        }
        for (int i = 0; i < invCount; i++)
        {
            int itemNum;
            int amtNum;
            if (PlayerPrefs.HasKey("PlayerInvItem" + i.ToString())) 
            {
                itemNum = PlayerPrefs.GetInt("PlayerInvItem" + i.ToString());
                amtNum = PlayerPrefs.GetInt("PlayerInvAmt" + i.ToString());

                //tempPlayerInventory.AddItem(databases.RetreiveItemData(itemNum).Base, amtNum);
            }


            PlayerPrefs.SetInt("CurrentInvTotal", i + 1);

        }//tempPlayerInventory
    }
    #endregion

}
