using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {Start, PlayerAction, PlayerRohkanSpellSelect, PlayerPaperiousSpellSelect, PlayerScissoraSpellSelect, Waiting, Busy, PostGame }
public class BattleSystem : Photon.MonoBehaviour, IPunObservable
{
    #region Variables
    [Header("P1 + HUD")]
    [SerializeField] PlayerCaster playerInfoForSpells;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;

    [Header("P2 + HUD")]
    [SerializeField] PlayerCaster player2InfoForSpells;
    [SerializeField] EnemyCasterSpellSet enemyAIInfoForSpells;  //For Solo(AI) Play
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;

    [Header("Round Timer text and Dialog")]
    public TextMeshProUGUI roundTimerText;
    public TextMeshProUGUI currentRoundText;
    [SerializeField] BattleDialog battleDialogSystem;

    [Header("Menus")]
    public BattleSystemMenus battleSysMenus;
    public GameObject dialogBoxMenu;
    public GameObject bookSelectMenu;
    public GameObject spellSelectMenu;
    public GameObject rohkanSpellsMenu;
    public GameObject paperiousSpellsMenu;
    public GameObject scissoraSpellsMenu;
    public GameObject spellCardUI;
    public GameObject waitingScreen;

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


    [Header("Photon")]
    public PhotonView playerPhotonView;
    SpellsScriptableObject player1CurrentSpell;
    SpellsScriptableObject player2CurrentSpell;
    SpellsScriptableObject p2SpellInQueue;

    [Header("Tween")]
    public LeenTweenSystem tweenSystem;

    [Header("Database")]
    [SerializeField] BattleSystemDatabases battleSysDatabase;

    BattleState state;

    bool player2IsHere = false;
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
        //if (onOrOff == true)
            //tweenSystem.GameObjectSelected(dialogBoxMenu);
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
        battleSysMenus.OpenCloseRohkanSpells(onOrOff);
    }
    public void OnOffPaperiousSpellsSelectMenu(bool onOrOff)
    {
        battleSysMenus.OpenClosePaperiousSpells(onOrOff);
    }
    public void OnOffScissoraSpellsSelectMenu(bool onOrOff)
    {
        battleSysMenus.OpenCloseScissoraSpells(onOrOff);
    }

    public void OnOffSpellCard(bool onOrOff) 
    {

        if (onOrOff)
        {
            LeanTween.moveLocalX(spellCardUI, -440, .2f);
            LeanTween.scale(spellCardUI, Vector3.one, .4f);
            //LeanTween.alpha(spellCardUI)
        }
        else 
        {
            LeanTween.moveLocalX(spellCardUI, -445, .5f);
            LeanTween.scale(spellCardUI, Vector3.zero, .5f);
            //LeanTween.alpha(spellCardUI, 0, .5f);
        }
    }

    public void OnOffWaitingScreen(bool onOrOff) 
    {
        //After a player selects a spell they may need to wait until the opponent selects theirs.
        waitingScreen.SetActive(onOrOff);
    }
    #endregion

    void Start()
    {
        photonView.RPC("SetupBattle", PhotonTargets.All);
        if (!photonView.isMine)
        {
            playerPhotonView.RPC("PlayerTwoHasArrived", PhotonTargets.All);
        }
    }

    public void FixedUpdate()
    {
        playerPhotonView.RPC("CheckingTimer", PhotonTargets.AllBuffered);
        
        if (photonView.isMine)
        {
            CheckingBothPlayersSpellInput();
        }
    }

    public void Update()
    {
        //This will be HandleUpdate Later

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

    [PunRPC]
    public IEnumerator SetupBattle()
    {
        if (PlayerPrefs.HasKey("PlayerCaster"))
        {
            var playerCasterData = PlayerPrefs.GetString("PlayerCaster");
            Debug.Log("Pref Found! = " + playerCasterData);
            playerUnit.NewSetUp(battleSysDatabase.RetrieveCasterData(playerCasterData));
        }
        else
            playerUnit.SetUp();

        //playerUnit.SetUp();
        playerHUD.SetData(playerUnit.TheCaster, playerUnit.casterHealth);

        enemyUnit.SetUp();
        enemyHUD.SetData(enemyUnit.TheCaster, enemyUnit.casterHealth);
        yield return playerUnit.CheckForPassive(playerUnit, enemyUnit, playerUnit.CasterHUD.CasterAnim, RoundPositionForPassives.GameStart);
        yield return enemyUnit.CheckForPassive(enemyUnit, playerUnit, enemyUnit.CasterHUD.CasterAnim, RoundPositionForPassives.GameStart);
        AilmentsDB.Initialize();
        currentRoundNum = 0;
        UpdateCurrentRound(currentRoundNum);
        roundTimerNum = 6;

        if (photonView.isMine)
        {
            playerInfoForSpells.SetSpellData();
            playerInfoForSpells.SetSpellDataForTotalList();
            player2InfoForSpells.SetSpellDataForTotalList();
            SetPlayerSpellData(playerInfoForSpells);
            
            yield return battleDialogSystem.TypeDialog("CASTERS READY!");
        }
        else 
        {
            playerInfoForSpells.SetSpellDataForTotalList();
            player2InfoForSpells.SetSpellDataForTotalList();
            SetPlayerSpellData(player2InfoForSpells);   //May not need anymore
        }

        ResetCurrentSpellForBothPlayers();
        PlayerAction();
    }

    void SetPlayerSpellData(PlayerCaster playerCaster)
    {
        //Setting Rohkan(Rock) Spell Data 
        rohkanSpellData[0].spellImage.sprite = playerCaster.RohkanSpell1.Base.SpellSprite;
        rohkanSpellData[0].spellName.text = playerCaster.RohkanSpell1.Base.SpellName;
        rohkanSpellData[0].spellDescription.text = playerCaster.RohkanSpell1.Base.SpellDescription;
        rohkanSpellData[0].spellBonusDescription.text = playerCaster.RohkanSpell1.Base.SpellBonusDescription;
        rohkanSpellData[0].spellData = playerCaster.RohkanSpell1.Base;

        rohkanSpellData[1].spellImage.sprite = playerCaster.RohkanSpell2.Base.SpellSprite;
        rohkanSpellData[1].spellName.text = playerCaster.RohkanSpell2.Base.SpellName;
        rohkanSpellData[1].spellDescription.text = playerCaster.RohkanSpell2.Base.SpellDescription;
        rohkanSpellData[1].spellBonusDescription.text = playerCaster.RohkanSpell2.Base.SpellBonusDescription;
        rohkanSpellData[1].spellData = playerCaster.RohkanSpell2.Base;


        //Setting Paperious(Paper) Spell Data 
        paperiousSpellData[0].spellImage.sprite = playerCaster.PaperiousSpell1.Base.SpellSprite;
        paperiousSpellData[0].spellName.text = playerCaster.PaperiousSpell1.Base.SpellName;
        paperiousSpellData[0].spellDescription.text = playerCaster.PaperiousSpell1.Base.SpellDescription;
        paperiousSpellData[0].spellBonusDescription.text = playerCaster.PaperiousSpell1.Base.SpellBonusDescription;
        paperiousSpellData[0].spellData = playerCaster.PaperiousSpell1.Base;

        paperiousSpellData[1].spellImage.sprite = playerCaster.PaperiousSpell2.Base.SpellSprite;
        paperiousSpellData[1].spellName.text = playerCaster.PaperiousSpell2.Base.SpellName;
        paperiousSpellData[1].spellDescription.text = playerCaster.PaperiousSpell2.Base.SpellDescription;
        paperiousSpellData[1].spellBonusDescription.text = playerCaster.PaperiousSpell2.Base.SpellBonusDescription;
        paperiousSpellData[1].spellData = playerCaster.PaperiousSpell2.Base;


        //Setting Scissora(Scissors) Spell Data 
        scissoraSpellData[0].spellImage.sprite = playerCaster.ScissoraSpell1.Base.SpellSprite;
        scissoraSpellData[0].spellName.text = playerCaster.ScissoraSpell1.Base.SpellName;
        scissoraSpellData[0].spellDescription.text = playerCaster.ScissoraSpell1.Base.SpellDescription;
        scissoraSpellData[0].spellBonusDescription.text = playerCaster.ScissoraSpell1.Base.SpellBonusDescription;
        scissoraSpellData[0].spellData = playerCaster.ScissoraSpell1.Base;

        scissoraSpellData[1].spellImage.sprite = playerCaster.ScissoraSpell2.Base.SpellSprite;
        scissoraSpellData[1].spellName.text = playerCaster.ScissoraSpell2.Base.SpellName;
        scissoraSpellData[1].spellDescription.text = playerCaster.ScissoraSpell2.Base.SpellDescription;
        scissoraSpellData[1].spellBonusDescription.text = playerCaster.ScissoraSpell2.Base.SpellBonusDescription;
        scissoraSpellData[1].spellData = playerCaster.ScissoraSpell2.Base;

        additionalSpellDatas[0].spellName = playerCaster.BlockAsSpell.Base.SpellName;
        additionalSpellDatas[0].spellData = playerCaster.BlockAsSpell.Base;

    }

    #region StateChangingMethods
    [PunRPC]
    void PlayerAction() 
    {
        state = BattleState.PlayerAction;
        OnOffDialogBox(false);
        OnOffbookSelect(true);
        //OnOffSpellSelect(false);

        OnOffRohkanSpellsSelectMenu(false);
        OnOffPaperiousSpellsSelectMenu(false);
        OnOffScissoraSpellsSelectMenu(false);
    }
    [PunRPC]
    void BeginAttackPhase() 
    {
        state = BattleState.Busy;
        OnOffbookSelect(false);
        //OnOffRohkanSpellsSelectMenu(false);
        //OnOffPaperiousSpellsSelectMenu(false);
        //OnOffScissoraSpellsSelectMenu(false);
        OnOffDialogBox(true);
        //OnOffSpellSelect(false);
        OnOffWaitingScreen(false);

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

    void PlayerWaitState() 
    {
        state = BattleState.Waiting;
        OnOffbookSelect(false);
        OnOffRohkanSpellsSelectMenu(false);
        OnOffPaperiousSpellsSelectMenu(false);
        OnOffScissoraSpellsSelectMenu(false);
        OnOffDialogBox(false);
        //OnOffSpellSelect(false);
        OnOffWaitingScreen(true);
    }
    #endregion

    #region BookAndSpellSelectionMethods
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

            if (photonView.isMine)
            {
                if (currentBookSelectAction == 3 && playerUnit.BlockCD > 0)
                    currentBookSelectAction = 0;
            }
            else 
            {
                if (currentBookSelectAction == 3 && enemyUnit.BlockCD > 0)
                    currentBookSelectAction = 0;
            }
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

                PlayerWaitState();
                if (photonView.isMine)
                {
                    player1CurrentSpell = additionalSpellDatas[0].spellData;    //pretty sure I dont need these anymore
                    playerPhotonView.RPC("SendP1SpellData", PhotonTargets.All, 6);
                    Debug.Log("Player 1 Current Spell Name" + player1CurrentSpell.SpellName);
                }
                else
                {
                    p2SpellInQueue = additionalSpellDatas[0].spellData; //This too
                    playerPhotonView.RPC("SendP2SpellData", PhotonTargets.All, 6);
                    Debug.Log("Player 2 Current Spell Name" + player2CurrentSpell.SpellName);
                }

            }
        }
    }

    void HandleSpellSelect() 
    {
        int spellPosNum;

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
                
                PlayerWaitState();
                if (photonView.isMine)
                {
                    spellPosNum = currentSpellSelectAction;
                    playerPhotonView.RPC("SendP1SpellData", PhotonTargets.All, spellPosNum);
                    Debug.Log("Player 1 Current Spell Name" + player1CurrentSpell.SpellName);
                }
                else
                {
                    spellPosNum = currentSpellSelectAction;
                    playerPhotonView.RPC("SendP2SpellData", PhotonTargets.All, spellPosNum);
                    Debug.Log("Player 2 Current Spell Name" + player2CurrentSpell.SpellName);

                }
            }
        }
        else if (state == BattleState.PlayerPaperiousSpellSelect)
        {
            UpdateHandlePaperiousSpellSelector(currentSpellSelectAction);
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                PlayerWaitState();
               
                if (photonView.isMine)
                {
                    spellPosNum = currentSpellSelectAction + 2;
                    playerPhotonView.RPC("SendP1SpellData", PhotonTargets.All, spellPosNum);
                    Debug.Log("Player 1 Current Spell Name" + player1CurrentSpell.SpellName);
                }
                else
                {
                    spellPosNum = currentSpellSelectAction + 2;
                    playerPhotonView.RPC("SendP2SpellData", PhotonTargets.All, spellPosNum);
                    Debug.Log("Player 2 Current Spell Name" + player2CurrentSpell.SpellName);
                }

            }
        }
        else if (state == BattleState.PlayerScissoraSpellSelect)
        {
            UpdateHandleScissoraSelector(currentSpellSelectAction);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerWaitState();
                
                if (photonView.isMine)
                {
                    spellPosNum = currentSpellSelectAction + 4;
                    playerPhotonView.RPC("SendP1SpellData", PhotonTargets.All, spellPosNum);
                    Debug.Log("Player 1 Current Spell Name" + player1CurrentSpell.SpellName);
                }
                else
                {
                    spellPosNum = currentSpellSelectAction + 4;
                    playerPhotonView.RPC("SendP2SpellData", PhotonTargets.All, spellPosNum);
                    Debug.Log("Player 2 Current Spell Name" + player2CurrentSpell.SpellName);
                }
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
    public IEnumerator AttackPhase() 
    {
        SpellsScriptableObject playerSpell = player1CurrentSpell;
        SpellsScriptableObject enemySpell = player2CurrentSpell;

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
        ResetCurrentSpellForBothPlayers();

        PlayerAction();
    }
    [PunRPC]
    IEnumerator PerformSpell(BattleUnit sourceUnit, BattleUnit targetUnit, SpellsScriptableObject spellToPerform, bool isEmpowered) 
    {
        yield return battleDialogSystem.TypeDialog(sourceUnit.TheCaster.CasterBase.CasterName + " Casted " + spellToPerform.SpellName);
        bool hasFainted = false;

        PopulateSpellCard(spellToPerform);
        OnOffSpellCard(true);

        GameObject spellGameObject = Instantiate(spellToPerform.SpellObject, sourceUnit.spellCastPos.position, sourceUnit.spellCastPos.rotation);


        if (isEmpowered)
            yield return spellGameObject.GetComponent<ISpellAbility>().EmpoweredAbility(sourceUnit, targetUnit);
        else
            yield return spellGameObject.GetComponent<ISpellAbility>().BasicAbility(sourceUnit, targetUnit);


        if (targetUnit.casterHealth <= 0)   //Coming Soon, Revamped KO code for when someone is defeated
            hasFainted = true;

       

        if (sourceUnit == playerUnit) 
        {
            yield return enemyHUD.UpdateHP((float)targetUnit.casterHealth / targetUnit.CasterHUD.HPBar.maxHealth);
        }
        else if(sourceUnit == enemyUnit)
            yield return playerHUD.UpdateHP((float)targetUnit.casterHealth / targetUnit.CasterHUD.HPBar.maxHealth);

        if (hasFainted)
        {
            yield return battleDialogSystem.TypeDialog("K O !"); //Coming Soon, Revamped KO code for when someone is defeated
        }

        yield return new WaitForSeconds(.4f);
        OnOffSpellCard(false);

    }

    public SpellsScriptableObject BasicEnemyAI(BattleUnit sourceUnit) 
    {
        //Good chance I'll put this in another class later. 
        //For now, here's some basic enemy AI, where the moves they'll pick are entirely random

        if (sourceUnit.BlockCD <= 0)
            return enemyAIInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyAIInfoForSpells.TotalEnemyListOfSpells.Count)].Base;
        else
            return enemyAIInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyAIInfoForSpells.TotalEnemyListOfSpells.Count - 1)].Base;

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
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedRohkanSpellThisRound = true;
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells++;
                    return spellTypeIcons[1];
                }
                else
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.WonButNOSWThisRound = true;
                    sourceUnit.ClangThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells++;
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedRohkanSpellThisRound = true;
                    return spellTypeIcons[0];
                }
            }
            else if (enemySpell.GodType == SpellGodType.Paperious)
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.ClangThisRound = false;

                sourceUnit.BonusPlayerStats.SuccessfullyCastedRohkanSpellThisRound = true;
                sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells++;

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
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedRohkanSpells++;
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedRohkanSpellThisRound = true;
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
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedPaperiousSpellThisRound = true;
                    return spellTypeIcons[4];
                }
                else
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.WonButNOSWThisRound = true;
                    sourceUnit.ClangThisRound = false;

                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedPaperiousSpells++;
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedPaperiousSpellThisRound = true;
                    return spellTypeIcons[3];
                }
            }
            else if (enemySpell.GodType == SpellGodType.Scissora)
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.ClangThisRound = false;

                sourceUnit.BonusPlayerStats.SuccessfullyCastedPaperiousSpellThisRound = true;
                sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedPaperiousSpells++;

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
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedPaperiousSpells++;
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedPaperiousSpellThisRound = true;
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
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedScissoraSpellThisRound = true;
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells++;
                    return spellTypeIcons[7];
                }
                else
                {
                    sourceUnit.SWThisRound = true;
                    sourceUnit.WonButNOSWThisRound = true;
                    sourceUnit.ClangThisRound = false;

                    sourceUnit.BonusPlayerStats.SuccessfullyCastedScissoraSpellThisRound = true;
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells++;
                    return spellTypeIcons[6];
                }
            }
            else if (enemySpell.GodType == SpellGodType.Rohkan)
            {
                sourceUnit.SWThisRound = false;
                sourceUnit.SWThisRound = false;

                sourceUnit.BonusPlayerStats.SuccessfullyCastedScissoraSpellThisRound = true;
                sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells++;

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
                    sourceUnit.BonusPlayerStats.TotalSuccessfullyCastedScissoraSpells++;
                    sourceUnit.BonusPlayerStats.SuccessfullyCastedScissoraSpellThisRound = true;
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
            if (!sourceUnit.HasSpecialBlock) 
            {
                sourceUnit.BlockCD = 2;
                sourceUnit.SetBlockIconAccordingToCD(sourceUnit.BlockCD);

            }
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

                break;
            case 2: //P2 Won the Round
                orderNum = 2;
                yield return P1.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, true);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                break;
            case 3: //P1 Win, No Bonus
                orderNum = 3;
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                break;
            case 4: //P2 Win, No Bonus
                orderNum = 4;
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                break;
            case 5: //CLANG
                orderNum = 5;
                yield return battleDialogSystem.TypeDialog("CLAAANG!!");
                yield return new WaitForSeconds(.75f);
                yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.BeforeAttack);
                yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.AfterPlayerAttacks);
                break;
            case 6: //P1 Successfully Blocked
                orderNum = 6;
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);  //Will Not Damage
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

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
                break;
            case 8: //P2 successfully Blocked
                orderNum = 8;
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, false);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, false);  //Will Not Damage
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                break;
            case 9: //P2 Block Breaks P1
                orderNum = 9;
                yield return battleDialogSystem.TypeDialog("Block Break!!");

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P2, P1, p2Spell, true);
                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);
                //yield return CheckForContinuation(P1);

                break;
            case 10: //P1 Block Breaks P2
                orderNum = 10;
                yield return battleDialogSystem.TypeDialog("Block Break!!");

                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.BeforeAttack);
                yield return PerformSpell(P1, P2, p1Spell, true);
                yield return P1.CheckForPassive(P1, P2, P1.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                yield return P2.CheckForPassive(P2, P1, P2.CasterHUD.CasterAnim, RoundPositionForPassives.AfterPlayerAttacks);

                break;


        }
        if (orderNumber == 0)
        {
            //Error - Returning 0 is an unaccounted for outcome
            Debug.LogWarning("Unaccounted For Outcome!");
            yield return new WaitForSeconds(2f);
        }
        
    }

    public IEnumerator EndOfRoundResetting(BattleUnit sourceUnit) 
    {
        yield return sourceUnit.RoundEndAilments();
        yield return sourceUnit.NaturalRust();
        
        if (!sourceUnit.PlayerAttemptedBlockThisRound) 
        {
            sourceUnit.BlockCD--;
            if (sourceUnit.BlockCD <= 0)
                sourceUnit.BlockCD = 0;
            if(!sourceUnit.HasSpecialBlock)
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

        sourceUnit.BonusPlayerStats.ResetNecessaryResettables();

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

            yield return DetermineEndRoundPassive(P1, P2);
            yield return EndOfRoundResetting(P2);
            yield return EndOfRoundResetting(P1);
        }
        else if (orderNum == 2 || orderNum == 4)
        {
            yield return P2.AllyHUD.RunMove(P2, P1);
            yield return P1.AllyHUD.RunMove(P1, P2);
            yield return new WaitForSeconds(.75f);

            yield return DetermineEndRoundPassive(P1, P2);
            yield return EndOfRoundResetting(P1);
            yield return EndOfRoundResetting(P2);
        }
        else if (orderNum == 6)
        {
            yield return P1.AllyHUD.RunMove(P1, P2);
            yield return new WaitForSeconds(.75f);

            yield return DetermineEndRoundPassive(P1, P2);
            yield return EndOfRoundResetting(P2);
            yield return EndOfRoundResetting(P1);
        }
        else if (orderNum == 8)
        {
            yield return P2.AllyHUD.RunMove(P2, P1);
            yield return new WaitForSeconds(.75f);

            yield return DetermineEndRoundPassive(P1, P2);
            yield return EndOfRoundResetting(P1);
            yield return EndOfRoundResetting(P2);
        }
        else if (orderNum == 5 || orderNum == 7)
        {
            if (p1Priority)
            {
                yield return new WaitForSeconds(.75f);

                yield return DetermineEndRoundPassive(P1, P2);
                yield return EndOfRoundResetting(P1);
                yield return EndOfRoundResetting(P2);
            }
            else
            {
                yield return new WaitForSeconds(.75f);

                yield return DetermineEndRoundPassive(P1, P2);
                yield return EndOfRoundResetting(P2);
                yield return EndOfRoundResetting(P1);
            }
        }
        else if (orderNum == 9)
        {
            yield return P2.AllyHUD.RunMove(P2, P1);
            yield return new WaitForSeconds(.75f);

            yield return DetermineEndRoundPassive(P1, P2);
            yield return EndOfRoundResetting(P1);
            yield return EndOfRoundResetting(P2);
        }
        else if (orderNum == 10) 
        {
            yield return P1.AllyHUD.RunMove(P1, P2);
            yield return new WaitForSeconds(.75f);

            yield return DetermineEndRoundPassive(P1, P2);
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

    IEnumerator DetermineEndRoundPassive(BattleUnit P1, BattleUnit P2)
    {
        if (orderNum == 1 || orderNum == 3 || orderNum == 6 || orderNum == 10)
        {
            yield return EndRoundPassiveOrderAlreadyDetermined(P1, P2);
        }
        else if (orderNum == 2 || orderNum == 4 || orderNum == 8 || orderNum == 9)
        {
            yield return EndRoundPassiveOrderAlreadyDetermined(P2, P1);
        }
        else if (orderNum == 5 || orderNum == 7) 
        {
            yield return PassiveOrderLogic(P1, P2, RoundPositionForPassives.EndOfRound);
        }
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
        }

        orderNum = 0;
    }
    public void ResetCurrentSpellForBothPlayers() 
    {
        p2SpellInQueue = null;
        player1CurrentSpell = null;
        player2CurrentSpell = null;
    }
    [PunRPC]
    public void CheckingTimer() 
    {
        if (state == BattleState.PlayerAction || state == BattleState.PlayerRohkanSpellSelect || state == BattleState.PlayerPaperiousSpellSelect || state == BattleState.PlayerScissoraSpellSelect 
            || state == BattleState.Waiting)
        {
            if (player2IsHere)
            {

                roundTimerNum -= Time.deltaTime;
                //tweenSystem.TimerScaleDownAndBackUp(roundTimerText.gameObject, .5f);
                roundTimerText.text = Mathf.FloorToInt(roundTimerNum % 60).ToString();
                //tweenSystem.TimerScaleDownAndBackUp(roundTimerText.gameObject, .5f);
                if (roundTimerNum <= 0)
                {
                    roundTimerNum = 0;
                    //Debug.Log("Ran Out of Time!!"); 
                    //Skipped Turn
                }
            }
        }
    }

    #endregion
    #region Necessary Online Functions
    //Here are some functions needed for online mode

    void BasicEnemyAIOnline(BattleUnit sourceUnit) 
    {
        if (sourceUnit.BlockCD <= 0)
            player2CurrentSpell = enemyAIInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyAIInfoForSpells.TotalEnemyListOfSpells.Count)].Base;
        else
            player2CurrentSpell = enemyAIInfoForSpells.TotalEnemyListOfSpells[Random.Range(0, enemyAIInfoForSpells.TotalEnemyListOfSpells.Count - 1)].Base;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
        
    }

    [PunRPC]
    void PlayerTwoHasArrived() 
    {
        player2IsHere = true;
    }

    void CheckingBothPlayersSpellInput() 
    {
        if (state != BattleState.Busy)
        {
            if (player1CurrentSpell != null && player2CurrentSpell != null)
            {
                Debug.Log("Battle phase begun");
                photonView.RPC("BeginAttackPhase", PhotonTargets.All);
                photonView.RPC("AttackPhase", PhotonTargets.All);
            }
        }
    }

    [PunRPC]
    void SendP2SpellData(int spellPos) 
    {
        player2CurrentSpell = player2InfoForSpells.TotalListOfSpells[spellPos].Base;
    }
    [PunRPC]
    void SendP1SpellData(int spellPos)
    {
        player1CurrentSpell = playerInfoForSpells.TotalListOfSpells[spellPos].Base;
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

