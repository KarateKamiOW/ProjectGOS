using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] CastersScriptableObject casterBase;
    [SerializeField] bool isPlayer;
    [SerializeField] BattleHUD casterHUD;
    [SerializeField] SummonedAllyHUD allyHUD;
    [SerializeField] List<GameObject> additionalEffects;
    [SerializeField] Transform KOPos;
    [SerializeField] GameObject KOObj;
    [SerializeField] BonusPlayerStats bonusPlayerStats;
    [SerializeField] Camera battleCam;
    public Transform spellCastPos;

    #region Variables
    public int casterHealth { get; set; }
    public int casterArmor { get; set; }

    public GameObject CasterPassiveGO { get; set; }

    public bool IsPlayerOne { get { return IsPlayerOne;} }

    public BattleHUD CasterHUD { get { return casterHUD; } }
    public SummonedAllyHUD AllyHUD { get { return allyHUD; } }
    public List<GameObject> AdditionalEffects { get { return additionalEffects; } }
    public BonusPlayerStats BonusPlayerStats { get { return bonusPlayerStats; } }

    public int TotalBonusDmgThisRound { get; set; }
    public int BlockCD { get; set; }
    public bool HasSpecialBlock { get; set; }

    public bool SWThisRound { get; set; }

    public bool RohkanSW { get; set; }
    public bool RohkanBB { get; set; }
    public bool PaperiousSW { get; set; }
    public bool PaperiousBB { get; set; }
    public bool ScissoraSW { get; set; }
    public bool ScissoraBB { get; set; }
    public bool WonButNOSWThisRound { get; set; }

    public bool ClangThisRound { get; set; }
    public bool PlayerAttemptedBlockThisRound { get; set; }
    public bool PlayerSuccessfullyBlockedThisRound { get; set; }
    public bool ContinueBattle { get; set; } = false;

    public List<AilmentData> PlayerAilmentsAndBuffsStatus = new List<AilmentData>();
    public Queue<string> AilmentChanges { get; private set; } = new Queue<string>();
    public Caster TheCaster { get; set; }
    #endregion


    public void SetUp() 
    {
        TheCaster = new Caster(casterBase);

        CasterPassiveGO = Instantiate(TheCaster.CasterBase.CasterPassive, spellCastPos.position, spellCastPos.rotation);

        casterHealth = 200;
        casterHUD.CasterAnim.runtimeAnimatorController = TheCaster.CasterBase.CasterAnimator;

        BlockCD = 0;
        HasSpecialBlock = false;
        bonusPlayerStats.IsInvincible = false;
        AllyHUD.NullifyAll();

       
    }

    public void NewSetUp(Caster playerCaster) 
    {
        TheCaster = playerCaster;

        CasterPassiveGO = Instantiate(TheCaster.CasterBase.CasterPassive, spellCastPos.position, spellCastPos.rotation);

        casterHealth = 200;
        casterHUD.CasterAnim.runtimeAnimatorController = TheCaster.CasterBase.CasterAnimator;

        BlockCD = 0;
        AllyHUD.NullifyAll();

    }

    public bool TakeDamage(int damage) 
    {
        int updatedDamage = damage;

        //If damage is less than 0, that implies that the player is instead being healed
        if (casterArmor > 0 && damage > 0) 
        {
            casterArmor -= damage;
            if (casterArmor < 0)
                updatedDamage = -(casterArmor);

        }

        casterHealth -= updatedDamage;
        if (casterHealth > casterHUD.HPBar.maxHealth)
            casterHealth = (int)casterHUD.HPBar.maxHealth;

        if (casterHealth <= 0)
        {
            casterHealth = 0;
            StopAllCoroutines();
            StartCoroutine(CheckForHpAboveZero());
            return true;
        }
        else
            return false;
    }

    public bool AddArmor(int armorAdded) 
    {
        casterArmor += armorAdded;
        if (casterArmor > casterHUD.HPBar.maxArmor)
            casterArmor = (int)casterHUD.HPBar.maxArmor;

        if (casterArmor <= 0)
        {
            casterArmor = 0;
            return false;
        }
        else
            return true;
    }

    public void SetBlockIconAccordingToCD(int cd) 
    {
        casterHUD.SetBlockIconImage(cd);
    }

    #region Ailments + Buffs Status/Setting/Triggering
    public void SetAilmentStatus(AilmentBuffID ailmentID, int numOfStacks) 
    {

        var ailmentData = new AilmentData(AilmentsDB.TheAilment[ailmentID]);

        //Debug.Log("Ailment Statuc Count = " + AilmentStatus.Count);


        for (int i = 0; i <= PlayerAilmentsAndBuffsStatus.Count; i++)
        {
            if (PlayerAilmentsAndBuffsStatus.Count == 0 || PlayerAilmentsAndBuffsStatus[0] == null) 
            {
                //First Check if the list is empty; if so, simply add it in
                PlayerAilmentsAndBuffsStatus.Insert(i, ailmentData);
                PlayerAilmentsAndBuffsStatus[i].AilmentStacks = numOfStacks;
                casterHUD.SetAilmentorBuffIconImage(ailmentData.ID, i, ailmentData.SourceBuff);
                casterHUD.SetAilmentOrBuffStacksAndDuration(i, numOfStacks, ailmentData.AilmentCurrentDuration);
                break;
            }

            if (i == PlayerAilmentsAndBuffsStatus.Count)
            {
                //Next, Check if this is = to the length of the list + 1. If so, add it to the end
                PlayerAilmentsAndBuffsStatus.Insert(i, ailmentData);
                PlayerAilmentsAndBuffsStatus[i].AilmentStacks = numOfStacks;
                casterHUD.SetAilmentorBuffIconImage(ailmentData.ID, i, ailmentData.SourceBuff);
                casterHUD.SetAilmentOrBuffStacksAndDuration(i, numOfStacks, ailmentData.AilmentCurrentDuration);
                break;
            }

            if (ailmentID == PlayerAilmentsAndBuffsStatus[i].ID) 
            {
                //Otherwise, add stacks here
                PlayerAilmentsAndBuffsStatus[i].AilmentStacks += numOfStacks;
                PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration = PlayerAilmentsAndBuffsStatus[i].AilmentMaxDuration;
                casterHUD.SetAilmentOrBuffStacksAndDuration(i, PlayerAilmentsAndBuffsStatus[i].AilmentStacks, ailmentData.AilmentCurrentDuration);
                //Debug.Log("Ailment " + ailmentData.Name + "Stacks = " + PlayerAilmentsAndBuffsStatus[i].AilmentStacks);
                break;
            }

        }
       
    }

    public IEnumerator RoundEndAilments() 
    {
        List<AilmentData> tempAilmentBuffList = new List<AilmentData>();
        bool ailmentOrderHasChanged = false;
        int buffAndAilmentCount = PlayerAilmentsAndBuffsStatus.Count;

        if (PlayerAilmentsAndBuffsStatus.Count > 0) 
        {
            for (int i = 0; i < PlayerAilmentsAndBuffsStatus.Count; i++)
            {
                if (PlayerAilmentsAndBuffsStatus[i].BuffID == BuffID.none)
                {
                    if (!PlayerSuccessfullyBlockedThisRound && PlayerAilmentsAndBuffsStatus[i].ID != AilmentBuffID.rust && PlayerAilmentsAndBuffsStatus[i].ID != AilmentBuffID.torch)
                    {
                        //While the player didnt successfully block and the ailment is NOT rust
                        TakeDamage((PlayerAilmentsAndBuffsStatus[i].DamagePerStack * PlayerAilmentsAndBuffsStatus[i].AilmentStacks));
                        yield return casterHUD.UpdateArmor((float)casterArmor / CasterHUD.HPBar.maxArmor);
                        yield return casterHUD.UpdateHP((float)casterHealth / CasterHUD.HPBar.maxHealth);
                        Debug.Log("Took " + PlayerAilmentsAndBuffsStatus[i].Name + " Damage!");
                        yield return new WaitForSeconds(1f);
                        PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration--;
                        if (PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration <= 0)
                            ailmentOrderHasChanged = true;
                        casterHUD.SetAilmentOrBuffStacksAndDuration(i, PlayerAilmentsAndBuffsStatus[i].AilmentStacks, PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration);
                    }
                    else if (!PlayerSuccessfullyBlockedThisRound && PlayerAilmentsAndBuffsStatus[i].ID == AilmentBuffID.rust)
                    {
                        //While the player didnt successfully block and the ailment IS rust
                        if (casterArmor > 0)
                        {
                            AddArmor(-(PlayerAilmentsAndBuffsStatus[i].DamagePerStack * PlayerAilmentsAndBuffsStatus[i].AilmentStacks));
                            yield return casterHUD.UpdateArmor((float)casterArmor / CasterHUD.HPBar.maxArmor);
                            yield return new WaitForSeconds(1f);
                            PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration--;
                            if (PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration <= 0)
                                ailmentOrderHasChanged = true;
                            casterHUD.SetAilmentOrBuffStacksAndDuration(i, PlayerAilmentsAndBuffsStatus[i].AilmentStacks, PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration);
                        }
                        else 
                        {
                            TakeDamage((1 * PlayerAilmentsAndBuffsStatus[i].AilmentStacks));
                            yield return casterHUD.UpdateHP((float)casterHealth / CasterHUD.HPBar.maxHealth);
                            Debug.Log("Took " + PlayerAilmentsAndBuffsStatus[i].Name + " Damage!");
                            yield return new WaitForSeconds(1f);
                            PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration--;
                            if (PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration <= 0)
                                ailmentOrderHasChanged = true;
                            casterHUD.SetAilmentOrBuffStacksAndDuration(i, PlayerAilmentsAndBuffsStatus[i].AilmentStacks, PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration);
                        }
                    }
                    else if (!PlayerSuccessfullyBlockedThisRound && PlayerAilmentsAndBuffsStatus[i].ID == AilmentBuffID.torch) 
                    {
                        //While the player didnt successfully block and the ailment IS TORCH
                        //Coming Soon
                    }
                    else
                    {
                        PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration--;
                        if (PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration <= 0)
                            ailmentOrderHasChanged = true;
                        casterHUD.SetAilmentOrBuffStacksAndDuration(i, PlayerAilmentsAndBuffsStatus[i].AilmentStacks, PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration);

                    }
                }
                else 
                {
                    PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration--;
                    if (PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration <= 0)
                        ailmentOrderHasChanged = true;
                    casterHUD.SetAilmentOrBuffStacksAndDuration(i, PlayerAilmentsAndBuffsStatus[i].AilmentStacks, PlayerAilmentsAndBuffsStatus[i].AilmentCurrentDuration);
                }
            }


            if (ailmentOrderHasChanged) 
            {
            
                for (int j = 0; j < PlayerAilmentsAndBuffsStatus.Count; j++)
                {
                    if (PlayerAilmentsAndBuffsStatus[j].AilmentCurrentDuration > 0)
                    {
                        var ailmentData = new AilmentData(PlayerAilmentsAndBuffsStatus[j]);
                        tempAilmentBuffList.Add(ailmentData);
                    }
                }
                
                PlayerAilmentsAndBuffsStatus.Clear();
                casterHUD.ClearALLAilmentorBuffIconImageAndData(buffAndAilmentCount);
               
                PlayerAilmentsAndBuffsStatus = new List<AilmentData>(tempAilmentBuffList);
                

                for (int k = 0; k < PlayerAilmentsAndBuffsStatus.Count; k++)
                {
                    casterHUD.SetAilmentorBuffIconImage(PlayerAilmentsAndBuffsStatus[k].ID, k, PlayerAilmentsAndBuffsStatus[k].SourceBuff);
                    casterHUD.SetAilmentOrBuffStacksAndDuration(k, PlayerAilmentsAndBuffsStatus[k].AilmentStacks, PlayerAilmentsAndBuffsStatus[k].AilmentCurrentDuration);
                }
            }
        }
    }

    public IEnumerator NaturalRust() 
    {
        if (casterArmor > 0) 
        {
            AddArmor(-5);
            yield return casterHUD.UpdateArmor((float)casterArmor / CasterHUD.HPBar.maxArmor);
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    public IEnumerator CheckForPassive(BattleUnit sourceU, BattleUnit targetU, Animator playerAnim, RoundPositionForPassives roundPos) 
    {
        //get the anim from the casterHUD.
        yield return CasterPassiveGO.GetComponent<ACasterPassive>().PassiveAbility(sourceU, targetU, playerAnim, roundPos);
    }

    public IEnumerator CheckForHpAboveZero() 
    {
        if (casterHealth <= 0) 
        {
            casterHealth = 0;
            //Zoom In
            battleCam.orthographicSize = Mathf.Lerp(battleCam.orthographicSize, 3, .7f);
            battleCam.transform.position = Vector3.Lerp(battleCam.transform.position, this.transform.position, .7f);

            //Instantiate KO Object
            GameObject KOObject = Instantiate(KOObj, KOPos.position, KOPos.rotation);
            //Slow Time
            Time.timeScale = .5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return new WaitForSecondsRealtime(3f);
            //Reset Time
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return new WaitForSeconds(2f);
            //Reset Camera
            battleCam.orthographicSize = Mathf.Lerp(battleCam.orthographicSize, 5, .7f);
            battleCam.transform.position = Vector3.Lerp(battleCam.transform.position, Vector3.zero, .7f);
            yield return new WaitForSecondsRealtime(.5f);
            battleCam.orthographicSize = 5f;
            battleCam.transform.position = Vector3.zero;
            yield return new WaitForSeconds(1.5f);
            StopAllCoroutines();
            //Post Game screen
        }
    }
}
