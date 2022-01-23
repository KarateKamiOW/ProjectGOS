using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilmentsDB
{
    //Ailment and Buffs Initializer
    public static void Initialize() 
    {
        foreach (var kvp in TheAilment) 
        {
            var ailmentID = kvp.Key;
            var ailment = kvp.Value;

            ailment.ID = ailmentID;

        
        }

        //Debug.Log("Initialized");
    }
    
    public static Dictionary<AilmentBuffID, Ailments> TheAilment { get; set; } = new Dictionary<AilmentBuffID, Ailments>()
    {
        
        {//Toxin
            AilmentBuffID.tox,
            new Ailments()
            {
                Name = "Toxin",
                StartMessage = "Toxin applied! ",
                DamagePerStack = 2,
                AilmentCurrentDuration = 3,
                AilmentMaxDuration = 3,

                BuffID = BuffID.none,
                SourceBuff = true //Shouldn't matter


            }

        },//Corrosion
        {
            AilmentBuffID.bleed,
            new Ailments()
            {
                Name = "Bleed",
                StartMessage = "Bleed applied! ",
                DamagePerStack = 5,
                AilmentCurrentDuration = 2,
                AilmentMaxDuration = 2,
               
                BuffID = BuffID.none,
                SourceBuff = true //Shouldn't matter


            }

        },//Bleed
        {
            AilmentBuffID.accusight,
            new Ailments()
            {
                Name = "Accusight",
                StartMessage = "Aim is True",
                DamagePerStack = 0,
                AilmentCurrentDuration = 999,
                AilmentMaxDuration = 999,

                BuffID = BuffID.accusight,
                SourceBuff = true,


            }

        },//Accusight
        {
            AilmentBuffID.papercut,
            new Ailments()
            {
                Name = "PaperCut",
                StartMessage = "Quick Cutting",
                DamagePerStack = 0,
                AilmentCurrentDuration = 999,
                AilmentMaxDuration = 999,

                BuffID = BuffID.papercutBuff,
                SourceBuff = true,


            }

        },//PaperCut's Buff
        {//Clang
            AilmentBuffID.clang,
            new Ailments()
            {
                Name = "CLANG",
                StartMessage = "CLANG STACKS INCREASED",
                DamagePerStack = 5,
                AilmentCurrentDuration = 2,
                AilmentMaxDuration = 2,

                BuffID = BuffID.clang,
                SourceBuff = true


            }

        },//CLANG
        {//Fia's Portal Buff
            AilmentBuffID.fiaportal,
            new Ailments()
            {
                Name = "Fia's Portal Buff",
                StartMessage = "Fia has sifted through the Dimensions",
                DamagePerStack = 10,
                AilmentCurrentDuration = 2,
                AilmentMaxDuration = 2,

                BuffID = BuffID.fiaPortalBuff,
                SourceBuff = true


            }

        },//Fia's Portal Buff
        {//Rust
            AilmentBuffID.rust,
            new Ailments()
            {
                Name = "Rust",
                StartMessage = "Rust Away Armor!",
                DamagePerStack = 5,
                AilmentCurrentDuration = 2,
                AilmentMaxDuration = 2,

                BuffID = BuffID.none,
                SourceBuff = true


            }

        },//Rust
        {//Sharpen Buff
            AilmentBuffID.sharpen,
            new Ailments()
            {
                Name = "Sharpen",
                StartMessage = "Your Blade Shines ",
                DamagePerStack = 10,
                AilmentCurrentDuration = 999,
                AilmentMaxDuration = 999,

                BuffID = BuffID.sharpenBuff,
                SourceBuff = true 


            }

        },//Sharpen

        



    };
    
}

public enum AilmentBuffID 
{ 
    clang, tox, bleed, rust, armr, accusight, papercut, fiaportal, torch, sharpen
}

public enum BuffID 
{ 
    none, clang, accusight, papercutBuff, fiaPortalBuff,  sharpenBuff
}
