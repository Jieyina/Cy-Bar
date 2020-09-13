using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneLoad : MonoBehaviour
{
    public void LoadS(string scenename){
        SceneManager.LoadScene(scenename);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitEditor()
    {
        EditorApplication.isPlaying = false;
    }
}
