using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int initMoney;
    private int money;

    public void spendMoney(int cost)
    {
        money -= cost;
        SceneManager.Instance.UI.updateMoney(money);
    }

    public bool canAfford(int cost)
    {
        return money >= cost;
    }

    // Start is called before the first frame update
    void Start()
    {
        money = initMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
