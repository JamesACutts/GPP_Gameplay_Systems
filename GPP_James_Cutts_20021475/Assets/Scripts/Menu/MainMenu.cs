using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{

    public string firstLevel;
    public GameObject optionScreen;
    public GameObject controlScreen;

    public GameObject firstMenuButton, optionsFirstButton, optionsCloseButton,controlsFirstButton, controlsCloseButton;

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }
    public void OpenOptions()
    {
        optionScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }
    public void QuitOptions()
    {
        optionScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsCloseButton);
    }

    public void OpenControls()
    {
        controlScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(controlsFirstButton);
    }
    public void QuitControls()
    {
        controlScreen.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(controlsCloseButton);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
