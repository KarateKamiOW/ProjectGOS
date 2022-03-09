using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogUI : MonoBehaviour
{

    [SerializeField] TMP_Text textLabel;

    [SerializeField] GameObject dialogBoxGO;
    OverworldDialogTyping ODT;
    ResponseHandler responseHandler;

    public bool IsOpen { get; private set; }

   

    // Start is called before the first frame update
    void Start()
    {
        ODT = GetComponent<OverworldDialogTyping>();
        responseHandler = GetComponentInChildren<ResponseHandler>();

        CloseDialogBox();
    }

    public void ShowDialog(DialogObject dialogObj) 
    {
        IsOpen = true;
        GameManager.instance.state = GameState.Dialog;
        PlayerController.instance.PauseMovement();
        OpenDialogBox();
        StartCoroutine(StepThroughDialog(dialogObj));
    }
    public void AddResponseEvents(DialogEvent[] responseEvent) 
    {
        responseHandler.AddResponseEvents(responseEvent);
        
    } 

    private IEnumerator StepThroughDialog(DialogObject dialogObj)
    {

        for (int i = 0; i < dialogObj.Dialogue.Length; i++)
        {
            string dialog = dialogObj.Dialogue[i];

            yield return ODT.Run(dialog, textLabel);

            if (i == dialogObj.Dialogue.Length - 1 && dialogObj.HasResponses)
                break;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (dialogObj.HasResponses)
        {
            responseHandler.gameObject.SetActive(true);
            responseHandler.ShowResponses(dialogObj.Responses);
        }
        else 
        {
            responseHandler.gameObject.SetActive(false);
            CloseDialogBox();
        }
    }

    public void CloseDialogBox() 
    {
        IsOpen = false;
        GameManager.instance.state = GameState.Freeroam;
        LeanTween.scale(dialogBoxGO, new Vector3(0, 0, 0), .06f);

        textLabel.text = string.Empty;
    }

    void OpenDialogBox() 
    {
        LeanTween.scale(dialogBoxGO, new Vector3(1, 1, 1), .06f);

    }
}
