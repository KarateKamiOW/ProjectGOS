using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ResponseOptions {None, One, Two, Three }
public class ResponseHandler : MonoBehaviour
{
    [SerializeField] GameObject singleOptionRespBox;
    [SerializeField] GameObject twoOptionRespBox;
    [SerializeField] GameObject tripleOptionRespBox;
    [SerializeField] RespBoxDetails singleOptionDetails;
    [SerializeField] RespBoxDetails[] twoOptionDetails;
    [SerializeField] RespBoxDetails[] threeOptionDetails;

    ResponseOptions responseOps = ResponseOptions.None;
    int currentSelectedResponseNum = 0;
    int responseIndex;
    DialogUI dialogUI;
    List<OverworldDialogResponse> OWResponses = new List<OverworldDialogResponse>();
    DialogEvent[] responseEvents;

    public void Start()
    {
        dialogUI = GetComponentInParent<DialogUI>();
    }

    public void Update()
    {
        if (responseOps == ResponseOptions.One)
        {
            HandleUpdateSingleResponse();
        }
        else if (responseOps == ResponseOptions.Two)
        {
            HandleUpdateTwoReponses();
        }
        else if (responseOps == ResponseOptions.Three) 
        {
            HandleUpdateThreeResponses();
        }
    }

    public void ShowResponses(OverworldDialogResponse[] responses) 
    {
        if (responses.Length == 1)
        {
            singleOptionDetails.shortRespText.text = responses[0].ResponseText;

            responseOps = ResponseOptions.One;
            singleOptionRespBox.SetActive(true);
            twoOptionRespBox.SetActive(false);
            tripleOptionRespBox.SetActive(false);

            OWResponses.Add(responses[0]);
        }
        else if (responses.Length == 2)
        {
            twoOptionDetails[0].shortRespText.text = responses[0].ResponseText;
            twoOptionDetails[1].shortRespText.text = responses[1].ResponseText;

            responseOps = ResponseOptions.Two;

            singleOptionRespBox.SetActive(false);
            twoOptionRespBox.SetActive(true);
            tripleOptionRespBox.SetActive(false);

            OWResponses.Add(responses[0]);
            OWResponses.Add(responses[1]);
        }
        else if (responses.Length == 3)
        {
            threeOptionDetails[0].shortRespText.text = responses[0].ResponseText;
            threeOptionDetails[1].shortRespText.text = responses[1].ResponseText;
            threeOptionDetails[2].shortRespText.text = responses[2].ResponseText;

            responseOps = ResponseOptions.Three;

            singleOptionRespBox.SetActive(false);
            twoOptionRespBox.SetActive(false);
            tripleOptionRespBox.SetActive(true);

            OWResponses.Add(responses[0]);
            OWResponses.Add(responses[1]);
            OWResponses.Add(responses[2]);

        }
        else 
        {
            Debug.Log(responses.Length + " Num of Responses");
            return;
        }
    }

    void HandleUpdateSingleResponse() 
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            singleOptionRespBox.SetActive(false);
            //dialogUI.ShowDialog(OWResponses[currentSelectedResponseNum].DialogObject);
            responseOps = ResponseOptions.None;

            if (responseEvents != null && responseIndex <= responseEvents.Length)
            {
                responseEvents[currentSelectedResponseNum].OnPickedResponse?.Invoke();
            }

            responseEvents = null;  //

            if (OWResponses[currentSelectedResponseNum].DialogObject)
            {
                dialogUI.ShowDialog(OWResponses[currentSelectedResponseNum].DialogObject);
            }
            else
            {
                dialogUI.CloseDialogBox();
            }
        }
    }
    void HandleUpdateTwoReponses() 
    {
        //Mathf.Clamp(currentSelectedResponseNum, 0, 1);
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSelectedResponseNum++;

            if (currentSelectedResponseNum > 1)
                currentSelectedResponseNum = 0;

            HandleTwoOptionSelectors(currentSelectedResponseNum);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            currentSelectedResponseNum--;

            if (currentSelectedResponseNum < 0)
                currentSelectedResponseNum = 1;

            HandleTwoOptionSelectors(currentSelectedResponseNum);

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            twoOptionRespBox.SetActive(false);
            dialogUI.ShowDialog(OWResponses[currentSelectedResponseNum].DialogObject);
            responseOps = ResponseOptions.None;

            if (responseEvents != null && currentSelectedResponseNum <= responseEvents.Length)
            {
                responseEvents[currentSelectedResponseNum].OnPickedResponse?.Invoke();
            }
            //responseEvents = null;  //

            /*if (OWResponses[currentSelectedResponseNum].DialogObject )
            {
                dialogUI.ShowDialog(OWResponses[currentSelectedResponseNum].DialogObject);
            }
            else 
            {
                dialogUI.CloseDialogBox();
            }
            responseEvents = null;*/
        }
    }
    void HandleUpdateThreeResponses() 
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentSelectedResponseNum++;

            if (currentSelectedResponseNum > 2)
                currentSelectedResponseNum = 0;

            HandleThreeOptionSelectors(currentSelectedResponseNum);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentSelectedResponseNum--;

            if (currentSelectedResponseNum < 0)
                currentSelectedResponseNum = 2;

            HandleThreeOptionSelectors(currentSelectedResponseNum);

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            tripleOptionRespBox.SetActive(false);
            //dialogUI.ShowDialog(OWResponses[currentSelectedResponseNum].DialogObject);
            responseOps = ResponseOptions.None;

            if (responseEvents != null && responseIndex <= responseEvents.Length) 
            {
                responseEvents[currentSelectedResponseNum].OnPickedResponse?.Invoke();
            }

            responseEvents = null;  //

            if (OWResponses[currentSelectedResponseNum].DialogObject)
            {
                dialogUI.ShowDialog(OWResponses[currentSelectedResponseNum].DialogObject);
            }
            else
            {
                dialogUI.CloseDialogBox();
            }
        }
    }

    public void HandleTwoOptionSelectors(int selectorNum)
    {
        for (int i = 0; i < twoOptionDetails.Length; i++)
        {
            if (i == selectorNum)
            {
                LeanTween.scale(twoOptionDetails[i].respSelector.gameObject, Vector3.one, .2f);
            }
            else
            {
                LeanTween.scale(twoOptionDetails[i].respSelector.gameObject, Vector3.zero, .2f);
            }
        }
    }

    public void HandleThreeOptionSelectors(int selectorNum)
    {
        for (int i = 0; i < threeOptionDetails.Length; i++)
        {
            if (i == selectorNum)
            {
                LeanTween.scale(threeOptionDetails[i].respSelector.gameObject, Vector3.one, .2f);
            }
            else
            {
                LeanTween.scale(threeOptionDetails[i].respSelector.gameObject, Vector3.zero, .2f);
            }
        }
    }

    public void AddResponseEvents(DialogEvent[] responseEvents) 
    {

        this.responseEvents = responseEvents;
        //Debug.Log("Response Handler Worked.");
    }
}


[System.Serializable]
public class RespBoxDetails
{
    public TMP_Text shortRespText;
    public Image respSelector;
}
