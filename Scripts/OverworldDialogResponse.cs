using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverworldDialogResponse 
{
    [SerializeField] string responseText;
    [SerializeField] DialogObject dialogObject;

    public string ResponseText => responseText;

    public DialogObject DialogObject => dialogObject;
}
