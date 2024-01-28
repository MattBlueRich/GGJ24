using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManagerScript : MonoBehaviour
{ 
    public void ToGame()
    {
        SceneManager.LoadScene("theCoolerLevel");
    }

    public void ToMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("mainMenu");
    }

    public void quitTheGame()
    {
        //do anything before quit
        Application.Quit();
    }
}
