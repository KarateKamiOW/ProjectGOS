using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SpellLogState {Base, RohkanSpells, PaperiousSpells, ScissoraSpells, Relics, SavedLoadOuts  }
public class SpellLogUI : MonoBehaviour
{
    [Header("GameObjects & Databases")]
    [SerializeField] InventoryObject playerInventory;
    [SerializeField] BattleSystemDatabases sysDatabases;
    [SerializeField] GameObject spellPanel;
    [SerializeField] Transform containerPos;
    [SerializeField] GameObject SpellLogGO;

    [Header("Panels & Buttons")]
    [SerializeField] CurrentSelectedSpellPanels currentEquippedSpellPanels;
    [SerializeField] SpellLogButtonTabs spellLogButtonTabs;

    [Header("Currently Hovered Spell Info")]
    [SerializeField] Image selectedSpellIMG;
    [SerializeField] TextMeshProUGUI selectedSpellName;
    [SerializeField] TextMeshProUGUI selectedSpellBasicDescription;
    [SerializeField] TextMeshProUGUI selectedSpellEmpoweredDescription;

    List<Spells> allRohkanSpells = new List<Spells>();
    List<Spells> allPaperiousSpells = new List<Spells>();
    List<Spells> allScissoraSpells = new List<Spells>();

    List<SpellsScriptableObject> AllRohkanSpells = new List<SpellsScriptableObject>();
    List<SpellsScriptableObject> AllPaperiousSpells = new List<SpellsScriptableObject>();
    List<SpellsScriptableObject> AllScissoraSpells = new List<SpellsScriptableObject>();

    List<GameObject> tempSpellsList = new List<GameObject>();

    SpellLogState spellLogState;
    PlayerCaster playerData;
    SpellsDatabase spellsDatabase;

    private void Awake()
    {
        //spellsDatabase = Resources.Load<SpellsDatabase>("SpellsDatabase");
    }
    
    void Start()
    {
        spellsDatabase = Resources.Load<SpellsDatabase>("SpellsDatabase");

        //This Unlocks All Player Spells. DELETE BEFORE ANY MAJOR RELEASE
        if (playerInventory.UnlockedSpells.Count > 0)
        {
            Debug.Log("ALL SPELLS UNLOCKED. DELETE BEFORE MAJOR RELEASE");
            playerInventory.UnlockedSpells.Clear();
            for (int i = 0; i < spellsDatabase.Spells.Length; i++)
            {
                playerInventory.UnlockedSpells.Add(spellsDatabase.Spells[i]);
            }
        }
        playerData = PlayerController.instance.GetComponent<PlayerCaster>();
        spellLogState = SpellLogState.Base;

        OrganizeSpells();
    }


    void OrganizeSpells() 
    {
        for (int i = 0; i < playerInventory.UnlockedSpells.Count; i++)
        {
            if (playerInventory.UnlockedSpells[i].GodType == SpellGodType.Rohkan)
            {
                AllRohkanSpells.Add(playerInventory.UnlockedSpells[i]);
            }

            if (playerInventory.UnlockedSpells[i].GodType == SpellGodType.Paperious)
            {
                AllPaperiousSpells.Add(playerInventory.UnlockedSpells[i]);
            }

            if (playerInventory.UnlockedSpells[i].GodType == SpellGodType.Scissora)
            {
                AllScissoraSpells.Add(playerInventory.UnlockedSpells[i]);
            }
        }
        
    }

    public void UpdateCurrentSelectedSpell(int selectedSpellNum) 
    {
        if (spellLogState == SpellLogState.RohkanSpells)
        {
            selectedSpellName.text = AllRohkanSpells[selectedSpellNum].SpellName;
            selectedSpellIMG.sprite = AllRohkanSpells[selectedSpellNum].SpellSprite;
            selectedSpellBasicDescription.text = AllRohkanSpells[selectedSpellNum].SpellDescription;
            selectedSpellEmpoweredDescription.text = AllRohkanSpells[selectedSpellNum].SpellBonusDescription;
        }

        if (spellLogState == SpellLogState.PaperiousSpells)
        {
            selectedSpellName.text = AllPaperiousSpells[selectedSpellNum].SpellName;
            selectedSpellIMG.sprite = AllPaperiousSpells[selectedSpellNum].SpellSprite;
            selectedSpellBasicDescription.text = AllPaperiousSpells[selectedSpellNum].SpellDescription;
            selectedSpellEmpoweredDescription.text = AllPaperiousSpells[selectedSpellNum].SpellBonusDescription;
        }

        if (spellLogState == SpellLogState.ScissoraSpells)
        {
            selectedSpellName.text = AllScissoraSpells[selectedSpellNum].SpellName;
            selectedSpellIMG.sprite = AllScissoraSpells[selectedSpellNum].SpellSprite;
            selectedSpellBasicDescription.text = AllScissoraSpells[selectedSpellNum].SpellDescription;
            selectedSpellEmpoweredDescription.text = AllScissoraSpells[selectedSpellNum].SpellBonusDescription;
        }
        
    }


    #region GeneratingSpellLists
    public void GenerateRohkanList() 
    {

        for (int i = 0; i < AllRohkanSpells.Count; i++)
        {
            GameObject obj = Instantiate(spellPanel);
            obj.transform.SetParent(containerPos.gameObject.transform, false);
            var objInfo = obj.GetComponent<SpellPanelUI>();

            objInfo.spellBookIMG.sprite = AllRohkanSpells[i].SpellSprite;
            objInfo.tempSpellBookIMG.sprite = AllRohkanSpells[i].SpellSprite;
            objInfo.GivenListPos = i;
            //objInfo.PanelGodType = SpellGodType.Rohkan;


            tempSpellsList.Add(obj);
        }

        

    }

    public void GeneratePaperiousList()
    {

        for (int i = 0; i < AllPaperiousSpells.Count; i++)
        {
            GameObject obj = Instantiate(spellPanel);
            obj.transform.SetParent(containerPos.gameObject.transform, false);
            var objInfo = obj.GetComponent<SpellPanelUI>();

            objInfo.spellBookIMG.sprite = AllPaperiousSpells[i].SpellSprite;
            objInfo.tempSpellBookIMG.sprite = AllPaperiousSpells[i].SpellSprite;
            objInfo.GivenListPos = i;


            tempSpellsList.Add(obj);
        }

    }

    public void GenerateScissoraList()
    {

        for (int i = 0; i < AllScissoraSpells.Count; i++)
        {
            GameObject obj = Instantiate(spellPanel);
            obj.transform.SetParent(containerPos.gameObject.transform, false);
            var objInfo = obj.GetComponent<SpellPanelUI>();

            objInfo.spellBookIMG.sprite = AllScissoraSpells[i].SpellSprite;
            objInfo.tempSpellBookIMG.sprite = AllScissoraSpells[i].SpellSprite;
            objInfo.GivenListPos = i;


            tempSpellsList.Add(obj);
        }

    }
    #endregion

    public void DestroyPanels() 
    {
        for (int i = 0; i < tempSpellsList.Count; i++) 
        {
            Destroy(tempSpellsList[i]);
        }
    }

    public void OpenUI() 
    {
        LeanTween.scale(SpellLogGO, Vector3.one, .5f);
    }

    public void CloseUI() 
    {
        LeanTween.scale(SpellLogGO, Vector3.zero, .2f);
        spellLogState = SpellLogState.Base;

        spellLogButtonTabs.rhokanSelectedTabParticle.SetActive(false);
        spellLogButtonTabs.paperiousSelectedTabParticle.SetActive(false);
        spellLogButtonTabs.scissoraSelectedTabParticle.SetActive(false);

        DestroyPanels();
    }
    public void OpenRhokanList() 
    {
        if (spellLogState != SpellLogState.RohkanSpells) 
        {
            DestroyPanels();
            spellLogState = SpellLogState.RohkanSpells;

            spellLogButtonTabs.rhokanSelectedTabParticle.SetActive(true);
            spellLogButtonTabs.paperiousSelectedTabParticle.SetActive(false);
            spellLogButtonTabs.scissoraSelectedTabParticle.SetActive(false);

            GenerateRohkanList();
            UpdateImageForEquippedSpellsSlot();

        }
    }

    public void OpenPaperiousList()
    {
        if (spellLogState != SpellLogState.PaperiousSpells) 
        {
            DestroyPanels();
            spellLogState = SpellLogState.PaperiousSpells;

            spellLogButtonTabs.rhokanSelectedTabParticle.SetActive(false);
            spellLogButtonTabs.paperiousSelectedTabParticle.SetActive(true);
            spellLogButtonTabs.scissoraSelectedTabParticle.SetActive(false);

            GeneratePaperiousList();
            UpdateImageForEquippedSpellsSlot();

        }
    }

    public void OpenScissoraList()
    {
        if (spellLogState != SpellLogState.ScissoraSpells) 
        {
            DestroyPanels();
            spellLogState = SpellLogState.ScissoraSpells;

            spellLogButtonTabs.rhokanSelectedTabParticle.SetActive(false);
            spellLogButtonTabs.paperiousSelectedTabParticle.SetActive(false);
            spellLogButtonTabs.scissoraSelectedTabParticle.SetActive(true);

            GenerateScissoraList();
            UpdateImageForEquippedSpellsSlot();

        }
    }

    public void SetPlayerSpellData(int spellnum, int slotPos) 
    {
        
        if (spellLogState == SpellLogState.RohkanSpells)
        {
            if (slotPos == 0)
            {
                if (playerData.RohkanSpell2.Base != AllRohkanSpells[spellnum])
                {
                    playerData.SetAndSaveRohkanSpell(AllRohkanSpells[spellnum], slotPos);
                    UpdateImageForEquippedSpellsSlot();
                }
            }
            else if (slotPos == 1) 
            {
                if (playerData.RohkanSpell1.Base != AllRohkanSpells[spellnum]) 
                {
                    playerData.SetAndSaveRohkanSpell(AllRohkanSpells[spellnum], slotPos);
                    UpdateImageForEquippedSpellsSlot();
                }
            }
        }

        else if (spellLogState == SpellLogState.PaperiousSpells)
        {
            if (slotPos == 0)
            {
                if (playerData.PaperiousSpell2.Base != AllPaperiousSpells[spellnum]) 
                {
                    playerData.SetAndSavePaperiousSpell(AllPaperiousSpells[spellnum], slotPos);
                    UpdateImageForEquippedSpellsSlot();
                }
            }
            else if (slotPos == 1)
            {
                if (playerData.PaperiousSpell1.Base != AllPaperiousSpells[spellnum]) 
                {
                    playerData.SetAndSavePaperiousSpell(AllPaperiousSpells[spellnum], slotPos);
                    UpdateImageForEquippedSpellsSlot();
                }
            }
        }
        else if (spellLogState == SpellLogState.ScissoraSpells) 
        {
            if (slotPos == 0)
            {
                if (playerData.ScissoraSpell2.Base != AllScissoraSpells[spellnum]) 
                {
                    playerData.SetAndSaveScissoraSpell(AllScissoraSpells[spellnum], slotPos);
                    UpdateImageForEquippedSpellsSlot();
                }
                
            }
            else if (slotPos == 1)
            {
                if (playerData.ScissoraSpell1.Base != AllScissoraSpells[spellnum]) 
                {
                    playerData.SetAndSaveScissoraSpell(AllScissoraSpells[spellnum], slotPos);
                    UpdateImageForEquippedSpellsSlot();
                }
            }
        }
    }

    public void UpdateImageForEquippedSpellsSlot() 
    {
        if (spellLogState == SpellLogState.RohkanSpells) 
        {
            currentEquippedSpellPanels.selectedSpellPanel0.spellBookIMG.sprite = playerData.RohkanSpell1.Base.SpellSprite;
            currentEquippedSpellPanels.selectedSpellPanel1.spellBookIMG.sprite = playerData.RohkanSpell2.Base.SpellSprite;
        }
        else if (spellLogState == SpellLogState.PaperiousSpells)
        {
            currentEquippedSpellPanels.selectedSpellPanel0.spellBookIMG.sprite = playerData.PaperiousSpell1.Base.SpellSprite;
            currentEquippedSpellPanels.selectedSpellPanel1.spellBookIMG.sprite = playerData.PaperiousSpell2.Base.SpellSprite;
        }
        else if (spellLogState == SpellLogState.ScissoraSpells)
        {
            currentEquippedSpellPanels.selectedSpellPanel0.spellBookIMG.sprite = playerData.ScissoraSpell1.Base.SpellSprite;
            currentEquippedSpellPanels.selectedSpellPanel1.spellBookIMG.sprite = playerData.ScissoraSpell2.Base.SpellSprite;
        }
    }
}

[System.Serializable]
public class CurrentSelectedSpellPanels 
{
    public SelectedSpellPanel selectedSpellPanel0;
    public SelectedSpellPanel selectedSpellPanel1;

    public Sprite selectedPanel;
    public Sprite unselectedPanel;
}

[System.Serializable]
public class SpellLogButtonTabs 
{
    public GameObject rhokanSelectedTabParticle;
    public GameObject paperiousSelectedTabParticle;
    public GameObject scissoraSelectedTabParticle;
}
