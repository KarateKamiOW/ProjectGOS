using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject healthObject;
    [SerializeField] GameObject armorObject;
    

    public float currentHealth, maxHealth;  //MaxHP is currently set to 200
    public float currentArmor, maxArmor;    //Max Armor is currently set to 75



    public void SetHealth(float maximumHealth)
    {
        //Sets Health at the beginning of the battle
        //Further HP updates will happen through SetHPSmoothened()
        //Special HP Updates(For Monsters, or other specific battles), change max hp after this function
        currentHealth = maximumHealth;
        maxHealth = maximumHealth;
        //slider.maxValue = maximumHealth;
        //slider.value = health;

        //whiteHealthSlider.maxValue = maximumHealth;
        //whiteHealthSlider.value = health;

        float healthNormalized = ((float)currentHealth / maxHealth);

        healthObject.transform.localScale = new Vector3(healthNormalized, 1f);
        //Here I will also set the armor to 0 at battle launch. Looking to make exceptions? Start Here.
        armorObject.transform.localScale = new Vector3(0f, 1f);
    }

    public IEnumerator SetArmorSmoothened(float newArmor) 
    {
        if (newArmor > maxArmor)
            newArmor = maxArmor;

        float currArmr = armorObject.transform.localScale.x;
        float changeAMT = currArmr - newArmor;

        while (currArmr - newArmor > Mathf.Epsilon) 
        {
            currArmr -= changeAMT * (Time.deltaTime * (float)6.5);
            armorObject.transform.localScale = new Vector3(currArmr, 1f);
            yield return null;
        }
        armorObject.transform.localScale = new Vector3(newArmor, 1f);
        //currentArmor = newArmor;

    }

    public IEnumerator SetHPSmoothened(float newHP) 
    {
        if (newHP > maxHealth)
            newHP = maxHealth;
        //Sets hp smoother(bar raises or lowers over a period of time)

        float currentHP = healthObject.transform.localScale.x;  //currentHealth;
        float changeAMT = currentHP - newHP;

        while (currentHP - newHP > Mathf.Epsilon) 
        {
            currentHP -= changeAMT * (Time.deltaTime * (float)6.5);
            healthObject.transform.localScale = new Vector3(currentHP, 1f);
            //slider.value = currentHP;
            yield return null;
        }
        healthObject.transform.localScale = new Vector3(newHP, 1f);
        //currentHealth = newHP;
        //slider.value = newHP;

        //whiteHealthSlider.value = newHP;
       
    }

    public void ShimmerArmor() 
    { 
        //While the player/enemy has armor, their armor bar will visually shimmer on/off
        //Its opacity will deincrement to 0 before incrementing back to 1.
    }
}
