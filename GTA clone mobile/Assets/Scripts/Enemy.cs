using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator anim;
    Rigidbody[] childrenRb;

    void Start()
    {
        anim = GetComponent<Animator>();
        childrenRb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childrenRb)
        {
            rb.isKinematic = true;
            rb.tag = "enemy_ragdoll";
        }
    }

    void Update()
    {

    }

    public void Death(bool gravity)
    {
        foreach (Rigidbody rb in childrenRb)
        {
            rb.isKinematic = false;
            rb.useGravity = gravity;
        }
        anim.enabled = false;
    }

    public void OffTelekinesis()
    {
        foreach (Rigidbody rb in childrenRb)
        {
            rb.useGravity = true;
        }
    }
}