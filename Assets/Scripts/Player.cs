using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public int initMoney = 0;
    [SerializeField]
    public int initYear = 0;
    [SerializeField]
    public int initMonth = 0;

    private int money;
    private int star;
    private int initTime;
    private float remainTime;
    private bool counting;

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
        SceneManager.Instance.UI.UpdateMoney(money);
        star = 0;
        initTime = initYear * 120 + initMonth * 10;
        remainTime = initTime;
        counting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            remainTime -= GameItem.PlaySpeed * Time.deltaTime;
            SceneManager.Instance.UI.UpdateTimeSlider(remainTime/initTime);
            if (remainTime<0)
            {

            }
        }
    }
}
