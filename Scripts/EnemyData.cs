using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData 
{
    public EnemyObject EnemyBase { get; set; }

    public EnemyData(EnemyObject enemyBaseInfo)
    {
            EnemyBase = enemyBaseInfo;
    }
    
}
