using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsCollectible : MonoBehaviour
{
    public AudioClip bootsCollectedClip;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        { 
                Destroy(gameObject);

                controller.PlaySound(bootsCollectedClip);
                controller.speedBoots();
        }

    }
}
