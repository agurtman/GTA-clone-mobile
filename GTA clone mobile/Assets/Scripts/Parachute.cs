using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    [SerializeField] float airResistance;
    [SerializeField] float deploymentHeight;
    bool deployed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    public void OpenParachute()
    {
        deployed = true;
        rb.drag = airResistance;
        anim.SetTrigger("open");
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, -transform.up, Color.red);
        if (!deployed)
        {
            if (Physics.Raycast(ray, out hit, deploymentHeight))
            {
                if (hit.collider.tag == "Enviroment")
                {
                    OpenParachute();
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        anim.SetTrigger("close");
    }
}