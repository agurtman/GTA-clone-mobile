using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaceController : MonoBehaviour
{
    [SerializeField] List<Transform> checkPointPositions;
    [SerializeField] Transform carPosition;   
    [SerializeField] GameObject checkPointVisual;
    [SerializeField] GameObject finish;
    [SerializeField] Text timeText;
    [SerializeField] Text finishUI;
    GameObject target;
    int countCheckPoint = 0;
    int count;
    bool isFinish;
    float timer;

    void Start()
    {
        target = Instantiate(checkPointVisual);
        ChangeCheckPoint();
        count = 0;
    }
    private void ChangeCheckPoint()
    {
        target.transform.position = checkPointPositions[countCheckPoint].position;
        target.transform.rotation = checkPointPositions[countCheckPoint].rotation;
    }

    void Update()
    {
        if (target != null && carPosition.position.x >= target.transform.position.x)
        {
            if (countCheckPoint < checkPointPositions.Count - 1)
            {
                countCheckPoint += 1;
                ChangeCheckPoint();
            }
            else
            {
                finish.SetActive(true);
            }
        }

        if (!isFinish)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCar"))
        {
            count++;
        }
        if (other.CompareTag("Car"))
        {
            isFinish = true;
            count++;
            finishUI.text = "Ты занял " + count.ToString() + " место!";
            Invoke("ExitRace", 5f);
        }
    }

    private void ExitRace()
    {
        SceneManager.LoadScene(1);
    }
}