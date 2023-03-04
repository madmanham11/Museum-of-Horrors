using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class LaugingRandomizer : MonoBehaviour
{
    System.Random ran = new System.Random();

    // Start is called before the first frame update
    [SerializeField] AudioSource laugh;

    // Update is called once per frame
    void Start()
    {

    }
    private void Update()
    {
        int chance= ran.Next(0,10000);
        if (chance >=9980)
        {
            laughing();
        }
    }

    private void laughing()
    {
        
            int stereo = ran.Next(-1, 2);
        if (!laugh.isPlaying)
        {
            laugh.panStereo = stereo;
            laugh.Play();
        }
            }
}
