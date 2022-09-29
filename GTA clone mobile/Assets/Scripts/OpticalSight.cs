using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpticalSight : MonoBehaviour
{
    [SerializeField] Camera cameraMain;
    [SerializeField] Camera opticCamera;
    [SerializeField] GameObject scopeUI;
    [SerializeField] GameObject moneyUI;
    PlayerLook playerLook;
    float mouse;
    private bool isScopeOn;
    float mouseMax = 0.5f;
    float maxFOV = 60;

    void Start()
    {
        playerLook = GetComponent<PlayerLook>();
        isScopeOn = false;
        scopeUI.SetActive(false);
    }

    public void SwitchScope()
    {
        if (isScopeOn)
        {
            isScopeOn = false;
            cameraMain.enabled = true;
            opticCamera.enabled = false;
            scopeUI.SetActive(false);
            moneyUI.SetActive(true);
        }
        else
        {
            mouse = mouseMax;
            isScopeOn = true;
            cameraMain.enabled = false;
            opticCamera.enabled = true;
            scopeUI.SetActive(true);
            moneyUI.SetActive(false);
        }
    }

    public void OnScopeChanged(float value)
    {
        opticCamera.fieldOfView = value;
        mouse = value / maxFOV * mouseMax;
        playerLook.ChangeMouseSensivity(mouse);
    }
}