using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAllyAbility : MonoBehaviour
{
    public int baseDamageOrHeal;
    public int empoweredDamageOrHeal;
    protected private BattleUnit sourceU;
    protected private BattleUnit targetU;
    protected private bool continueBattle = false;

    public abstract IEnumerator BasicAbility(BattleUnit sourceUnit, BattleUnit targetUnit, Animator anim);

    public IEnumerator PauseAfterAnimation(Animator anim)
    {
        //Debug.Log("Anim Length = " + anim.runtimeAnimatorController.animationClips[0].);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(anim.runtimeAnimatorController.animationClips[0].length +.3f);
    }
}
