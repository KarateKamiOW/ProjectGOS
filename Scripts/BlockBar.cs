using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBar : MonoBehaviour
{
    public Image blockBar;

    public List<Sprite> blockbarSprites;

    public void SetBlock(int blockCD) 
    {
        if (blockCD == 0)   //Off CD, Block Bar is Filled
            blockBar.sprite = blockbarSprites[2];
        else if (blockCD == 1)  //1 More Round till CD is up, bar half full.
            blockBar.sprite = blockbarSprites[1];
        else
            blockBar.sprite = blockbarSprites[0];   //Block CD is > 2, its simply empty. Anything lower or higher than 2 is just empty
    }
}
