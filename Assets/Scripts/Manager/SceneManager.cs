using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public UIManager UI { get; private set; }
    public Factory Factory { get; private set; }
    public Storage Storage { get; private set; }
    public Bar Bar { get; private set; }
    public static SceneManager Instance { get; private set; }

    private bool destroy = false;
    private RaycastHit hit;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        Player = FindObjectOfType<Player>();
        UI = FindObjectOfType<UIManager>();
        Factory = FindObjectOfType<Factory>();
        Storage = FindObjectOfType<Storage>();
        Bar = FindObjectOfType<Bar>();
    }

    public void SpeedUp()
    {
        GameItem.SetPlaySpeed(2);
        //GameItem[] items = FindObjectsOfType<GameItem>();
        //if (items.Length != 0)
        //{
        //    foreach (var item in items)
        //        item.SetAnimSpeed();
        //}
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 2;
        }
    }

    public void RestoreSpeed()
    {
        GameItem.SetPlaySpeed(1);
        //GameItem[] items = FindObjectsOfType<GameItem>();
        //if (items.Length != 0)
        //{
        //    foreach (var item in items)
        //        item.SetAnimSpeed();
        //}
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 1;
        }
    }

    public void Pause()
    {
        GameItem.SetPlaySpeed(0);
        //GameItem[] items = FindObjectsOfType<GameItem>();
        //if (items.Length != 0)
        //{
        //    foreach (var item in items)
        //        item.SetAnimSpeed();
        //}
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 0;
        }
    }

    public void DestroyItem()
    {
        destroy = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50000f, 1<<11))
                {
                    hit.transform.parent.gameObject.GetComponent<GameItem>().DestroyItem();
                }
                destroy = false;
            }
        }
    }
}
