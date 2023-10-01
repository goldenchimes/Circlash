using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    const float MinimumReflex = 0.5f;
    const float MaximumReflex = 1.0f;

    const float MinimumPerception = 0.15f;
    const float MaximumPerception = 0.25f;

    const float MinimumTolerance = 0.1f;
    const float MaximumTolerance = 0.75f;

    const float MinimumPatience = 1.0f;
    const float MaximumPatience = 3.0f;

    [SerializeField]
    Arrow arrow;

    float reflex = 0.0f;
    float perception = 0.0f;
    float tolerance = 0.0f;
    float patience = 0.0f;

    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
        reflex = Random.Range(MinimumReflex, MaximumReflex);
        perception = Random.Range(MinimumPerception, MaximumPerception);
        tolerance = Random.Range(MinimumTolerance, MaximumTolerance);
        patience = Random.Range(MinimumPatience, MaximumPatience);
    }

    public void OnEnable()
    {
        StartCoroutine(Observe());
    }

    IEnumerator Observe()
    {
        float time = 0.0f;
        bool observing = true;
        yield return new WaitForSeconds(reflex);
        while (observing)
        {
            if (arrow.value <= tolerance)
            {
                observing = false;
            }
            else
            {
                yield return new WaitForSeconds(perception);
                time += perception;
                observing = time < patience;
            }
        }
        enabled = false;
        player.AcceptValue();
        yield return null;
    }
}
