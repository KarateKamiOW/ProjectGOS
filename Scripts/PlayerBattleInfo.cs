using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleInfo : MonoBehaviour
{
    public CastersScriptableObject playersCharacter;
 

    public List<Spells> RohkanSpells { get; set; }
    public List<Spells> PaperiousSpells { get; set; }

    public List<Spells> ScissoraSpells { get; set; }
}

public class RockSpells 
{
    public SpellsScriptableObject spell;

    public SpellsScriptableObject Spell 
    { get { return spell; } }
}

public class PaperSpells
{
    public SpellsScriptableObject spell;

    public SpellsScriptableObject Spell
    { get { return spell; } }
}
public class ScissorSpells
{
    public SpellsScriptableObject spell;

    public SpellsScriptableObject Spell
    { get { return spell; } }
}
