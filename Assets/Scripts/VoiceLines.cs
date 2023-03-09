using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{
    [SerializeField] private float threshold = .1f;
    [SerializeField] private float deadZone = 0.025f;
    [SerializeField] private AudioSource _audioSource;

    private bool _isPressed;
    private bool _isVisable;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;
    private bool _hasPlayed;

    
    // Start is called before the first frame update
    void Start()
    {
        _hasPlayed = false;
        _isPressed = false;
        _isVisable = true;

        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        if (!_isPressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }
        if (_isPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
        {
            value = 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        if (!_hasPlayed && !_isVisable)
        {
            _audioSource.Play();
            _hasPlayed = true;
            _isVisable = true;
        }
        else
        {
            _hasPlayed = false;
            _isVisable = false;
        }
    }

    private void Released()
    {
        _isPressed = false;
    }
}
