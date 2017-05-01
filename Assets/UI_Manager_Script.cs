using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager_Script : MonoBehaviour {

    //menu
    public void StartGame()
    {
        SceneManager.LoadScene("levelchooser");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        SceneManager.LoadScene("settings");
    }


    //levelchooser
    public void StartLevelTuto()
    {

        SceneManager.LoadScene("levelTuto");
    }

    public void StartLevel1()
    {
        SceneManager.LoadScene("main");
    }


    //settings
    public void SaveSettings()
    {

    }

    public void BackMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
