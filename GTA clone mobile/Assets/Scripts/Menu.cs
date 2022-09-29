using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject optionsUi;
    [SerializeField] GameObject mainUi;
    [SerializeField] GameObject mainPosition;
    [SerializeField] GameObject volumePosition;
    GameObject camera;


    bool isOptions = false;
    public enum State { Main, Settings }
    State current;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        current = State.Main;
    }

    void Update()
    {
        if (!isOptions)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, mainPosition.transform.position, 0.05f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, mainPosition.transform.rotation, 0.05f);
        }
        else
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, volumePosition.transform.position, 0.05f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, volumePosition.transform.rotation, 0.05f);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        if (!isOptions) SwitchState(State.Settings);
        else SwitchState(State.Main);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SwitchState(State state)
    {
        current = state;
        switch (state)
        {
            case State.Main:
                mainUi.SetActive(true);
                optionsUi.SetActive(false);
                isOptions = false;
                break;
            case State.Settings:
                mainUi.SetActive(false);
                optionsUi.SetActive(true);
                isOptions = true;
                break;
            default:
                break;
        }
    }

}