using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmItem : MonoBehaviour
{
    public Material material1;
    public Material material2;

    private string matName;
    private int amount;
    private float produceTime;
    private int cost;

    private bool Growing;
    private float startGrowTime;
    private bool toStore;

    public void SetProps(string str, int num, float time, int price)
    {
        matName = str;
        amount = num;
        produceTime = time;
        cost = price;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.Instance.Player.canAfford(cost))
        {
            SceneManager.Instance.Player.spendMoney(cost);
            Growing = true;
            startGrowTime = Time.time;
        }
    }

    private void RestartGrow()
    {
        if (SceneManager.Instance.Player.canAfford(cost))
        {
            SceneManager.Instance.Player.spendMoney(cost);
            transform.Find("model").gameObject.GetComponent<Renderer>().material = material1;
            Growing = true;
            startGrowTime = Time.time;
        }
    }

    public int GetAmount()
    {
        return amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Growing && Time.time - startGrowTime > produceTime)
        {
            transform.Find("model").gameObject.GetComponent<Renderer>().material = material2;
            Growing = false;
            //SceneManager.Instance.Farm.AddProduct(matName,gameObject);
            if (SceneManager.Instance.Storage.CheckCapacity())
            {
                SceneManager.Instance.Storage.AddMaterial(matName, amount);
                Debug.Log("stored " + matName + " x " + amount);
                RestartGrow();
            }
            else
            {
                toStore = true;
            }
        }

        if (toStore)
        {
            if (SceneManager.Instance.Storage.CheckCapacity())
            {
                SceneManager.Instance.Storage.AddMaterial(matName, amount);
                Debug.Log("stored " + matName + " x " + amount);
                RestartGrow();
                toStore = false;
            }
        }
    }
}
