using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {Start, PlayerAction, PlayerRohkanSpellSelect, PlayerPaperiousSpellSelect, PlayerScissoraSpellSelect, EnemyMove, Busy }
public class BattleSystem : Photon.MonoBehaviour
{
    #region Variables
    [Header("PlayerUnit + HUD")]
    [SerializeField] PlayerCaster playerInfoForSpells;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;

    [Header("EnemyUnit + HUD")]
    [SerializeField] EnemyCasterSpellSet enemyInfoForSpells;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;

    [Header("Round Timer text and Dialog")]
    [SerializeField] TextMeshProUGUI countdownTimer;
    [SerializeField] BattleDialog battleDialogSystem;

    [Header("Menus")]
    public GameObject dialogBoxMenu;
    public GameObject bookSelectMenu;
    public GameObject spellSelectMenu;
    public GameObject rohkanSpellsMenu;
    public GameObject paperiousSpellsMenu;
    public GameObject scissoraSpellsMenu;
    public GameObject spellCardUI;

    [Header("Selectors")]
    public List<Image> bookSelectors;
    public List<Image> rohkanSpellSelectors;
    public List<Image> paperiousSpellSelectors;
    public List<Image> scissoraSpellSelectors;

    [Header("Spell Data")]
    public List<SpellInfoAndData> rohkanSpellData;
    public List<SpellInfoAndData> paperiousSpellData;
    public List<SpellInfoAndData> scissoraSpellData;
    public List<AdditionalSpellDataBank> additionalSpellDatas;
    public SpellCardUIInfo castedSpellCard;

    [Header("Icons")]
    public List<Sprite> spellTypeIcons;
    public TextMeshProUGUI roundTimerText;
    public TextMeshProUGUI currentRoundText;

    [Header("Photon")]
    public PhotonView playerPhotonView;

    BattleState state;

    int currentBookSelectAction;
    int currentSpellSelectAction;
    int orderNum;
    bool p1Priority = true;
    float roundTimerNum;
    int currentRoundNum;

    #endregion

    #region OnAndOffMenus
    public void OnOffDialogBox(bool onOrOff)
    {
        dialogBoxMenu.SetActive(onOrOff);
    }

    public void OnOffbookSelect(bool onOrOff)
    {
        bookSelectMenu.SetActive(onOrOff);
    }

    public void OnOffSpellSelect(bool onOrOff)
    {
        spellSelectMenu.SetActive(onOrOff);
    }

    public void OnOffRohkanSpellsSelectMenu(bool onOrOff)
    {
        rohkanSpellsMenu.SetActive(onOrOff);
    }
    public void OnOffPaperiousSpellsSelectMenu(bool onOrOff)
    {
        paperiousSpellsMenu.SetActive(onOrOff);
    }
    public void OnOffScissoraSpellsSelectMenu(bool onOrOff)
    {
        scissoraSpellsMenu.SetActive(onOrOff);
    }

    public void OnOffSpellCard(bool onOrOff) 
    {
        spellCardUI.SetActive(onOrOff);
    }
    #endregion

    void Start()
    {
        photonView.RPC("SetupBattle", PhotonTargets.All);
        //StartCoroutine(SetupBattle());
    }

    public void Update()
    {
        //This will be HandleUpdate Later

        if (photonView.isMine)
        {

            if (state == BattleState.PlayerAction)
            {
                HandleBookSelect(); 

            }
            else if (state == BattleState.PlayerRohkanSpellSelect)
            {
                HandleSpellSelect();
                
            }
            else if (state == BattleState.PlayerPaperiousSpellSelect)
            {
                HandleSpellSelect();
                
            }
            else if (state == BattleState.PlayerScissoraSpellSelect)
            {
                HandleSpellSelect();
            }
        }


    }

    [PunRPC]
    public IEnumerator SetupBattle()
    {
        playerUnit.SetUp();
        playerHUD.SetData(playerUnit.TheCaster, playerUnit.casterHealth);

        enemyUnit.SetUp();
        enemyHUD.SetData(enemyUnit.TheCaster, enemyUnit.casterHealth);

        yield return playerUnit.CheckForPassive(playerUnit, enemyUnit, playerUnit.CasterHUD.CasterAnim, RoundPositionForPassives.GameStart);
        yield return enemyUnit.CheckForPassive(enemyUnit, playerUnit, enemyUnit.CasterHUD.CasterAnim, RoundPositionForPassives.GameStart);

        //Sets the spell data
        playerInfoForSpells.SetSpellData();
        SetPlayerSpellData();

        enemyInfoForSpells.SetSpellData();

        AilmentsDB.Initialize();
        yield return battleDialogSystem.TypeDialog("CASTERS READY!");
        //Trigger Personal Caster Dialog


        currentRoundNum = 0;
        UpdateCurrentRound(currentRoundNum);
        roundTimerNum = 6;

        PlayerAction();
    }

    void SetPlayerSpellData()
    {
        //Setting Rohkan(Rock) Spell Data 
        rohkanSpellData[0].spellImage.sprite = playerInfoForSpells.RohkanSpell1Exposed.Base.SpellSprite;
        rohkanSpellData[0].spellName.text = playerInfoForSpells.RohkanSpell1Exposed.Base.SpellName;
        rohkanSpellData[0].spellDescription.text = playerInfoForSpells.RohkanSpell1Exposed.Base.SpellDescription;
        rohkanSpellData[0].spellBonusDescription.text = playerInfoForSpells.RohkanSpell1Exposed.Base.SpellBonusDescription;
        rohkanSpellData[0].spellData = playerInfoForSpells.RohkanSpell1Exposed.Base;

        rohkanSpellData[1].spellImage.sprite = playerInfoForSpells.RohkanSpell2Exposed.Base.SpellSprite;
        rohkanSpellData[1].spellName.text = playerInfoForSpells.RohkanSpell2Exposed.Base.SpellName;
        rohkanSpellData[1].spellDescription.text = playerInfoForSpells.RohkanSpell2Exposed.Base.SpellDescription;
        rohkanSpellData[1].spellBonusDescription.text = playerInfoForSpells.RohkanSpell2Exposed.Base.SpellBonusDescription;
        rohkanSpellData[1].spellData = playerInfoForSpells.RohkanSpell2Exposed.Base;


        //Setting Paperious(Paper) Spell Data 
        paperiousSpellData[0].spellImage.sprite = playerInfoForSpells.PaperiousSpell1Exposed.Base.SpellSprite;
        paperiousSpellData[0].spellName.text = playerInfoForSpells.PaperiousSpell1Exposed.Base.SpellName;
        paperiousSpellData[0].spellDescription.text = playerInfoForSpells.PaperiousSpell1Exposed.Base.SpellDescription;
        paperiousSpellData[0].spellBonusDescription.text = playerInfoForSpells.PaperiousSpell1Exposed.Base.SpellBonusDescription;
        paperiousSpellData[0].spellData = playerInfoForSpells.PaperiousSpell1Exposed.Base;

        paperiousSpellData[1].spellImage.sprite = playerInfoForSpells.PaperiousSpell2Exposed.Base.SpellSprite;
        paperiousSpellData[1].spellName.text = playerInfoForSpells.PaperiousSpell2Exposed.Base.SpellName;
        paperiousSpellData[1].spellDescription.text = playerInfoForSpells.PaperiousSpell2Exposed.Base.SpellDescription;
        paperiousSpellData[1].spellBonusDescription.text = playerInfoForSpells.PaperiousSpell2Exposed.Base.SpellBonusDescription;
        paperiousSpellData[1].spellData = playerInfoForSpells.PaperiousSpell2Exposed.Base;


        //Setting Scissora(Scissors) Spell Data 
        scissoraSpellData[0].spellImage.sprite = playerInfoForSpells.ScissoraSpell1Exposed.Base.SpellSprite;
        scissoraSpellData[0].spellName.text = playerInfoForSpells.ScissoraSpell1Exposed.Base.SpellName;
        scissoraSpellData[0].spellDescription.text = playerInfoForSpells.ScissoraSpell1Exposed.Base.SpellDescription;
        scissoraSpellData[0].spellBonusDescription.text = playerInfoForSpells.ScissoraSpell1Exposed.Base.SpellBonusDescription;
        scissoraSpellData[0].spellData = playerInfoForSpells.ScissoraSpell1Exposed.Base;

        scissoraSpellData[1].spellImage.sprite = playerInfoForSpells.ScissoraSpell2Exposed.Base.SpellSprite;
        scissoraSpellData[1].spellName.text = playerInfoForSpells.ScissoraSpell2Exposed.Base.SpellName;
        scissoraSpellData[1].spellDescription.text = playerInfoForSpells.ScissoraSpell2Exposed.Base.SpellDescription;
        scissoraSpellData[1].spellBonusDescription.text = playerInfoForSpells.ScissoraSpell2Exposed.Base.SpellBonusDescription;
        scissoraSpellData[1].spellData = playerInfoForSpells.ScissoraSpell2Exposed.Base;

        additionalSpellDatas[0].spellName = playerInfoForSpells.BlockAsSpellExposed.Base.SpellName;
        additionalSpellDatas[0].spellData = playerInfoForSpells.BlockAsSpellExposed.Base;


    }

    #region StateChangingMethods
    void PlayerAction() 
    {
        state = BattleState.PlayerAction;
        OnOffDialogBox(false);
        OnOffbookSelect(true);
        OnOffSpellSelect(false);

        OnOffRohkanSpellsSelectMenu(false);
        OnOffPaperiousSpellsSelectMenu(false);
        OnOffScissoraSpellsSelectMenu(false);
    }
    void BeginAttackPhase() 
    {
        state = BattleState.Busy;
        OnOffbookSelect(false);
        OnOffRohkanSpellsSelectMenu(false);
        OnOffPaperiousSpellsSelectMenu(false);
        OnOffScissoraSpellsSelectMenu(false);
        OnOffDialogBox(true);
        OnOffSpellSelect(false);

    }
    void PlayerRohkanSpellSelect() 
    {
        state = BattleState.PlayerRohkanSpellSelect;
        OnOffbookSelect(false);
        OnOffSpellSelect(true);
        OnOffRohkanSpellsSelectMenu(true);

    }
    void PlayerPaperiousSpellSelect()
    {
        state = BattleState.PlayerPaperiousSpellSelect;
        OnOffbookSelect(false);
        OnOffSpellSelect(true);
        OnOffPaperiousSpellsSelectMenu(true);

    }
    void PlayerScissoraSpellSelect()
    {
        state = BattleState.PlayerScissoraSpellSelect;
        OnOffbookSelect(false);
        OnOffSpellSelect(true);
        OnOffScissoraSpellsSelectMenu(true);

    }
    #endregion

    #region BookAndSpellSelectionMethods
    [PunRPC]
    void HandleBookSelect() 
    {
        Mathf.Clamp(currentBookSelectAction, 0, 3);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            if (currentBookSelectAction == 0)
                currentBookSelectAction = 3;
            else
                currentBookSelectAction--;

            if (currentBookSelectAction == 3 && playerUnit.BlockCD > 0)
                currentBookSelectAction = 2;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentBookSelectAction == 3)
                currentBookSelectAction = 0;
            else
                currentBookSelectAction++;

            if (currentBookSelectAction == 3 && playerUnit.BlockCD > 0)
                currentBookSelectAction = 0;
        }
        UpdateHandleBookSelector(currentBookSelectAction);

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (currentBookSelectAction == 0)
            {
                //Player Picks Rock!
                PlayerRohkanSpellSelect();
            }
            else if (currentBookSelectAction == 1)
            {
                //Player Pick Paper!
                PlayerPaperiousSpellSelect();
            }
            else if (currentBookSelectAction == 2)
            {
                //Player picks Scissors!
                PlayerScissoraSpellSelect();
            }
            else if(currentBookSelectAction == 3)
            {
                BeginAttackPhase();
                photonView.RPC("AttackPhase", PhotonTargets.All, additionalSpellDatas[0].spellData, enemyInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyInfoForSpells.TotalEnemyListOfSpells.Count - 1)].Base);
                //StartCoroutine(AttackPhase(additionalSpellDatas[0].spellData, enemyInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyInfoForSpells.TotalEnemyListOfSpells.Count - 1)].Base));
                //Implies player chose to Block!
                /*if (playerUnit.BlockCD <= 0)
                {
                    BeginAttackPhase();
                    StartCoroutine(AttackPhase(additionalSpellDatas[0].spellData, enemyInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyInfoForSpells.TotalEnemyListOfSpells.Count - 1)].Base));
                    Debug.Log("BLOCK! (Functionality coming soon)");
                }
                else 
                {
                    Debug.Log("Block is on CD!");
                }*/
                
            }
        }
    }

    void HandleSpellSelect() 
    {
        Mathf.Clamp(currentSpellSelectAction, 0, 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentSpellSelectAction == 0)
                currentSpellSelectAction = 1;
            else
                currentSpellSelectAction--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentSpellSelectAction == 1)
                currentSpellSelectAction = 0;
            else
                currentSpellSelectAction++;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) 
        {
            //Player wants to return
            currentSpellSelectAction = 0;
            PlayerAction();
        }


        if (state == BattleState.PlayerRohkanSpellSelect)
        {
            UpdateHandleRohkanSpellSelector(currentSpellSelectAction);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BeginAttackPhase();
                StartCoroutine(AttackPhase(rohkanSpellData[currentSpellSelectAction].spellData, BasicEnemyAI(enemyUnit)));
            }
        }
        else if (state == BattleState.PlayerPaperiousSpellSelect)
        {
            UpdateHandlePaperiousSpellSelector(currentSpellSelectAction);
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                BeginAttackPhase();
                StartCoroutine(AttackPhase(paperiousSpellData[currentSpellSelectAction].spellData, BasicEnemyAI(enemyUnit)));
            }
        }
        else if (state == BattleState.PlayerScissoraSpellSelect)
        {
            UpdateHandleScissoraSelector(currentSpellSelectAction);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BeginAttackPhase();
                StartCoroutine(AttackPhase(scissoraSpellData[currentSpellSelectAction].spellData, BasicEnemyAI(enemyUnit)));
            }
        }

    }

    void UpdateHandleBookSelector(int selectedBookAction) 
    {
        for (int i = 0; i < bookSelectors.Count; i++) 
        {
            if (i == selectedBookAction) 
            {
                bookSelectors[i].enabled = true;
            }
            else
                bookSelectors[i].enabled = false;
        }
    }

    void UpdateHandleRohkanSpellSelector(int selectedSpellAction) 
    {
        for (int i = 0; i < rohkanSpellData.Count; i++)
        {
            if (i == selectedSpellAction)
            {
                rohkanSpellSelectors[i].enabled = true;
            }
            else
                rohkanSpellSelectors[i].enabled = false; 
        }

    }
    void UpdateHandlePaperiousSpellSelector(int selectedSpellAction)
    {
        for (int i = 0; i < paperiousSpellData.Count; i++)
        {
            if (i == selectedSpellAction)
            {
                paperiousSpellSelectors[i].enabled = true;
            }
            else
                paperiousSpellSelectors[i].enabled = false;
        }
    }
    void UpdateHandleScissoraSelector(int selectedSpellAction)
    {
        for (int i = 0; i < scissoraSpellData.Count; i++)
        {
            if (i == selectedSpellAction)
            {
                scissoraSpellSelectors[i].enabled = true;
            }
            else
                scissoraSpellSelectors[i].enabled = false;
        }

    }
    #endregion

    #region AttackPhaseSpellCasting

    [PunRPC]
    public IEnumerator AttackPhase(SpellsScriptableObject playerSpell, SpellsScriptableObject enemySpell) 
    {
        //Lets Start by calling for any beginning of Round Passive Triggers
        yield return PassiveOrderLogic(playerUnit, enemyUnit, RoundPositionForPassives.BeginningOfRound);
        
        //Reveals Duel GodTypes(Rock/Paper/Scissors) Image for both player and enemy
        playerHUD.SetIconImage(true, IconToDisplay(playerSpell));
        enemyHUD.SetIconImage(true, IconToDisplay(enemySpell));

        //Shortly after a second, reveal results of duel. (I.E. Player one wins picking rock into scissors, so the icon changes green)
        yield return new WaitForSeconds(1.1f);
        playerHUD.SetIconImage(true, SWBBClangLogicForImageAndAPSetting(playerSpell, enemySpell, playerUnit, enemyUnit));
        enemyHUD.SetIconImage(true, SWBBClangLogicForImageAndAPSetting(enemySpell, playerSpell, enemyUnit, playerUnit));

        //After another short delay, both icons disapear, and the battle begins.
        yield return new WaitForSeconds(.80f);
        playerHUD.SetIconImage(false, IconToDisplay(playerSpell));
        enemyHUD.SetIconImage(false, IconToDisplay(enemySpell));




        //------------

        yield return AssignAttackOrder(playerUnit, enemyUnit, playerSpell, enemySpell, SWBBClangLogicResultsForAttackPhase(playerUnit, enemyUnit));
        
        yield return new WaitForSeconds(.75f);
        yield return EndOfRoundAllyAndAilmentDamageOrder(playerUnit, enemyUnit, orderNum);

        if (p1Priority)
            p1Priority = false;
        else
            p1Priority = true;

        UpdateCurrentRound(currentRoundNum);

        PlayerAction();
    }

    IEnumerator PerformSpell(BattleUnit sourceUnit, BattleUnit targetUnit, SpellsScriptableObject spellToPerform, bool isEmpowered) 
    {
        yield return battleDialogSystem.TypeDialog(sourceUnit.TheCaster.CasterBase.CasterName + " Casted " + spellToPerform.SpellName);
        bool hasFainted = false;

        PopulateSpellCard(spellToPerform);
        OnOffSpellCard(true);


        //GameObject spellGameObject = Instantiate(spellToPerform.SpellObject, sourceUnit.transform.position, Quaternion.identity); //as GameObject;

        GameObject spellGameObject = Instantiate(spellToPerform.SpellObject, sourceUnit.spellCastPos.position, sourceUnit.spellCastPos.rotation);




        if (isEmpowered)
            yield return spellGameObject.GetComponent<ISpellAbility>().EmpoweredAbility(sourceUnit, targetUnit);
        else
            yield return spellGameObject.GetComponent<ISpellAbility>().BasicAbility(sourceUnit, targetUnit);

        

        //yield return spellToPerform.SpellObject.GetComponent<ISpellAbility>().BasicAbility(sourceUnit, targetUnit);

        if (targetUnit.casterHealth <= 0)
            hasFainted = true;

       

        if (sourceUnit == playerUnit) 
        {
            yield return enemyHUD.UpdateHP((float)targetUnit.casterHealth / targetUnit.CasterHUD.HPBar.maxHealth);
        }
        else if(sourceUnit == enemyUnit)
            yield return playerHUD.UpdateHP((float)targetUnit.casterHealth / targetUnit.CasterHUD.HPBar.maxHealth);

        if (hasFainted)
        {
            yield return battleDialogSystem.TypeDialog("K O !");
        }

        yield return new WaitForSeconds(.4f);
        OnOffSpellCard(false);

    }

    public SpellsScriptableObject BasicEnemyAI(BattleUnit sourceUnit) 
    {
        //Good chance I'll put this in another class later. 
        //For now, here's some basic enemy AI, where the moves they'll pick are entirely random

        if (sourceUnit.BlockCD <= 0)
            return enemyInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyInfoForSpells.TotalEnemyListOfSpells.Count)].Base;
        else
            return enemyInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyInfoForSpells.TotalEnemyListOfSpells.Count - 1)].Base;

    }

    public void PopulateSpellCard(SpellsScriptableObject castedSpell) 
    {
        //castedSpellCard.cardSprite = 
        switch (castedSpell.WinType) 
        {
            case SpellWinType.None:
                castedSpellCard.cardBackSprite.sprite = castedSpellCard.spellCardBackToUse[3]; //Blank cardback for now
                break;
            case SpellWinType.SW:
                castedSpellCard.cardBackSprite.sprite = castedSpellCard.spellCardBackToUse[0];
                break;
            case SpellWinType.BB:
                castedSpellCard.cardBackSprite.sprite = castedSpellCard.spellCardBackToUse[1];
                break;
            case SpellWinType.Both:
                castedSpellCard.cardBackSprite.sprite = castedSpellCard.spellCardBackToUse[2];
                break;
        }

        switch (castedSpell.GodType) 
        {
            case SpellGodType.Rohkan:
                castedSpellCard.godTypeSprite.sprite = castedSpellCard.spellGodTypeSprites[0];
                break;
            case SpellGodType.Paperious:
                castedSpellCard.godTypeSprite.sprite = castedSpellCard.spellGodTypeSprites[1];
                break;
            case SpellGodType.Scissora:
                castedSpellCard.godTypeSprite.sprite = castedSpellCard.spellGodTypeSprites[2];
                break;
            case SpellGodType.Block:
                castedSpellCard.godTypeSprite.sprite = null;   //Keep at null for now
                break;
        }

        castedSpellCard.spellBookSprite.sprite = castedSpell.SpellSprite;
        castedSpellCard.spellName.text = castedSpell.SpellName;
        castedSpellCard.spellDescription.text = castedSpell.SpellDescription;
        castedSpellCard.spellBonusDescription.text = castedSpell.SpellBonusDescription;

    }

    #endregion

    #region IconSpriteDisplayAndDuelingLogic
    public Sprite IconToDisplay(SpellsScriptableObject spellData) 
    {
        //This function returns a sprite back(Rock-Paper-Scissor) to reveal to players before the attacks go off, who won the round!
        if (spellData.GodType == SpellGodType.Rohkan)
            return spellTypeIcons[0];
        else if (spellData.GodType == SpellGodType.Paperious)
            return spellTypeIcons[3];
        else if (spellData.GodType == SpellGodType.Scissora)
            return spellTypeIcons[6];
        else if (spellData.GodType == SpellGodType.Block)
            return spellTypeIcons[9];
        else
            return null;
    }

    public Sprite SWBBClangLogicForImageAndAPSetting(SpellsScriptableObject sourceSpell, SpellsScriptableObject enemySpell, BattleUnit sourceUnit, BattleUnit targetUnit) 
    {
        //Shewt Win/Block Break/Clang Logic For Image And Attack Phase Setting
        //This function takes both spells cast from both players, and changes bools to determine whether they won or lost the round.
        //SWBBClangLogicResultsForAttackPhase() will take these results and essentially dive further as and to what happens due to the results of both players bool changes.
        //This function also returns a sprite, which is used in addition with IconToDisplay() to reveal spelltypeicons.
        if (sourceSpell.GodType == SpellGodType.Rohkan)
        {
            //Rock Spell
            if (enemySpell.GodType == SpellGodType.Scissora)
            {
                if (sourceSpell.WinType == SpellWinType.SW || sourceSpell.WinType == SpellWinType.Both)
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.RohkanSW = true;
                    sourceUnit.ClangThisRound = false;
                    sourceUnit.WonButNOSWThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalRohkanSW++;
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells++;
                    return spellTypeIcons[1];
                }
                else
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.WonButNOSWThisRound = true;
                    sourceUnit.ClangThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells++;
                    return spellTypeIcons[0];
                }
            }
            else if (enemySpell.GodType == SpellGodType.Paperious)
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.ClangThisRound = false;
                
                return spellTypeIcons[2];
            }
            else if (enemySpell.GodType == SpellGodType.Rohkan) 
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.ClangThisRound = true;

                sourceUnit.SetAilmentStatus(AilmentBuffID.clang, 1);
                sourceUnit.BonusPlayerStats.TotalClangs++;

                return spellTypeIcons[12];
            }
            else if(enemySpell.GodType == SpellGodType.Block)
            {
                if (sourceSpell.WinType == SpellWinType.BB || sourceSpell.WinType == SpellWinType.Both)
                {
                    sourceUnit.RohkanBB = true;
                    sourceUnit.BonusPlayerStats.TotalRohkanBB++;
                    return spellTypeIcons[1];
                }
                else
                {
                    //Here means the enemy successfully blocked you!
                    return spellTypeIcons[2];
                }
            }
            else
                return null;
        }
        else if (sourceSpell.GodType == SpellGodType.Paperious)
        {
            //Paper

            if (enemySpell.GodType == SpellGodType.Rohkan)
            {
                if (sourceSpell.WinType == SpellWinType.SW || sourceSpell.WinType == SpellWinType.Both)
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.PaperiousSW = true;
                    sourceUnit.ClangThisRound = false;
                    sourceUnit.WonButNOSWThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalPaperiousSW++;
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedPaperiousSpells++;
                    return spellTypeIcons[4];
                }
                else
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.WonButNOSWThisRound = true;
                    sourceUnit.ClangThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedPaperiousSpells++;
                    return spellTypeIcons[3];
                }
            }
            else if (enemySpell.GodType == SpellGodType.Scissora)
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.ClangThisRound = false;   
                
                return spellTypeIcons[5];
            }
            else if (enemySpell.GodType == SpellGodType.Paperious) 
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.ClangThisRound = true;

                sourceUnit.SetAilmentStatus(AilmentBuffID.clang, 1);

                sourceUnit.BonusPlayerStats.TotalClangs++;

                return spellTypeIcons[12];
            }
            else if (enemySpell.GodType == SpellGodType.Block)
            {
                if (sourceSpell.WinType == SpellWinType.BB || sourceSpell.WinType == SpellWinType.Both)
                {
                    sourceUnit.PaperiousBB = true;
                    sourceUnit.BonusPlayerStats.TotalPaperiousBB++;
                    return spellTypeIcons[4];
                }
                else
                {
                    //Here means the enemy successfully blocked you!
                    return spellTypeIcons[5];
                }
            }
            else
                return null;

        }
        else if (sourceSpell.GodType == SpellGodType.Scissora)
        {
            //Scissors

            if (enemySpell.GodType == SpellGodType.Paperious)
            {
                if (sourceSpell.WinType == SpellWinType.SW || sourceSpell.WinType == SpellWinType.Both)
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.ScissoraSW = true;
                    sourceUnit.ClangThisRound = false;
                    sourceUnit.WonButNOSWThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalScissoraSW++;
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells++;
                    return spellTypeIcons[7];
                }
                else
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.WonButNOSWThisRound = true;
                    sourceUnit.ClangThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells++;
                    return spellTypeIcons[6];
                }
            }
            else if (enemySpell.GodType == SpellGodType.Rohkan)
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.SWThisRound = false;  
                
                return spellTypeIcons[8];
            }
            else if (enemySpell.GodType == SpellGodType.Scissora) 
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.ClangThisRound = true;

                sourceUnit.SetAilmentStatus(AilmentBuffID.clang, 1);

                sourceUnit.BonusPlayerStats.TotalClangs++;

                return spellTypeIcons[12];
            }
            else if (enemySpell.GodType == SpellGodType.Block)
            {
                if (sourceSpell.WinType == SpellWinType.BB || sourceSpell.WinType == SpellWinType.Both)
                {
                    sourceUnit.ScissoraBB = true;
                    sourceUnit.BonusPlayerStats.TotalScissoraBB++;
                    return spellTypeIcons[7];
                }
                else
                {
                    //Here means the enemy successfully blocked you!
                    return spellTypeIcons[8];
                }
            }
            else
                return null;

        }
        else if (sourceSpell.GodType == SpellGodType.Block) 
        {
            sourceUnit.BlockCD = 2;
            sourceUnit.SetBlockIconAccordingToCD(sourceUnit.BlockCD);
            //Set Block WinType To 'None' in Editor
            if (enemySpell.WinType == SpellWinType.BB || enemySpell.WinType == SpellWinType.Both)
            {
                //Enemy Successfully Block Broke You!

                sourceUnit.SWThisRound = false;
                sourceUnit.PlayerAttemptedBlockThisRound = true;
                sourceUnit.PlayerSuccessfullyBlockedThisRound = false;

                return spellTypeIcons[11];
            }
            else 
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.PlayerAttemptedBlockThisRound = true;
                sourceUnit.PlayerSuccessfullyBlockedThisRound = true;
                return spellTypeIcons[10];
            }
        }
        else
            return null;
    }

    public int SWBBClangLogicResultsForAttackPhase(BattleUnit player1, BattleUnit player2) 
    {
        //This function takes the results from the 'SWBBClangLogicForImage' function to determine what order and type of attack the attackphase will go.
        //Returning 0 is essentially an error. An unaccounted for result. Review code and specify/fix!
        if (player1.SWThisRound && !player2.PlayerAttemptedBlockThisRound)
        {
            if (player1.WonButNOSWThisRound)
                return 3;   //P1 Won, however they did not use a move with a SW. No bonus
            else
                return 1;   //P1 Won the round. Both players attack, however, P1 gains a bonus.
        }
        else if (!player1.SWThisRound && !player2.PlayerAttemptedBlockThisRound && !player1.PlayerAttemptedBlockThisRound)
        {
            if (player2.SWThisRound) 
            {
                if (player2.WonButNOSWThisRound)
                    return 4;  //P2 won, however they did not use a move with a SW. No bonus.
                else
                    return 2;   //P2 Won the round. Both players attack, however, P2 gains a bonus.
            }
            if (player1.ClangThisRound || player2.ClangThisRound)
                return 5;   //Claaaaang 
            else
                return 0;   
        }
        else if (player1.PlayerAttemptedBlockThisRound || player2.PlayerAttemptedBlockThisRound)
        {
            if (player1.PlayerSuccessfullyBlockedThisRound)
            {
                if (player2.PlayerSuccessfullyBlockedThisRound)
                    return 7;   //Both players successfully blocked! Immune + heal 40 for both.
                else
                    return 6;   //P1 successfully blocked! P1 Immune to all dmg this round!
            }
            else if (player2.PlayerSuccessfullyBlockedThisRound)
            {
                return 8;   //P2 successfully blocked! P2 Immune to all dmg this round!
            }
            else if (player2.RohkanBB || player2.PaperiousBB || player2.ScissoraBB)
                return 9; //P1 Unsuccessfully blocked. They will take empowered damage from P2!
            else if (player1.RohkanBB || player1.PaperiousBB || player1.ScissoraBB)
                return 10; //P2 Unsuccessfully blocked. They will take empowered damage from P1!
            else
                return 0;  
        }
        else
            return 0;
    }

    public IEnumerator AssignAttackOrder(BattleUnit P1, BattleUnit P2, SpellsScriptableObject p1Spell, SpellsScriptableObject p2Spell, int orderNumber) 
    {
        Debug.Log("Ordernum = " + orderNumber);
       
        switch (orderNumber) 
        {
            case 0:
                Debug.LogWarning("Unaccounted For Outcome!");
                yield return new WaitForSeconds(2f);
                break;
            case 1: //P1 Won the Round
                orderNum = 1;
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, true);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return EndRoundPassiveOrderAlreadyDetermined(P1, P2);
                break;
            case 2: //P2 Won the Round
                orderNum = 2;
                yield return P1.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, true);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return EndRoundPassiveOrderAlreadyDetermined(P2, P1);
                break;
            case 3: //P1 Win, No Bonus
                orderNum = 3;
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return EndRoundPassiveOrderAlreadyDetermined(P1, P2);
                break;
            case 4: //P2 Win, No Bonus
                orderNum = 4;
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return EndRoundPassiveOrderAlreadyDetermined(P2, P1);
                break;
            case 5: //CLANG
                orderNum = 5;
                yield return battleDialogSystem.TypeDialog("CLAAANG!!");
                yield return new WaitForSeconds(.75f);
                yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.BeforeAttack);
                yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.AfterPlayerAttacks);
                yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.EndOfRound);
                break;
            case 6: //P1 Successfully Blocked
                orderNum = 6;
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);  //Will Not Damage
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return EndRoundPassiveOrderAlreadyDetermined(P1, P2);
                break;
            case 7: //Both Players Blocked
                orderNum = 7;
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);    //May need to be changed later
                yield return PerformSpell(P1, P2, p1Spell, true);
                //Order between these two shouldn't matter here
                //But if they ever do, this may need to be tweaked by PssiveOrderLogic()

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, true);   //Both players blocked
                yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.AfterPlayerAttacks);
                yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.EndOfRound);
                break;
            case 8: //P2 successfully Blocked
                orderNum = 8;
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);  //Will Not Damage
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return EndRoundPassiveOrderAlreadyDetermined(P2, P1);
                break;
            case 9: //P2 Block Breaks P1
                orderNum = 9;
                yield return battleDialogSystem.TypeDialog("Block Break!!");

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, true);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);
                //yield return CheckForContinuation(P1);

                yield return EndRoundPassiveOrderAlreadyDetermined(P2, P1);
                break;
            case 10: //P1 Block Breaks P2
                orderNum = 10;
                yield return battleDialogSystem.TypeDialog("Block Break!!");

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, true);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return EndRoundPassiveOrderAlreadyDetermined(P1, P2);
                break;


        }
        if (orderNumber == 0)
        {
            //Error - Returning 0 is an unaccounted for outcome
            Debug.LogWarning("Unaccounted For Outcome!");
            yield return new WaitForSeconds(2f);
        }
        else if (orderNumber == 1) 
        { }
    }

    public IEnumerator EndOfRoundResetting(BattleUnit sourceUnit) 
    {
        yield return sourceUnit.RoundEndAilments();
        yield return sourceUnit.NaturalRust();
        //StartCoroutine(ShowStatusChanges(sourceUnit));
        
        
        if (!sourceUnit.PlayerAttemptedBlockThisRound) 
        {
            sourceUnit.BlockCD--;
            if (sourceUnit.BlockCD <= 0)
                sourceUnit.BlockCD = 0;
            sourceUnit.SetBlockIconAccordingToCD(sourceUnit.BlockCD);
        }
        sourceUnit.SWThisRound = false;
        sourceUnit.WonButNOSWThisRound = false;
        sourceUnit.ClangThisRound = false;
        sourceUnit.PlayerAttemptedBlockThisRound = false;
        sourceUnit.PlayerSuccessfullyBlockedThisRound = false;
        sourceUnit.RohkanSW = false;
        sourceUnit.RohkanBB = false;
        sourceUnit.PaperiousSW = false;
        sourceUnit.PaperiousBB = false;
        sourceUnit.ScissoraSW = false;
        sourceUnit.ScissoraBB = false;
        sourceUnit.TotalBonusDmgThisRound = 0;
        orderNum = 0;

        if (currentBookSelectAction == 3)
            currentBookSelectAction = 0;
    }

    public IEnumerator EndOfRoundAllyAndAilmentDamageOrder(BattleUnit P1, BattleUnit P2, int orderNum) 
    {
        if (orderNum == 1 || orderNum == 3)
        {
            yield return P1.AllyHUD.RunMove(P1, P2);
            yield return P2.AllyHUD.RunMove(P2, P1);
            yield return new WaitForSeconds(.75f);
            yield return EndOfRoundResetting(P2);
            yield return EndOfRoundResetting(P1);
        }
        else if (orderNum == 2 || orderNum == 4)
        {
            yield return P2.AllyHUD.RunMove(P2, P1);
            yield return P1.AllyHUD.RunMove(P1, P2);
            yield return new WaitForSeconds(.75f);
            yield return EndOfRoundResetting(P1);
            yield return EndOfRoundResetting(P2);
        }
        else if (orderNum == 6)
        {
            yield return P1.AllyHUD.RunMove(P1, P2);
            yield return new WaitForSeconds(.75f);
            yield return EndOfRoundResetting(P2);
            yield return EndOfRoundResetting(P1);
        }
        else if (orderNum == 8)
        {
            yield return P2.AllyHUD.RunMove(P2, P1);
            yield return new WaitForSeconds(.75f);
            yield return EndOfRoundResetting(P1);
            yield return EndOfRoundResetting(P2);
        }
        else if (orderNum == 5 || orderNum == 7)
        {
            if (p1Priority)
            {
                yield return new WaitForSeconds(.75f);
                yield return EndOfRoundResetting(P1);
                yield return EndOfRoundResetting(P2);
            }
            else
            {
                yield return new WaitForSeconds(.75f);
                yield return EndOfRoundResetting(P2);
                yield return EndOfRoundResetting(P1);
            }
        }
        else if (orderNum == 9)
        {
            yield return P2.AllyHUD.RunMove(P2, P1);
            yield return new WaitForSeconds(.75f);
            yield return EndOfRoundResetting(P1);
            yield return EndOfRoundResetting(P2);
        }
        else if (orderNum == 10) 
        {
            yield return P1.AllyHUD.RunMove(P1, P2);
            yield return new WaitForSeconds(.75f);
            yield return EndOfRoundResetting(P2);
            yield return EndOfRoundResetting(P1);
        }
        else
        {
            Debug.Log("Err0r orderNum = " + orderNum);
        }
    }


    #endregion
    #region Passives
    IEnumerator PassiveOrderLogic(BattleUnit p1, BattleUnit p2, RoundPositionForPassives passivePos) 
    {
        //Occassionally, Passive order will be dictated by the games assigned order; going from p1 -> p2 the following round
        //Alternates between rounds starting with p1.
        //For the future another considertion can be to give it to the player who won last round
        if (passivePos == RoundPositionForPassives.BeginningOfRound)
        {
            if (p1Priority)
            {
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.BeginningOfRound);
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.BeginningOfRound);
            }
            else
            {
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.BeginningOfRound);
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.BeginningOfRound);
            }
        }
        else if (passivePos == RoundPositionForPassives.BeforeAttack) 
        {
            if (p1Priority)
            {
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
            }
            else
            {
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
            }
        }
        else if (passivePos == RoundPositionForPassives.AfterPlayerAttacks)
        {
            if (p1Priority)
            {
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);
            }
            else
            {
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);
            }

        }
        else if (passivePos == RoundPositionForPassives.EndOfRound)
        {
            if (p1Priority)
            {
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.EndOfRound);
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.EndOfRound);
            }
            else
            {
                yield return p2.CheckForPassive(p2, p1, p2.CasterHUD.CasterAnim, RoundPositionForPassives.EndOfRound);
                yield return p1.CheckForPassive(p1, p2, p1.CasterHUD.CasterAnim, RoundPositionForPassives.EndOfRound);
            }
        }
        
    }

    IEnumerator EndRoundPassiveOrderAlreadyDetermined(BattleUnit unit1, BattleUnit unit2) 
    {
        //Use this function to assign passive order when it is already determined.
        //Input the unit who's passive activates first for unit1 
        //Debug.Log("Checking Round End Passives");
        yield return unit1.CheckForPassive(unit1, unit2, unit1.CasterHUD.CasterAnim, RoundPositionForPassives.EndOfRound);
        //yield return CheckForContinuation(unit1);
        yield return unit2.CheckForPassive(unit2, unit1, unit2.CasterHUD.CasterAnim, RoundPositionForPassives.EndOfRound);
        //yield return CheckForContinuation(unit2);
    }
    #endregion

    #region RoundTimerAndCurrentRound
    public void UpdateCurrentRound(int currentRound)
    {
        currentRoundNum++;
        currentRoundText.text = currentRoundNum.ToString();

        if (currentRoundNum > 1) 
        {
            roundTimerNum = 6;
            roundTimerText.text = Mathf.FloorToInt(roundTimerNum % 60).ToString();
                //roundTimerNum.ToString();
        }
        
    }
    public void CheckingTimer() 
    {
        if (state == BattleState.PlayerAction || state == BattleState.PlayerRohkanSpellSelect || state == BattleState.PlayerPaperiousSpellSelect || state == BattleState.PlayerScissoraSpellSelect)
        {

            roundTimerNum -= Time.deltaTime;
            roundTimerText.text = Mathf.FloorToInt(roundTimerNum % 60).ToString();
            if (roundTimerNum <= 0)
            {
                roundTimerNum = 0;
                //Debug.Log("Ran Out of Time!!"); 
                //Skipped Turn
            }
        }
    }

    #endregion
}

[System.Serializable]
public class SpellInfoAndData 
{
    public Image spellImage;
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI spellDescription;
    public TextMeshProUGUI spellBonusDescription;
    public SpellsScriptableObject spellData { get; set; }

}

[System.Serializable]
public class AdditionalSpellDataBank 
{
    public string spellName;
    public SpellsScriptableObject spellData;
}

[System.Serializable]
public class SpellCardUIInfo 
{
    public Image cardBackSprite;
    public SpriteRenderer spellBookSprite;
    public SpriteRenderer godTypeSprite;
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI spellDescription;
    public TextMeshProUGUI spellBonusDescription;

    public List<Sprite> spellCardBackToUse;
    public List<Sprite> spellGodTypeSprites;

}

