using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Player player { get; private set; }
    public UIManager ui { get; private set; }
    public Farm farm { get; private set; }
    public static SceneManager Instance { get; private set; } // static singleton
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        // Cache references to all desired variables
        player = FindObjectOfType<Player>();
        ui = FindObjectOfType<UIManager>();
        farm = FindObjectOfType<Farm>();
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
