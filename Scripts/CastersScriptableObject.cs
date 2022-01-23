using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Caster", menuName = "Caster/Create new Caster")]
public class CastersScriptableObject : ScriptableObject
{
    [SerializeField] string casterName;
    [SerializeField] int casterNumber;
    [SerializeField] Sprite casterSprite;
    [SerializeField] RuntimeAnimatorController overworldAnimator;
    [SerializeField] RuntimeAnimatorController casterAnimator;
    [SerializeField] GameObject casterPassive;
    [SerializeField] Sprite passiveSpriteSymbol;
    [TextArea(10,10)]
    [SerializeField] string passiveDescription;

    [SerializeField] CasterRegion theCastersRegion;
    [TextArea(20, 20)]
    [SerializeField] string casterLore;


    public string CasterName
    { get { return casterName; } }

    public int CasterNumber 
    { get { return casterNumber; } }

    public Sprite CasterSprite 
    { get { return casterSprite; } }

    public RuntimeAnimatorController OverworldAnimator 
    { get { return overworldAnimator; } }

    public RuntimeAnimatorController CasterAnimator 
    { get { return casterAnimator; } }

    public Sprite PassiveSpriteSymbol 
    { get { return passiveSpriteSymbol; } }

    public string PassiveDescription 
    { get { return passiveDescription; } }

    public CasterRegion TheCastersRegion 
    { get { return theCastersRegion; } }
    public string CasterLore 
    { get { return casterLore; } }
    
    public GameObject CasterPassive { get { return casterPassive; } }
}
public enum CasterRegion 
{ 
    None,
    Sun,
    Earth, 
    Moon
}
