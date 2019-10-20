using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuAbout : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            BackToMainMenu();
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}