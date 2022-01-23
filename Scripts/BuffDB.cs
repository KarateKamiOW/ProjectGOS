using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDB 
{
    public static void ChannelToAllBuffBehavior(BuffID buffID, BattleUnit sourceUnit, BattleUnit targetUnit, int listPos) 
    {
        switch (buffID) 
        {
            case BuffID.accusight:
                Accusight(sourceUnit, targetUnit, listPos);
                break;
            case BuffID.papercutBuff:
                PaperCutBuff(sourceUnit, listPos);
                break;
            case BuffID.clang:
                CLANG(sourceUnit, listPos);
                break;
            case BuffID.fiaPortalBuff:
                FiasPortalBuff(sourceUnit, listPos);
                break;
            case BuffID.sharpenBuff:
                SharpenBuff(sourceUnit,targetUnit, listPos);
                break;
            default:
                Debug.Log("Err0r");
                break;
        }
    }

    public static void CLANG(BattleUnit sourceUnit, int listPos) 
    {
        if (sourceUnit.SWThisRound || sourceUnit.RohkanBB || sourceUnit.ScissoraBB || sourceUnit.PaperiousBB)
        {
            sourceUnit.TotalBonusDmgThisRound += (sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].AilmentStacks * 5);
        }
        else 
        {
            sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].AilmentCurrentDuration = 0;
        }
    }

    public static void Accusight(BattleUnit sourceUnit, BattleUnit targetUnit, int listPos) 
    {
        if (sourceUnit.SWThisRound) 
        {
            if (sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].IsEmpowered)
            {
                if (!targetUnit.BonusPlayerStats.IsInvincible)
                    targetUnit.SetAilmentStatus(AilmentBuffID.bleed, 4);
            }
            else 
            { 
                if(!targetUnit.BonusPlayerStats.IsInvincible)
                    targetUnit.SetAilmentStatus(AilmentBuffID.bleed, 3);
            }

            Debug.Log("Perform Accusight DeBuff");
            sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].AilmentCurrentDuration = 0;
        }
    }

    public static void PaperCutBuff(BattleUnit sourceUnit, int listPos) 
    {
        if (sourceUnit.ScissoraSW || sourceUnit.ScissoraBB) 
        {
            sourceUnit.TotalBonusDmgThisRound += 10;
            sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].AilmentCurrentDuration = 0;
        }
    
    }

    public static void FiasPortalBuff(BattleUnit sourceUnit, int listPos) 
    {
        sourceUnit.TotalBonusDmgThisRound += 10;
        sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].AilmentCurrentDuration = 0;
    }

    public static void SharpenBuff(BattleUnit sourceUnit, BattleUnit targetUnit, int listPos) 
    {
        if (sourceUnit.ScissoraSW || sourceUnit.ScissoraBB) 
        {
            if (sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].IsEmpowered)
            {
                if (!targetUnit.BonusPlayerStats.IsInvincible) 
                {
                    sourceUnit.TotalBonusDmgThisRound += 15;
                    targetUnit.SetAilmentStatus(AilmentBuffID.bleed, 4);

                }
            }
            else 
            {
                if (!targetUnit.BonusPlayerStats.IsInvincible) 
                {
                    sourceUnit.TotalBonusDmgThisRound += 10;
                    targetUnit.SetAilmentStatus(AilmentBuffID.bleed, 2);
                }
            }
            sourceUnit.PlayerAilmentsAndBuffsStatus[listPos].AilmentCurrentDuration = 0;
        }
    }

}
