using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmItem : MonoBehaviour
{
    private string matName;
    private int amount;
    private float produceTime;

    private bool Growing;
    private float startGrowTime;

    public void SetProps(string str, int num, float time)
    {
        matName = str;
        amount = num;
        produceTime = time;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start grow " + matName);
        Growing = true;
        startGrowTime = Time.time;
    }

    public void RestartGrow()
    {
        Debug.Log("Start grow " + matName);
        Growing = true;
        startGrowTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Growing && Time.time - startGrowTime > produceTime)
        {
            Debug.Log("Growed " + matName);
            Growing = false;
            SceneManager.Instance.Farm.AddGrowedItem(gameObject);
            SceneManager.Instance.Farm.AddMaterial(matName,amount);
        }
    }
}
