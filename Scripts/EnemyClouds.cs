using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyClouds : MonoBehaviour
{
    Animator anim;
    public EnemyObject enemy;
    //When a player comes in contact with this cloud, it will trigger a scene swap with the enemy data
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("EnemyBattle");
        }
    }
}
