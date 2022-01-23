using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ISpellAbility : MonoBehaviour
{
    public int baseDamageOrHeal;
    public int empoweredDamageOrHeal;
    public Animator anim;
    public SummonedUnit unitToSummon; //May be left blank

    [Header("Particle Systems")]
    public GameObject basicParticleSys;
    public GameObject empoweredParticleSys;
    protected private BattleUnit sourceU;
    protected private BattleUnit targetU; 
    public bool continueBattle { get; set; } = false;
    
    public abstract IEnumerator BasicAbility(BattleUnit sourceUnit, BattleUnit targetUnit);
    public abstract IEnumerator EmpoweredAbility(BattleUnit sourceUnit,BattleUnit targetUnit);

    public void BasicDamageTheOpponent()
    {
        //Basic Func to damage the opponent with base damage
        //More complicated forms of dealing damage must be coded uniqely

        float damageToShakeConv = 0f;
        Mathf.Clamp(damageToShakeConv, 0f, 1.5f);
        damageToShakeConv = (baseDamageOrHeal + sourceU.TotalBonusDmgThisRound) / 90f ;

        if (!targetU.PlayerSuccessfullyBlockedThisRound && !targetU.BonusPlayerStats.IsInvincible)
        {
            targetU.TakeDamage(baseDamageOrHeal + sourceU.TotalBonusDmgThisRound);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));

            targetU.CasterHUD.CasterAnim.SetTrigger("Hit");

            ScreenShakeController.instance.StartShake(damageToShakeConv, .35f);
        }
        else 
        {
            targetU.TakeDamage(0);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));
        }
    }

    public void EmpoweredDamageTheOpponent() 
    {
        //Basic Func to damage the opponent with empowered damage
        //More complicated forms of dealing damage must be coded uniqely

        float damageToShakeConv = 0f;
        Mathf.Clamp(damageToShakeConv, 0f, 1.5f);
        damageToShakeConv = (empoweredDamageOrHeal + sourceU.TotalBonusDmgThisRound) / 90f;

        if (!targetU.PlayerSuccessfullyBlockedThisRound && !targetU.BonusPlayerStats.IsInvincible)
        {
            targetU.TakeDamage(empoweredDamageOrHeal + sourceU.TotalBonusDmgThisRound);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));

            targetU.CasterHUD.CasterAnim.SetTrigger("Hit");

            ScreenShakeController.instance.StartShake(damageToShakeConv, .4f);
        }
        else 
        {
            targetU.TakeDamage(0);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));
        }

    }
    public void BasicAOEDamage()
    {
        if (!targetU.PlayerSuccessfullyBlockedThisRound && !targetU.BonusPlayerStats.IsInvincible)
        {
            targetU.TakeDamage(baseDamageOrHeal + sourceU.TotalBonusDmgThisRound);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));
            StartCoroutine(targetU.AllyHUD.TakeDamageForAll(baseDamageOrHeal + sourceU.TotalBonusDmgThisRound ));
            targetU.CasterHUD.CasterAnim.SetTrigger("Hit");
        }
        else 
        {
            targetU.TakeDamage(0);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));
            StartCoroutine(targetU.AllyHUD.TakeDamageForAll(0));
        }
    }

    public void EmpoweredAOEDamage()
    {
        if (!targetU.PlayerSuccessfullyBlockedThisRound && !targetU.BonusPlayerStats.IsInvincible)
        {
            targetU.TakeDamage(empoweredDamageOrHeal + sourceU.TotalBonusDmgThisRound);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));
            StartCoroutine(targetU.AllyHUD.TakeDamageForAll(baseDamageOrHeal + sourceU.TotalBonusDmgThisRound));
            targetU.CasterHUD.CasterAnim.SetTrigger("Hit");
        }
        else 
        {
            targetU.TakeDamage(0);
            StartCoroutine(targetU.CasterHUD.UpdateArmor((float)targetU.casterArmor / targetU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(targetU.CasterHUD.UpdateHP((float)targetU.casterHealth / targetU.CasterHUD.HPBar.maxHealth));
            StartCoroutine(targetU.AllyHUD.TakeDamageForAll(0));
        }
    }

    public void BasicHealUser() 
    {
        if (!targetU.PlayerSuccessfullyBlockedThisRound && !targetU.BonusPlayerStats.IsInvincible)
        {
            sourceU.TakeDamage(-baseDamageOrHeal);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(sourceU.CasterHUD.UpdateHP((float)sourceU.casterHealth / sourceU.CasterHUD.HPBar.maxHealth));
        }
        else 
        {
            sourceU.TakeDamage(0);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(sourceU.CasterHUD.UpdateHP((float)sourceU.casterHealth / sourceU.CasterHUD.HPBar.maxHealth));
        }
    }

    public void EmpoweredHealUser() 
    {
        if (!targetU.PlayerSuccessfullyBlockedThisRound)
        {
            sourceU.TakeDamage(-empoweredDamageOrHeal);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(sourceU.CasterHUD.UpdateHP((float)sourceU.casterHealth / sourceU.CasterHUD.HPBar.maxHealth));
        }
        else 
        {
            sourceU.TakeDamage(0);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
            StartCoroutine(sourceU.CasterHUD.UpdateHP((float)sourceU.casterHealth / sourceU.CasterHUD.HPBar.maxHealth));
        }
        
    }

    public void BasicGrantArmor() 
    {
        if (!targetU.PlayerSuccessfullyBlockedThisRound)
        {
            sourceU.AddArmor(baseDamageOrHeal);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
        }
        else 
        {
            sourceU.AddArmor(0);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
        }
    }

    public void EmpoweredGrantArmor() 
    {
        if (!targetU.PlayerSuccessfullyBlockedThisRound)
        {
            sourceU.AddArmor(empoweredDamageOrHeal);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
        }
        else
        {
            sourceU.AddArmor(0);
            StartCoroutine(sourceU.CasterHUD.UpdateArmor((float)sourceU.casterArmor / sourceU.CasterHUD.HPBar.maxArmor));
        }
    }

    public void SummonUnit() 
    {
        if (!targetU.PlayerSuccessfullyBlockedThisRound)
            StartCoroutine(sourceU.AllyHUD.SummonUnit(unitToSummon));

    }

    public void DestroyMe() 
    {
        Destroy(this.gameObject);
    }

    public void CheckForBuffsToApply() 
    {
        //This Function will check for a Buff to be applied once, then de-increments
        //The Damage Boosts, will add to the sourceUnits BONUSDMG Parameter
        //As a result, even for multiHitting moves, make sure this is called only ONCE for each players attack phase.
        //Multiple calls may result in exponential buff stacking
        for (int i = 0; i < sourceU.PlayerAilmentsAndBuffsStatus.Count; i++) 
        {
            if (sourceU.PlayerAilmentsAndBuffsStatus[i].BuffID != BuffID.none) 
            {
                BuffDB.ChannelToAllBuffBehavior(sourceU.PlayerAilmentsAndBuffsStatus[i].BuffID, sourceU, targetU, i);
            }
        }
    }

    public IEnumerator PauseAfterAnimation(Animator anim) 
    {
        //Debug.Log("Anim Length = " + anim.runtimeAnimatorController.animationClips[0].);
        yield return new WaitForEndOfFrame();
        //Debug.Log("Anim Length = " + anim.runtimeAnimatorController.animationClips[0].length);
        yield return new WaitForSeconds(anim.runtimeAnimatorController.animationClips[0].length + .25f);
    }


}
