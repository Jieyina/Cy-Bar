using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public int initMoney = 0;
    private int money;
    private int star;

    public void spendMoney(int cost)
    {
        money -= cost;
        SceneManager.Instance.UI.UpdateMoney(money);
    }

    public void GainMoney(int num)
    {
        money += num;
        SceneManager.Instance.UI.UpdateMoney(money);
    }

    public void GainStar(int num)
    {
        star += num;
        SceneManager.Instance.UI.UpdateStar(star);
    }

    public bool canAfford(int cost)
    {
        return money >= cost;
    }

    // Start is called before the first frame update
    void Start()
    {
        money = initMoney;
        star = 0;
        SceneManager.Instance.UI.UpdateMoney(money);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
