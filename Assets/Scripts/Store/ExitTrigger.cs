using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private StoreManager sm;

    private void Start()
    {
        sm = FindObjectOfType<StoreManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sm.Exit();
        }


    }
}
