using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KOTextBehaviour : MonoBehaviour
{
    public TMP_Text textComponent;
    public GameObject letterK;
    public GameObject letterO;

    private void Start()
    {
        StartCoroutine(TriggerKO());
    }

    // Update is called once per frame
    void Update()
    {
        /*textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++) 
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) 
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++) 
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++) 
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }*/

    }

    public IEnumerator TriggerKO() 
    {
        PopUpLetter(letterK, .4f);
        yield return new WaitForSecondsRealtime(.9f);
        PopUpLetter(letterO, .4f);
        yield return new WaitForSecondsRealtime(1.2f);
        Destroy(this.gameObject);
        
    }
    public void PopUpLetter(GameObject theGameObject, float overWhatTime)
    {
        LeanTween.cancel(theGameObject);
        theGameObject.transform.localScale = Vector3.zero;
        

        LeanTween.scale(theGameObject, new Vector3(1.5f, 1.5f, 2), overWhatTime)
            .setEaseOutElastic()
            .setDelay(.1f);
    }
}
