using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class DialogEvent 
{
    [HideInInspector] public string name;
    [SerializeField] private UnityEvent onPickedResponse;

    public UnityEvent OnPickedResponse => onPickedResponse;
}
