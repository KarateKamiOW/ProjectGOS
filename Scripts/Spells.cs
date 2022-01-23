using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells 
{
    public SpellsScriptableObject Base { get; set; }

    public Spells(SpellsScriptableObject spellsBase) 
    {
        Base = spellsBase;
    }
}
