using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Enemy", menuName = "CreateNewBattleEnemy")]
public class EnemyObject : ScriptableObject
{
    [SerializeField] string enemyName;
    [SerializeField] int enemyIDNumber;
    [SerializeField] int maxHP;
    [SerializeField] Sprite enemySprite;
    [SerializeField] RuntimeAnimatorController enemyAnimator;
    [SerializeField] GameObject enemyPassive;
    [SerializeField] Sprite passiveSpriteSymbol;
    [TextArea(10, 10)]
    [SerializeField] string passiveDescription;

    [SerializeField] CasterRegion theEnemiesRegion;
    [SerializeField] List<EnemyDrops> enemyDrops;
    [SerializeField] int minSolcs, maxSolcs;
    [SerializeField] GameObject enemySpellSet;
    [TextArea(20, 20)]
    [SerializeField] string enemyLore;
    public string EnemyName
    { get { return enemyName; } }

    public int EnemyIDNumber
    { get { return enemyIDNumber; } }

    public int MaxHP => maxHP;

    public Sprite EnemySprite
    { get { return enemySprite; } }

    public RuntimeAnimatorController EnemyAnimator
    { get { return enemyAnimator; } }

    public Sprite PassiveSpriteSymbol
    { get { return passiveSpriteSymbol; } }

    public string PassiveDescription
    { get { return passiveDescription; } }

    public CasterRegion TheEnemiesRegion
    { get { return theEnemiesRegion; } }

    public List<EnemyDrops> EnemyDrops => enemyDrops;

    public int MinSolcs => minSolcs;
    public int MaxSolcs => maxSolcs;

    public GameObject EnemySpellset => enemySpellSet;
    public string EnemyLore => enemyLore;

    public GameObject CasterPassive { get { return enemyPassive; } }
}

[System.Serializable]
public class EnemyDrops 
{
    public ItemDataObject itemDrop;
    public float dropPercentage;
}
