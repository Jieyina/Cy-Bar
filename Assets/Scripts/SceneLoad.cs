using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadS(string scenename){
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenename);
    }
}
