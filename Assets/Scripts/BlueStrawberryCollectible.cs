using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueStrawberryCollectible : MonoBehaviour
{
    public AudioClip bluecollectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
                Destroy(gameObject);

                controller.PlaySound(bluecollectedClip);

            controller.collectedBlueStrawb = true;
        }

    }
}
