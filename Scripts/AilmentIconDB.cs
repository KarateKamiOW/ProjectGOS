using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AilmentIconDB : MonoBehaviour
{
    public List<Sprite> ailmentIconSprites;

    public Sprite SetAilmentIcon(AilmentBuffID ailment, bool isBuff ) 
    {
        if (ailment == AilmentBuffID.tox)
            return ailmentIconSprites[0];
        else if (ailment == AilmentBuffID.bleed)
            return ailmentIconSprites[1];
        else if (ailment == AilmentBuffID.rust)
            return ailmentIconSprites[2];
        else if (ailment == AilmentBuffID.clang)
            return ailmentIconSprites[5];
        else
        {
            if (isBuff)
                return ailmentIconSprites[3];
            else
                return ailmentIconSprites[4];
        }
    }
}
