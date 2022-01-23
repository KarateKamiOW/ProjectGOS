using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    public float timeUntilDestruction;

    // Update is called once per frame
    void Update()
    {
        timeUntilDestruction -= Time.deltaTime;
        if (timeUntilDestruction <= 0)
            Destroy(this.gameObject);
    }
}
