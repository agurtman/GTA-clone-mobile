using UnityEngine;

public class EnemyAI : NavigationAI
{
    [SerializeField][Range(0, 360)] private float ViewAngle = 90f;
    [SerializeField] private float ViewDistance = 15f;
    [SerializeField] private Transform Target;
    float timer = 0;
    public int health = 100;
    bool isDead;


    override protected void Update()
    {
        float distanceToPlayer = Vector3.Distance(Target.transform.position, agent.transform.position);
        if (IsInView() && isDead == false)
        {
            if (distanceToPlayer >= 2f)
                MoveToTarget();
            else
            {
                timer += Time.deltaTime;
                agent.isStopped = true;
                anim.SetBool("idle", true);
                if (timer > 1)
                {
                    timer = 0;
                    anim.SetTrigger("hit");
                    Target.GetComponentInChildren<PlayerController>().ChangeHealth(-10);
                }
            }
        }
        else
        {
            agent.isStopped = false;
            base.Update();
        }
        DrawView();
    }

    private bool IsInView()
    {
        float currentAngle = Vector3.Angle(transform.forward, Target.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Target.position - transform.position, out hit, ViewDistance))
        {
            if (currentAngle < ViewAngle / 2f && Vector3.Distance(transform.position, Target.position) <= ViewDistance && hit.transform == Target.transform)
            {
                return true;
            }
        }
        return false;
    }
    private void MoveToTarget()
    {
        agent.isStopped = false;
        agent.speed = 3.5f;
        agent.SetDestination(Target.position);
    }

    private void DrawView()
    {
        Vector3 left = transform.position + Quaternion.Euler(new Vector3(0, ViewAngle / 2f, 0)) * (transform.forward * ViewDistance);
        Vector3 right = transform.position + Quaternion.Euler(-new Vector3(0, ViewAngle / 2f, 0)) * (transform.forward * ViewDistance);
        Debug.DrawLine(transform.position, left, Color.yellow);
        Debug.DrawLine(transform.position, right, Color.yellow);
    }

    public void ChangeHealth(int count)
    {
        health += count;
        if (health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        anim.SetTrigger("dead");
        agent.speed = 0;
        isDead = true;
    }
}
