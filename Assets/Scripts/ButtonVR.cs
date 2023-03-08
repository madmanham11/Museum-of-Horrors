using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public GameObject canvas;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    bool isPressed;
    bool isVisable;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        isVisable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.0015f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
            if (isVisable)
            {
                isVisable = false;
            } else
            {
                isVisable = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.090f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void ShowHide()
    {
        if(isVisable)
        {
            canvas.SetActive(true);
        }else
        {
            canvas.SetActive(false);
        }
    }
}
