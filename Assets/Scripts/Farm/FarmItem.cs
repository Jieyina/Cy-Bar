using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmItem : MonoBehaviour
{
    [SerializeField]
    private TextMesh progress = null;

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
            Growing = true;
            startGrowTime = Time.time;
            progress.text = "0%";
        }
    }

    public int GetAmount()
    {
        return amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Growing)
        {
            float timePast = Time.time - startGrowTime;
            if (timePast > produceTime)
            {
                Growing = false;
                progress.text = "100%";
                if (SceneManager.Instance.Storage.CheckCapacity())
                {
                    SceneManager.Instance.Storage.AddMaterial(matName, amount);
                    RestartGrow();
                }
                else
                {
                    toStore = true;
                }
            }
            else
            {
                progress.text = Mathf.Floor(timePast / produceTime * 100).ToString() + "%";
            }
        }

        if (toStore)
        {
            if (SceneManager.Instance.Storage.CheckCapacity())
            {
                SceneManager.Instance.Storage.AddMaterial(matName, amount);
                RestartGrow();
                toStore = false;
            }
        }
    }
}
