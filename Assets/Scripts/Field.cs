using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.scene.isLoaded)
        {
            other.SendMessage("OutOfBounds", null, SendMessageOptions.DontRequireReceiver);
        }
    }
}
