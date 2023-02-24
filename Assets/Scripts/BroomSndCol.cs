using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomSndCol : MonoBehaviour
{
    public AudioSource broom;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject)
        {
            broom.Play();
        }
    }

}
