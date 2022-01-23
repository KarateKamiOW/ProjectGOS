using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonedUnitData 
{
    public SummonedUnit Base { get; set; }
    public int CurrentHealth { get; set; }

    public SummonedUnitData(SummonedUnit unitData) 
    {
        Base = unitData;
    }
}
