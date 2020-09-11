using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmItem : GameItem
{
    [SerializeField]
    private TextMesh progress = null;

    private string matName;
    private int amount;
    private float produceTime;
    private int cost;
    private int buildCost;

    private bool Growing;
    private float remainTime;
    private bool toStore;

    public void SetProps(string str, int num, float time, int price, int bCost)
    {
        matName = str;
        amount = num;
        produceTime = time;
        cost = price;
        buildCost = cost;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (SceneManager.Instance.Player.canAfford(cost))
        {
            SceneManager.Instance.Player.spendMoney(cost);
            Growing = true;
            remainTime = produceTime;
        }
    }

    private void RestartGrow()
    {
        if (SceneManager.Instance.Player.canAfford(cost))
        {
            SceneManager.Instance.Player.spendMoney(cost);
            Growing = true;
            remainTime = produceTime;
            progress.text = "0%";
        }
    }

    public override void DestroyItem()
    {
        SceneManager.Instance.Player.GainMoney((int)Mathf.Floor(buildCost * 0.5f));
        transform.parent.gameObject.layer = 9;
        base.DestroyItem();
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
            remainTime -= playSpeed * Time.deltaTime;
            if (remainTime < 0)
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
                progress.text = Mathf.Floor((produceTime - remainTime) / produceTime * 100).ToString() + "%";
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
