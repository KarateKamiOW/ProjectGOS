using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spells", menuName = "Spells/Create new Spell")]
public class SpellsScriptableObject : ScriptableObject
{
    [SerializeField] string spellName;
    [SerializeField] Sprite spellSprite;

    [TextArea(5,5)]
    [SerializeField] string spellDescription;
    [TextArea(5, 5)]
    [SerializeField] string spellBonusDescription;
    [SerializeField] SpellGodType godType;
    [SerializeField] SpellWinType winType;  //How Does this ability gain its bonus? SW or BB. Perhaps both.
    [SerializeField] int damage;
    [SerializeField] int winDamageBoost;     //Damage if you SW/BB
    [SerializeField] bool targetsAllEnemies;    //Check true if this ability hits every enemy
    [SerializeField] bool isMultiHit;   //Check true if this ability strikes multiple times
    [SerializeField] GameObject spellObject;
    [TextArea(5, 5)]
    [SerializeField] string spellLore;



    //Later
    //SpellAnimation/gameObject

    public string SpellName 
    { get { return spellName; } }

    public Sprite SpellSprite 
    { get { return spellSprite; } }

    public string SpellDescription 
    { get { return spellDescription;  } }

    public string SpellBonusDescription 
    { get { return spellBonusDescription; } }

    public SpellGodType GodType 
    { get { return godType; } }

    public SpellWinType WinType 
    { get { return winType; } }

    public int Damage 
    { get { return damage; } }

    public int WinDamageBoost 
    { get { return winDamageBoost; } }

    public bool TargetsAllEnemies 
    { get { return targetsAllEnemies; } }

    public bool IsMultiHit 
    { get { return isMultiHit; } }

    public GameObject SpellObject 
    { get { return spellObject; } }

    public string SpellLore 
    { get { return spellLore; } }

    
}
public enum SpellGodType 
{ 
    Rohkan,
    Paperious,
    Scissora,
    Block
}

public enum SpellWinType 
{ 
    None,
    SW,
    BB,
    Both
}
