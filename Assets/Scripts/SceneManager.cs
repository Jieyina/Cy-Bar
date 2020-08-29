using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public UIManager UI { get; private set; }
    public Farm Farm { get; private set; }
    public Factory Factory { get; private set; }
    public static SceneManager Instance { get; private set; } 
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        Player = FindObjectOfType<Player>();
        UI = FindObjectOfType<UIManager>();
        Farm = FindObjectOfType<Farm>();
        Factory = FindObjectOfType<Factory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
