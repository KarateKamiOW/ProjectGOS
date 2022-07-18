using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystemMenus : MonoBehaviour
{
    [SerializeField] BattleSystemMenusViewFX battleSystemMenuFX;
    [SerializeField] CameraMenuSettings cameraMenuSettings;
    public GameObject dialogBoxMenu;
    public GameObject bookSelectMenu;
    public GameObject spellSelectMenu;
    public RectTransform rohkanSpell1;
    public RectTransform rohkanSpell2;
    public RectTransform paperiousSpell1;
    public RectTransform paperiousSpell2;
    public RectTransform scissoraSpell1;
    public RectTransform scissoraSpell2;
    public RectTransform blockSpell;
    public GameObject waitingScreen;
    public PostMatchScreen postMatchScreen;
    public GameObject postGameScreenGO;

    public BattleSystemMenusViewFX BattleSysMenusView => battleSystemMenuFX;
    public CameraMenuSettings CameraMenuSettings => cameraMenuSettings;

    void Start()
    {
        //cam = GetComponentInParent<Camera>();   
        cameraMenuSettings.InitialZoomSize = cameraMenuSettings.BattleCam.orthographicSize;
        CameraMenuSettings.initialTransformPos = cameraMenuSettings.BattleCam.transform;
    }
    void LateUpdate()
    {
        if (cameraMenuSettings.ZoomActive)
        {
            cameraMenuSettings.BattleCam.orthographicSize = Mathf.Lerp(cameraMenuSettings.BattleCam.orthographicSize, cameraMenuSettings.zoomTo, (cameraMenuSettings.ZoomSpeed * .1f));
        }
        else 
        { 
            if(cameraMenuSettings.BattleCam.orthographicSize != cameraMenuSettings.InitialZoomSize)
                cameraMenuSettings.BattleCam.orthographicSize = Mathf.Lerp(cameraMenuSettings.BattleCam.orthographicSize, cameraMenuSettings.InitialZoomSize, (cameraMenuSettings.ZoomSpeed * .1f));
        }

        if (cameraMenuSettings.DeathZoomActive)
        {
            if (cameraMenuSettings.IsPlayerOne)
            {
                //battleCam.orthographicSize = Mathf.Lerp(battleCam.orthographicSize, 3, .7f);
                cameraMenuSettings.BattleCam.transform.position = Vector3.Lerp(cameraMenuSettings.BattleCam.transform.position, cameraMenuSettings.Player1.transform.position, .7f);
            }
            else
            {
                cameraMenuSettings.BattleCam.transform.position = Vector3.Lerp(cameraMenuSettings.BattleCam.transform.position, cameraMenuSettings.Player2.transform.position, .7f);
            }
        }
        else 
        {
            if (cameraMenuSettings.BattleCam.orthographicSize != cameraMenuSettings.InitialZoomSize)
                cameraMenuSettings.BattleCam.orthographicSize = Mathf.Lerp(cameraMenuSettings.BattleCam.orthographicSize, cameraMenuSettings.InitialZoomSize, (cameraMenuSettings.ZoomSpeed * .1f));
            //cameraMenuSettings.BattleCam.transform.position = Vector3.Lerp(cameraMenuSettings.BattleCam.transform.position, Vector3.zero, .7f);
        }
    }

    public void OpenBookSelect(bool openClose) 
    {
        if (openClose)
        {
            LeanTween.cancel(bookSelectMenu);
            LeanTween.scale(bookSelectMenu, Vector3.one, .15f).setEase(LeanTweenType.easeInBack);
        }
        else 
        {
            LeanTween.cancel(bookSelectMenu);
            LeanTween.scale(bookSelectMenu, Vector3.zero, .15f).setEase(LeanTweenType.easeInBack);
        }
    }
            
    public void OpenCloseRohkanSpells(bool openClose) 
    {
        if (openClose)
        {
            spellSelectMenu.SetActive(true);
            LeanTween.cancel(rohkanSpell1);
            LeanTween.cancel(rohkanSpell2);

            LeanTween.move(rohkanSpell1, new Vector3(389.2f, -47.2f, 0), .15f); 
            LeanTween.move(rohkanSpell2, new Vector3(399.35f, -47.2f, 0), .15f);

            LeanTween.scale(rohkanSpell1.gameObject, Vector3.one, .35f).setEaseOutElastic();//.setDelay(.10f);
            LeanTween.scale(rohkanSpell2.gameObject, Vector3.one, .35f).setEaseOutElastic();//.setDelay(.10f); ;

            Image rohSpell1 = rohkanSpell1.GetComponent<Image>();
            Image rohSpell2 = rohkanSpell2.GetComponent<Image>();

            StartCoroutine(FadeImage(false, rohSpell1));
            StartCoroutine(FadeImage(false, rohSpell2));
        }
        else 
        {
            //spellSelectMenu.SetActive(false);
            LeanTween.cancel(rohkanSpell1);
            LeanTween.cancel(rohkanSpell2);


            LeanTween.move(rohkanSpell1, new Vector3(380f, -46.7f, 0), .3f);//.setEaseInCirc();
            LeanTween.move(rohkanSpell2, new Vector3(410f, -46.7f, 0), .3f);//.setEaseInCirc();

            LeanTween.scale(rohkanSpell1.gameObject, Vector3.zero, .35f).setEaseOutElastic();//.setEaseInCirc().setDelay(.25f); ;
            LeanTween.scale(rohkanSpell2.gameObject, Vector3.zero, .35f).setEaseOutElastic().setOnComplete(() => 
            {
                spellSelectMenu.SetActive(false);
            });//.setEaseInCirc().setDelay(.25f); 

            Image rohSpell1 = rohkanSpell1.GetComponent<Image>();
            Image rohSpell2 = rohkanSpell2.GetComponent<Image>();

            StartCoroutine(FadeImage(true, rohSpell1));
            StartCoroutine(FadeImage(true, rohSpell2));
        }
    }
    public void OpenClosePaperiousSpells(bool openClose) 
    {
        if (openClose)
        {
            spellSelectMenu.SetActive(true);
            LeanTween.cancel(paperiousSpell1);
            LeanTween.cancel(paperiousSpell2);


            LeanTween.move(paperiousSpell1, new Vector3(389.2f, -47.2f, 0), .15f);
            LeanTween.move(paperiousSpell2, new Vector3(399.35f, -47.2f, 0), .15f);

            LeanTween.scale(paperiousSpell1.gameObject, Vector3.one, .35f).setEaseOutElastic(); 
            LeanTween.scale(paperiousSpell2.gameObject, Vector3.one, .35f).setEaseOutElastic(); 

            Image paperSpell1 = paperiousSpell1.GetComponent<Image>();
            Image paperSpell2 = paperiousSpell2.GetComponent<Image>();

            StartCoroutine(FadeImage(false, paperSpell1));
            StartCoroutine(FadeImage(false, paperSpell2));
        }
        else
        {
            LeanTween.cancel(paperiousSpell1);
            LeanTween.cancel(paperiousSpell2);

            spellSelectMenu.SetActive(false);

            LeanTween.move(paperiousSpell1, new Vector3(380f, -46.7f, 0), .3f);
            LeanTween.move(paperiousSpell2, new Vector3(410f, -46.7f, 0), .3f);

            LeanTween.scale(paperiousSpell1.gameObject, Vector3.zero, .1f);
            LeanTween.scale(paperiousSpell2.gameObject, Vector3.zero, .1f);

            Image paperSpell1 = paperiousSpell1.GetComponent<Image>();
            Image paperSpell2 = paperiousSpell2.GetComponent<Image>();

            StartCoroutine(FadeImage(true, paperSpell1));
            StartCoroutine(FadeImage(true, paperSpell2));
        }
    }
    public void OpenCloseScissoraSpells(bool openClose) 
    {
        if (openClose)
        {

            spellSelectMenu.SetActive(true);

            LeanTween.cancel(scissoraSpell1);
            LeanTween.cancel(scissoraSpell2);


            LeanTween.move(scissoraSpell1, new Vector3(389.2f, -47.2f, 0), .15f);
            LeanTween.move(scissoraSpell2, new Vector3(399.35f, -47.2f, 0), .15f);

            LeanTween.scale(scissoraSpell1.gameObject, Vector3.one, .35f).setEaseOutElastic(); 
            LeanTween.scale(scissoraSpell2.gameObject, Vector3.one, .35f).setEaseOutElastic(); 

            Image scissorSpell1 = scissoraSpell1.GetComponent<Image>();
            Image scissorSpell2 = scissoraSpell2.GetComponent<Image>();

            StartCoroutine(FadeImage(false, scissorSpell1));
            StartCoroutine(FadeImage(false, scissorSpell2));
        }
        else
        {
            spellSelectMenu.SetActive(false);

            LeanTween.cancel(scissoraSpell1);
            LeanTween.cancel(scissoraSpell2);

            LeanTween.move(scissoraSpell1, new Vector3(380f, -46.7f, 0), .3f);
            LeanTween.move(scissoraSpell2, new Vector3(410f, -46.7f, 0), .3f);

            LeanTween.scale(scissoraSpell1.gameObject, Vector3.zero, .1f);
            LeanTween.scale(scissoraSpell2.gameObject, Vector3.zero, .1f);

            Image scissorSpell1 = scissoraSpell1.GetComponent<Image>();
            Image scissorSpell2 = scissoraSpell2.GetComponent<Image>();

            StartCoroutine(FadeImage(true, scissorSpell1));
            StartCoroutine(FadeImage(true, scissorSpell2));
        }
    }
    public void OpenCloseBlockSpell(bool openClose)
    {
        if (openClose)
        {

            spellSelectMenu.SetActive(true);

            LeanTween.cancel(blockSpell);
            


            LeanTween.move(blockSpell, new Vector3(394.12f, -47.2f, 0), .15f);

            LeanTween.scale(blockSpell.gameObject, Vector3.one, .35f).setEaseOutElastic(); 
      

            Image blockSpell1 = blockSpell.GetComponent<Image>();

            StartCoroutine(FadeImage(false, blockSpell1));
        }
        else
        {
            spellSelectMenu.SetActive(false);

            LeanTween.cancel(blockSpell);

            LeanTween.move(blockSpell, new Vector3(380f, -46.63139f, 0), .3f); 
            LeanTween.scale(blockSpell.gameObject, Vector3.zero, .1f);

            Image blockSpell1 = blockSpell.GetComponent<Image>();
            

            StartCoroutine(FadeImage(true, blockSpell1));
            
        }
    }

    public void SpellSelectAnimation(int spellPos) 
    {
        switch (spellPos) 
        { 
            case 0:
                LeanTween.scale(rohkanSpell1, new Vector3(1.1f,1.1f,1), .2f).setEaseOutElastic().setOnComplete(() => 
                {
                    OpenCloseRohkanSpells(false);
                    OpenClosePaperiousSpells(false);
                    OpenCloseScissoraSpells(false);
                    OpenCloseBlockSpell(false);
                    cameraMenuSettings.ZoomActive = true;
                });
                break;
            case 1:
                LeanTween.scale(rohkanSpell2, new Vector3(1.1f, 1.1f, 1), .2f).setEaseOutElastic().setOnComplete(() =>
                {
                    OpenCloseRohkanSpells(false);
                    OpenClosePaperiousSpells(false);
                    OpenCloseScissoraSpells(false);
                    OpenCloseBlockSpell(false);
                    cameraMenuSettings.ZoomActive = true;
                });
                break;
            case 2:
                LeanTween.scale(paperiousSpell1, new Vector3(1.1f, 1.1f, 1), .2f).setEaseOutElastic().setOnComplete(() =>
                {
                    OpenCloseRohkanSpells(false);
                    OpenClosePaperiousSpells(false);
                    OpenCloseScissoraSpells(false);
                    OpenCloseBlockSpell(false);
                    cameraMenuSettings.ZoomActive = true;
                });
                break;
            case 3:
                LeanTween.scale(paperiousSpell2, new Vector3(1.1f, 1.1f, 1), .2f).setEaseOutElastic().setOnComplete(() =>
                {
                    OpenCloseRohkanSpells(false);
                    OpenClosePaperiousSpells(false);
                    OpenCloseScissoraSpells(false);
                    OpenCloseBlockSpell(false);
                    cameraMenuSettings.ZoomActive = true;
                });
                break;
            case 4:
                LeanTween.scale(scissoraSpell1, new Vector3(1.1f, 1.1f, 1), .2f).setEaseOutElastic().setOnComplete(() =>
                {
                    OpenCloseRohkanSpells(false);
                    OpenClosePaperiousSpells(false);
                    OpenCloseScissoraSpells(false);
                    OpenCloseBlockSpell(false);
                    cameraMenuSettings.ZoomActive = true;
                });
                break;
            case 5:
                LeanTween.scale(scissoraSpell2, new Vector3(1.1f, 1.1f, 1), .2f).setEaseOutElastic().setOnComplete(() =>
                {
                    OpenCloseRohkanSpells(false);
                    OpenClosePaperiousSpells(false);
                    OpenCloseScissoraSpells(false);
                    OpenCloseBlockSpell(false);
                    cameraMenuSettings.ZoomActive = true;
                });
                break;
            case 6:
                LeanTween.scale(blockSpell, new Vector3(1.1f, 1.1f, 1), .2f).setEaseOutElastic().setOnComplete(() =>
                {
                    OpenCloseRohkanSpells(false);
                    OpenClosePaperiousSpells(false);
                    OpenCloseScissoraSpells(false);
                    OpenCloseBlockSpell(false);
                    cameraMenuSettings.ZoomActive = true;
                });
                break;
        }
    }

    public void DeathZoom(bool isPlayerOne) 
    {
        cameraMenuSettings.IsPlayerOne = isPlayerOne;
        cameraMenuSettings.DeathZoomActive = true;
        cameraMenuSettings.zoomTo = 3f;
        cameraMenuSettings.ZoomActive = true;

        
    }

    public void OpenSoloPostMatchScreen(bool playerWon) 
    {
        postMatchScreen.SwapBGWinOrLose(playerWon);
        postMatchScreen.GeneratePostMatchStats();
        LeanTween.scale(postGameScreenGO, Vector3.one, .25f);

        /*if (playerWon)
        {
            postMatchScreen.GeneratePostMatchStats();
            LeanTween.scale(postGameScreenGO, Vector3.one, .25f);
            postMatchScreen.SwapBGWinOrLose(playerWon);

        }
        else 
        {
            postMatchScreen.GeneratePostMatchStats();
            LeanTween.scale(postGameScreenGO, Vector3.one, .25f);
            postMatchScreen.SwapBGWinOrLose(playerWon);
        }*/
        
    }

    IEnumerator FadeImage(bool fadeAway, Image img)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1f; i >= 0; i -= Time.deltaTime * 5f)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1f; i += Time.deltaTime * 5f)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }



}
[System.Serializable]
public class BattleSystemMenusViewFX 
{
    public Image wideScreenTopImg;
    public Image wideScreenBottomImg;

    public void OpenCloseScenicWideView(bool onOrOff) 
    {
        LeanTween.cancel(wideScreenTopImg.gameObject);
        LeanTween.cancel(wideScreenBottomImg.gameObject);
        if (onOrOff)
        {
            //LeanTween.move(wideScreenTopImg.gameObject, new Vector3(1.47f, 263.74f, 0), .3f);
            //LeanTween.move(wideScreenBottomImg.gameObject, new Vector3(1.47f, -254.86f, 0), .3f)

            LeanTween.scale(wideScreenTopImg.gameObject, Vector3.one, .3f).setEaseOutCirc();
            LeanTween.scale(wideScreenBottomImg.gameObject, Vector3.one, .3f).setEaseOutCirc();
        }
        else 
        {
            //LeanTween.move(wideScreenTopImg.gameObject, new Vector3(1.47f, 300f, 0), .3f);
            //LeanTween.move(wideScreenBottomImg.gameObject, new Vector3(1.47f, -20f, 0), .3f);

            LeanTween.scale(wideScreenTopImg.gameObject, Vector3.zero, .3f).setEaseOutElastic();
            LeanTween.scale(wideScreenBottomImg.gameObject, Vector3.zero, .3f).setEaseOutElastic();
        }
    }
}
[System.Serializable]
public class CameraMenuSettings
{
    [SerializeField] BattleUnit P1;
    [SerializeField] BattleUnit P2;
    [SerializeField] Camera battleCam;
    public float zoomTo;
    [SerializeField] float zoomSpeed;
    [SerializeField] List<Transform> actionPoints;
    public float InitialZoomSize { get; set; }
    public bool ZoomActive { get; set; } = false;
    public bool DeathZoomActive { get; set; } = false;

    public Transform initialTransformPos { get; set; }

    public BattleUnit Player1 => P1;
    public BattleUnit Player2 => P2;
    public bool IsPlayerOne { get; set; } = false;

    public Camera BattleCam => battleCam;
    public float ZoomSpeed => zoomSpeed;


}
