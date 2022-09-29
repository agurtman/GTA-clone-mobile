using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] GameObject car;
    CarController сarController;
    [SerializeField] float radius;
    [SerializeField] Joystick joystick;
    [SerializeField] float speed = 5;
    [SerializeField] Transform point;
    [SerializeField] Camera carCamera;
    [SerializeField] AudioSource carAudio;
    [SerializeField] AudioListener carAudioListener;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject carUI;
    NavMeshAgent agent;
    Rigidbody rb;
    Vector3 direction;
    Animator anim;
    bool isGrounded;
    bool isDriver;

    void Start()
    {
        сarController = car.GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.enabled == true)
        {
            agent.SetDestination(point.position);
        }
        if (!isDriver)
        {
            float horizontal = joystick.Horizontal;
            float vertical = joystick.Vertical;
            direction = transform.TransformDirection(horizontal, 0, vertical);
            anim.SetFloat("move", Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical)));
        }
        if (isDriver && agent.remainingDistance < .25f)
        {
            Invoke("SwitchCamera", 1f);
            isDriver = false;
            agent.enabled = false;
            transform.LookAt(car.transform);
            сarController.enabled = true;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + speed * direction * Time.deltaTime);
    }

    public void InCar()
    {
        StartCoroutine("IInCar");
    }

    private void SwitchCamera()
    {
        carCamera.enabled = true;
        gameObject.SetActive(false);
        gameObject.transform.SetParent(car.transform);
        carAudioListener.enabled = true;
        carAudio.Play();
        gameUI.SetActive(false);
        carUI.SetActive(true);
    }

    IEnumerator IInCar()
    {
        if (Vector3.Distance(transform.position, car.transform.position) <= radius && !isDriver)
        {
            agent.enabled = true;
            agent.speed = 10;
            yield return new WaitForSeconds(1);
            isDriver = true;
            anim.SetFloat("move", 1f);
        }
    }
}