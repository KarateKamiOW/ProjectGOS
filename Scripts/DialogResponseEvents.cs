using System;
using UnityEngine;


public class DialogResponseEvents : MonoBehaviour
{
    [SerializeField] DialogObject dialogObj;
    [SerializeField] DialogEvent[] events;

    public DialogObject DialogObj => dialogObj;

    public DialogEvent[] ResponseEvents => events;

    public void OnValidate() 
    {
        if (dialogObj == null) return;
        if (dialogObj.Responses == null) return;
        if (events != null && events.Length == dialogObj.Responses.Length) return;

        if (events == null)
        {
            events = new DialogEvent[dialogObj.Responses.Length];
        }
        else 
        {
            Array.Resize(ref events, dialogObj.Responses.Length);
        }

        for (int i = 0; i < dialogObj.Responses.Length; i++) 
        {
            OverworldDialogResponse response = dialogObj.Responses[i];

            if (events[i] != null) 
            {
                events[i].name = response.ResponseText;
                continue;
            }

            events[i] = new DialogEvent() { name = response.ResponseText };
        }
    }
}
