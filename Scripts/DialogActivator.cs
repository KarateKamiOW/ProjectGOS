using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour, IInteractable
{
    [SerializeField] DialogObject dialogObj;

    [SerializeField] DialogUI dialogUI;

    public void UpdateDialogObject(DialogObject dialogObj)
    {
        this.dialogObj = dialogObj;
    }

    public void AddQuest(QuestObject testQuest)
    {
        PlayerController.instance.QuestLog.AddQuest(testQuest);
    }

    public void Interact(PlayerController player) 
    {
        /*if (TryGetComponent(out DialogResponseEvents responseEvents) && responseEvents.DialogObj == dialogObj) 
        {
            Debug.Log("Try Get Worked.");
            player.DialogueUI.AddResponseEvents(responseEvents.ResponseEvents);
        }*/
        PlayerController.instance.DialogueUI = dialogUI;
        foreach (DialogResponseEvents responseEvents in GetComponents<DialogResponseEvents>()) 
        {
            player.DialogueUI.AddResponseEvents(responseEvents.ResponseEvents);
            break;
        }

        if(GameManager.instance.state == GameState.Freeroam)
            dialogUI.ShowDialog(dialogObj);
        //player.DialogueUI.ShowDialog(dialogObj);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player)) 
        {   
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player)) 
        {
            if(player.Interactable is DialogActivator dialogActivator && dialogActivator == this)
                player.Interactable = null;
        }
    }
}
