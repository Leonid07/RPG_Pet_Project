using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button ButtonExit;
    public GameObject[] PanelOptions;
    void Start()
    {
        ButtonExit.onClick.AddListener(OnExitButtonClick);
        foreach (GameObject panel in PanelOptions)
        {
            panel.SetActive(false);
        }
    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
