using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueDialogBehavior : MonoBehaviour
{
    public List<DialogObject> additionalDialogObjs = new List<DialogObject>();

    protected private int DialogPos = 0;

    public abstract DialogObject ShowUniqueDialog();
    
    
}
