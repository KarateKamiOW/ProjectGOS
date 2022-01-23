using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caster 
{
    public CastersScriptableObject CasterBase { get; set; }

    public Caster(CastersScriptableObject CasterBaseInfo)
    {
        CasterBase = CasterBaseInfo;
    }
}
