using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class NavigationAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator anim;
    [SerializeField] List<Transform> points = new List<Transform>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        SetDestination();
    }

    virtual protected void Update()
    {
        if (agent.remainingDistance < .25f)
        {
            StartCoroutine("Idle");
        }
    }

    public void SetDestination()
    {
        Vector3 newTarget = points[Random.Range(0, points.Count)].position;
        agent.SetDestination(newTarget);
    }

    public void Fear()
    {
        StartCoroutine("Fear1");
    }

    IEnumerator Idle()
    {
        agent.speed = 0;
        SetDestination();
        anim.SetBool("idle", true);
        yield return new WaitForSeconds(5);
        agent.speed = 3.5f;
        anim.SetBool("idle", false);

    }

    IEnumerator Fear1()
    {
        agent.speed = 10;
        anim.SetBool("fear", true);
        SetDestination();
        yield return new WaitForSeconds(3);
        agent.speed = 3.5f;
        anim.SetBool("fear", false);
    }
}