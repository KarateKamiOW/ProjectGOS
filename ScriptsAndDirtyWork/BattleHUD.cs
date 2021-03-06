using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userNameText;
    [SerializeField] TextMeshProUGUI casterNameText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Animator casterAnim;
    [SerializeField] SpriteRenderer swBBIcon;
    [SerializeField] BlockBar blockbar;
    [SerializeField] AilmentIconDB ailmentIconDB;

    [SerializeField] List<AilmentIconData> ailmentIconsData;  // Put these on null sprite renderers. It will fill in based on # of ailments

    [SerializeField] SpriteRenderer unitSpriteRenderer;

    Material matWhite;
    Material matDefault;
    Color colorTempToChangeOpacity; 

    public Animator CasterAnim 
    { get { return casterAnim; } }

    public HPBar HPBar 
    { get { return hpBar; } }

    public BlockBar PlayerBlockBar 
    { get { return blockbar; } }

    public SpriteRenderer UnitSpriteRenderer => unitSpriteRenderer;

    public void SetData(Caster theCaster, float casterHealth) 
    {
        casterNameText.text = theCaster.CasterBase.CasterName;
        casterAnim.runtimeAnimatorController = theCaster.CasterBase.CasterAnimator;
        hpBar.SetHealth(casterHealth);
        blockbar.SetBlock(0);

        swBBIcon.sprite = null;

        colorTempToChangeOpacity = swBBIcon.GetComponent<SpriteRenderer>().color;

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = unitSpriteRenderer.material;
    }

    public void SetSoloPlayerData(Caster theCaster, float currHealth, float maxHealth) 
    {
        string playerName;
        if (PlayerPrefs.HasKey("PlayerUsername"))
            playerName = PlayerPrefs.GetString("PlayerUsername");
        else
            playerName = "";

        userNameText.text = playerName;
        casterNameText.text = theCaster.CasterBase.CasterName;
        casterAnim.runtimeAnimatorController = theCaster.CasterBase.CasterAnimator;
        hpBar.SetNewHealth(currHealth, maxHealth);
        blockbar.SetBlock(0);

        swBBIcon.sprite = null;

        colorTempToChangeOpacity = swBBIcon.GetComponent<SpriteRenderer>().color;

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = unitSpriteRenderer.material;
    }

    public void SetSoloEnemyData(EnemyCaster theEnemyCaster, float enemyCasterHealth) 
    {
        userNameText.text = "";
        casterNameText.text = theEnemyCaster.EnemyCasterBase.name;
        casterAnim.runtimeAnimatorController = theEnemyCaster.EnemyCasterBase.EnemyAnimator;
        hpBar.SetHealth(enemyCasterHealth);
        blockbar.SetBlock(0);

        swBBIcon.sprite = null;

        colorTempToChangeOpacity = swBBIcon.GetComponent<SpriteRenderer>().color;

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = unitSpriteRenderer.material;
    }

    public void TakeHitFlashWhite() 
    {
        unitSpriteRenderer.material = matWhite;
        Invoke("ResetMaterial", .1f);
    }
    void ResetMaterial() 
    {
        unitSpriteRenderer.material = matDefault;
    }

    public IEnumerator UpdateHP(float newHp) 
    {
        yield return hpBar.SetHPSmoothened(newHp);
    }

    public IEnumerator UpdateArmor(float newArmor) 
    {
        yield return hpBar.SetArmorSmoothened(newArmor);
    }

    #region Icon Setting
    public void SetIconImage(bool onOrOff,Sprite imageToSet) 
    {
        
        if (onOrOff)
        {
            colorTempToChangeOpacity.a = 1;
            swBBIcon.color = colorTempToChangeOpacity;
            swBBIcon.sprite = imageToSet;
        }
        else 
        {
            colorTempToChangeOpacity.a = 0;
            swBBIcon.color = colorTempToChangeOpacity;
            swBBIcon.sprite = null;
        }
        
    }

    public void SetBlockIconImage(int onCDorNo) 
    {
        blockbar.SetBlock(onCDorNo);
    }

    public void SetAilmentorBuffIconImage(AilmentBuffID ailmentID, int position, bool isBuff) 
    {
        ailmentIconsData[position].IconSprite.sprite = ailmentIconDB.SetAilmentIcon(ailmentID, isBuff);
    }

    public void SetAilmentOrBuffStacksAndDuration(int position, int numOfStacks, int duration) 
    {
        ailmentIconsData[position].stacksText.text = numOfStacks.ToString();

        if (duration > 20)
            ailmentIconsData[position].durationText.text = "\u221E";
        else
            ailmentIconsData[position].durationText.text = duration.ToString();
    }

    public void ClearAilmentorBuffIconImageAndData(int position) 
    {
        ailmentIconsData[position].IconSprite = null;
        ailmentIconsData[position].stacksText.text = "";
        ailmentIconsData[position].durationText.text = "";
    }
    public void ClearALLAilmentorBuffIconImageAndData(int maxListNum)
    {
        for (int i = 0; i < maxListNum; i++)
        {
            ailmentIconsData[i].IconSprite.sprite = null;
            ailmentIconsData[i].stacksText.text = "";
            ailmentIconsData[i].durationText.text = "";
        }
    }

    public void IconOrderUpdate() 
    { 
        //Here will be the loop that will check if the icons need to be pushed 
    }
    #endregion

    public IEnumerator DeathFade() 
    {
        for (float f = 1f; f >= -0.05f; f -= 0.20f) 
        {
            Color c = unitSpriteRenderer.material.color;
            c.a = f;
            unitSpriteRenderer.material.color = c;
            yield return new WaitForSeconds(.05f);
        }
    }
}

[System.Serializable]
public class AilmentIconData 
{
    public SpriteRenderer IconSprite;
    public TextMeshProUGUI stacksText;
    public TextMeshProUGUI durationText;



}
