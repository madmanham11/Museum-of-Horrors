using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLusion : MonoBehaviour
{
    // Start is called before the first frame update
    System.Random ran = new System.Random();

    // Start is called before the first frame update
    [SerializeField] GameObject handslu;


    // Update is called once per frame
    void Update()
    {
     GotHands();
        
    }
    private void GotHands()
    {
        int chance = ran.Next(0, 12500);
        if (chance >= 12499)
        {
            Debug.Log("Start");

            if (!handslu.activeSelf)
            {
                handslu.SetActive(true);
                Invoke("losthands", 1.5f);
            }
            
        }
    

    }

    private void losthands()
    {
        Debug.Log("End");

        handslu.SetActive(false);
    }
}
