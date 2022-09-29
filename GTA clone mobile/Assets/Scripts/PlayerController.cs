using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Text HpText;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject rifleStart;
    [SerializeField] private Text ammoText;
    [SerializeField] ParticleSystem flash;
    [SerializeField] GameObject impact;
    [SerializeField] Text moneyText;
    [SerializeField] GameObject questTarget;
    [SerializeField] Dialogue dialogue;
    [SerializeField] Camera camera;

    public int money;
    private int ammo;
    private int capacity;
    private int capacityMax = 50;
    private int health;
    bool shoot;
    private float shootTimer;
    private Animator anim;
    float range = 100f;
    private float impactForce = 100f;


    public void ChangeHealth(int count)
    {
        health = health + count;
        HpText.text = health.ToString();
    }

    public void AddAmmo(int count)
    {
        ammo += count;
        ammoText.text = capacity + "/" + ammo;
    }

    public void Reload()
    {
        int need = capacityMax - capacity;
        if (need <= ammo)
        {
            ammo -= need;
            capacity += need;
        }
        else
        {
            capacity += ammo;
            ammo = 0;
        }
        ammoText.text = capacity + "/" + ammo;
    }

    void Start()
    {
        PlayerPrefs.SetInt("FindCar", 1);
        anim = GetComponent<Animator>();
        ChangeHealth(100);
        AddAmmo(100);
        Reload();
        GetMoney(1000);
    }

    public void Update()
    {
        shootTimer += Time.deltaTime;
        if (shoot && shootTimer >= 0.1f)
        {
            if (capacity <= 0)
            {
                return;
            }
            shootTimer = 0;
            capacity -= 1;
            ammoText.text = capacity + "/" + ammo;
            flash.Play();
            RaycastHit shootObj;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out shootObj, range))
            {
                GameObject inst = Instantiate(impact, shootObj.point, Quaternion.LookRotation(shootObj.normal));
                Destroy(inst, 0.5f);
                Debug.Log(shootObj.collider.name);
                if (shootObj.rigidbody != null)
                {
                    shootObj.rigidbody.AddForce(-shootObj.normal * impactForce);
                }
                if (shootObj.collider.tag == "RedBarrel")
                {
                    shootObj.collider.GetComponent<RedBarrel>().Boom();
                }
                if (shootObj.collider.tag == "enemy_ragdoll")
                {
                    shootObj.collider.transform.root.GetComponentInChildren<Enemy>().Death(true);
                    shootObj.collider.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * 100, ForceMode.Impulse);
                }
                if (shootObj.collider.tag == "Enemy")
                {
                    shootObj.collider.GetComponent<EnemyAI>().ChangeHealth(-10);
                }
            }
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.tag == "AI")
            {
                hit.transform.GetComponent<NavigationAI>().Fear();
            }
            if (hit.collider.tag == "Health")
            {
                ChangeHealth(50);
                Destroy(hit.collider.gameObject);
            }
            if (hit.collider.tag == "Ammo")
            {
                AddAmmo(50);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void OnPointerDown()
    {
        shoot = true;
    }

    public void OnPointerUp()
    {
        shoot = false;
    }

    public void SayHello()
    {
        anim.SetTrigger("hi");
    }

    public void HelloGuys(string say)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 50);
        foreach (var people in colliders)
        {
            if (people.tag == "people")
            {
                people.GetComponent<Animator>().SetTrigger("hi");
                print(say);
            }
        }
    }

    public void GetMoney(int count)
    {
        money += count;
        moneyText.text = "Money: " + money.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("greenCar"))
        {
            if (PlayerPrefs.GetInt("FindCar") == 2)
            {
                Destroy(collision.gameObject);
                PlayerPrefs.SetInt("FindCar", 3);
                dialogue.target.transform.position = questTarget.transform.position;
            }
        }
    }
}