using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }


    public void Return()
    {
        Cursor.visible = true;
        GeneticAlgorithm.SavePopulation();
        SceneManager.LoadScene(0);
    }


    public void Continue()
    {
        SceneManager.LoadScene(0);
    }


    public void NewGame()
    {
        // clear player prefs
        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene(0);
    }
}
