using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gauge : MonoBehaviour
{
    public const float MaxValue = 1.0f;

    [SerializeField]
    UnityEvent<float> valueUpdated;

    [SerializeField]
    float increment = 0.1f;

    float _value = 0.0f;
    int direction = 1;

    float value
    {
        get => _value;
        set
        {
            if (value > MaxValue)
            {
                value = (2 * MaxValue) - value;
                direction = -1;
            }
            else if (value < 0.0f)
            {
                value = -value;
                direction = 1;
            }
            _value = value;
            valueUpdated.Invoke(_value);
        }
    }

    void FixedUpdate()
    {
        value += increment * direction;
    }

    public void Init()
    {
        value = Random.Range(0.0f, MaxValue);
        direction = Random.Range(0, 2) == 0 ? 1 : -1;
    }
}
