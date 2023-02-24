using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSound : MonoBehaviour
{
public GameObject Player;
    Vector3 pos1;
    Vector3 pos2;
    public AudioSource Creak;
    public 
    // Update is called once per frame
    void Update()
    {
        pos1= Player.transform.position;
        if(pos2!=null&& pos1 != pos2)
        {
            Creak.Play();
        }
        pos2 = pos1;
    }
}
