using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Caster", menuName = "Caster/Create new Summoned Unit")]
public class SummonedUnit : ScriptableObject
{
    [SerializeField] string unitName;
    [SerializeField] Sprite unitSprite;
    [SerializeField] RuntimeAnimatorController unitAnimator;
    [SerializeField] int maxHP;
    [SerializeField] GameObject summonedUnitsGOAAbility;//Game Object With Abstract Class Here

    public string UnitName
    { get { return unitName; } }

    public Sprite UnitSprite 
    { get { return unitSprite; } }

    public RuntimeAnimatorController UnitAnimator 
    { get { return unitAnimator; } }

    public int MaxpHP 
    { get { return maxHP; } }

    public GameObject SummonedUnitsGOAAbility 
    { get { return summonedUnitsGOAAbility; } }
}
