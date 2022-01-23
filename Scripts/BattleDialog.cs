using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleDialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] int lettersPerSecond;


    public void SetDialog(string dialog) 
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog) 
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray()) 
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(.5f);
    }
}
