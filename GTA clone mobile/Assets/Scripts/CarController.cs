using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    [SerializeField] List<AxleInfo> axleInfos;
    [SerializeField] float maxMotorTorque;
    [SerializeField] float maxSteeringAngle;
    [SerializeField] Joystick joystick;
    [SerializeField] TrailRenderer leftWheel;
    [SerializeField] TrailRenderer rightWheel;
    [SerializeField] GameObject player;
    [SerializeField] AudioListener audioListener;
    [SerializeField] GameObject raceUI;
    [SerializeField] GameObject SpotLight;
    [SerializeField] GameObject PointLight;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject carUI;
    bool isOn = false;
    float pi;
    bool isBreak;

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }
        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {
        pi = Mathf.Lerp(0.6f, 1.6f, joystick.Vertical);
        GetComponent<AudioSource>().pitch = Mathf.Lerp(GetComponent<AudioSource>().pitch, pi, 0.01f);

        float motor = maxMotorTorque * joystick.Vertical;
        float steering = maxSteeringAngle * joystick.Horizontal;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = -motor;
                axleInfo.rightWheel.motorTorque = -motor;
            }
            if (!isBreak)
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;

                leftWheel.emitting = false;
                rightWheel.emitting = false;
                PointLight.SetActive(false);
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 2000;
                axleInfo.rightWheel.brakeTorque = 2000;

                leftWheel.emitting = true;
                rightWheel.emitting = true;
                PointLight.SetActive(true);
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);

        }
    }

    public void SpotLightEnabled()
    {
        if (!isOn)
        {
            SpotLight.SetActive(true);
            isOn = true;
        }
        else
        {
            SpotLight.SetActive(false);
            isOn = false;
        }
    }

    public void StopOn()
    {
        isBreak = true;
    }

    public void StopOff()
    {
        isBreak = false;
    }

    public void ExitCar()
    {
        player.SetActive(true);
        player.transform.parent = null;
        this.enabled = false;
        audioListener.enabled = false;
        GetComponent<AudioSource>().Stop();
        gameUI.SetActive(true);
        carUI.SetActive(false);
    }

    public void StartRace()
    {
        SceneManager.LoadScene("Drift track");
    }
    public void ExitRace()
    {
        raceUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StartRace"))
        {
            raceUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StartRace"))
        {
            ExitRace();
        }
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}