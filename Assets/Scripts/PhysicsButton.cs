using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float threshold = .1f;
    [SerializeField] private float deadZone = 0.025f;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    private bool _isVisable = true;

    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }
        if(_isPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if(Mathf.Abs(value) < deadZone)
        {
            value = 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        ShowHide();
    }

    private void Released()
    {
        _isPressed = false;
    }

    public void ShowHide()
    {
        if (!_isVisable)
        {
            canvas.SetActive(true);
            _isVisable = true;
        }
        else
        {
            canvas.SetActive(false);
            _isVisable = false;
        }
    }
}
