using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Arrow : MonoBehaviour
{
    enum State {Idle, Power, Angle};

    [SerializeField]
    UnityEvent acceptPower;

    [SerializeField]
    UnityEvent<float> acceptAngle;

    [SerializeField]
    Transform player;

    [SerializeField]
    Transform shaft;

    [SerializeField]
    float maxRotation = 60.0f;

    [SerializeField]
    float minScale = 0.1f;

    [SerializeField]
    float powerScale = 1.0f;

    float referenceAngle = 0.0f;
    State state = State.Power;

    float power
    {
        get => shaft.localScale.y * powerScale;
        set
        {
            Vector3 scale = shaft.localScale;
            scale.y = value;
            shaft.localScale = scale;
        }
    }

    float angle
    {
        get => player.localEulerAngles.z;
        set
        {
            Vector3 angles = player.localEulerAngles;
            angles.z = value;
            player.localEulerAngles = angles;
        }
    }

    public float value
    {
        get
        {
            switch (state)
            {
                case State.Power: return power;
                case State.Angle: return angle;
            }
            return 0.0f;
        }
    }

    public void AcceptValue()
    {
        switch (state)
        {
            case State.Power:
                state = State.Angle;
                acceptPower.Invoke();
                break;
            case State.Angle:
                state = State.Idle;
                acceptAngle.Invoke(power);
                break;
        }
    }

    public void Init()
    {
        referenceAngle = player.localEulerAngles.z;
        state = State.Power;
    }

    public void ValueUpdated(float value)
    {
        switch (state)
        {
            case State.Power: power = minScale + ((1.0f - minScale) * (value / Gauge.MaxValue)); break;
            case State.Angle: angle = referenceAngle + (2.0f * maxRotation * (value - (0.5f * Gauge.MaxValue))); break;
        }
    }
}
