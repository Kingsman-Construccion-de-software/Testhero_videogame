using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTrigger : MonoBehaviour
{

    [SerializeField]
    private int index;

    [SerializeField]
    private bool increase;

    private StoreManager sm;

    private void Start()
    {
        sm = FindObjectOfType<StoreManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (increase)
            {
                sm.IncreaseAmount(index);
            }
            else
            {
                sm.DecreaseAmount(index);
            }
        }

        
    }
}
