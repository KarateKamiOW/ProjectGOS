using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeenTweenSystem : MonoBehaviour
{
    public Image burstSelectedSpellSquare;
    public void FadePointer() 
    { 
        
    }

    public void GameObjectScaleToNothing(GameObject theGameObject, float overWhatTime)
    {
        LeanTween.scale(theGameObject, new Vector3(0, 0, 0), overWhatTime);
    }

    public void GameObjectScaleToNormalSize(GameObject theGameObject, float overWhatTime) 
    {
        LeanTween.scale(theGameObject, new Vector3(1, 1, 1), overWhatTime);
    }

    public void TimerScaleDownAndBackUp(GameObject theGameObject, float overWhatTime) 
    {
        LeanTween.cancel(theGameObject);

        theGameObject.transform.localScale = Vector3.one;

        LeanTween.scale(theGameObject, Vector3.one * 2, overWhatTime).setEasePunch();
    }

    public void TimerTextScaleDownAndBackUp(TextMeshProUGUI text, float overWhatTime) 
    { 
        //LeanTween.cancel(text)
    }

    public void RotateY(GameObject theGameObject, float overWhatTime)
    {
        LeanTween.cancel(theGameObject);
        theGameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(theGameObject, Vector3.one, overWhatTime)
            .setEaseOutElastic()
            .setDelay(.2f);
    }

    public void SpellSelectedHighlight(GameObject theGameObject, float overWhatTime) 
    {
        LeanTween.cancel(theGameObject);
        theGameObject.transform.localScale = Vector3.zero;
        LeanTween.cancel(burstSelectedSpellSquare.gameObject);
        burstSelectedSpellSquare.transform.localScale = Vector3.zero;

        LeanTween.scale(burstSelectedSpellSquare.gameObject, Vector3.one, overWhatTime);
        //LeanTween.value

        LeanTween.scale(theGameObject, Vector3.one, overWhatTime)
            .setEaseOutElastic()
            .setDelay(.1f);
    }
}
