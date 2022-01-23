using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ailments 
{
    public AilmentBuffID ID { get; set; }
    public BuffID BuffID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }

    public int DamagePerStack { get; set; }

    public int AilmentStacks { get; set; }

    public int AilmentCurrentDuration { get; set; }

    public int AilmentMaxDuration { get; set; }

    public bool IsEmpowered { get; set; }
    public bool SourceBuff { get; set; }

}

public class AilmentData
{
    public AilmentBuffID ID { get; set; }
    public BuffID BuffID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }

    public int DamagePerStack { get; set; }

    public int AilmentStacks { get; set; }

    public int AilmentCurrentDuration { get; set; }

    public int AilmentMaxDuration { get; set; }

    public bool IsEmpowered { get; set; }
    public bool SourceBuff {get; set;}
    

    public AilmentData(Ailments baseAilment)
    {
        Name = baseAilment.Name;
        Description = baseAilment.Description;
        StartMessage = baseAilment.StartMessage;
        DamagePerStack = baseAilment.DamagePerStack;
        AilmentStacks = baseAilment.AilmentStacks;
        AilmentCurrentDuration = baseAilment.AilmentCurrentDuration;
        AilmentMaxDuration = baseAilment.AilmentMaxDuration;
        IsEmpowered = baseAilment.IsEmpowered;
        ID = baseAilment.ID;
        BuffID = baseAilment.BuffID;
        SourceBuff = baseAilment.SourceBuff;

    }

    public AilmentData(AilmentData baseAilment) 
    {
        Name = baseAilment.Name;
        Description = baseAilment.Description;
        StartMessage = baseAilment.StartMessage;
        DamagePerStack = baseAilment.DamagePerStack;
        AilmentStacks = baseAilment.AilmentStacks;
        AilmentCurrentDuration = baseAilment.AilmentCurrentDuration;
        AilmentMaxDuration = baseAilment.AilmentMaxDuration;
        IsEmpowered = baseAilment.IsEmpowered;
        ID = baseAilment.ID;
        BuffID = baseAilment.BuffID;
        SourceBuff = baseAilment.SourceBuff;
    }


}
