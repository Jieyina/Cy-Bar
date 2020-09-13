using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int initMoney = 0;
    [SerializeField]
    private int initYear = 0;
    [SerializeField]
    private int initMonth = 0;
    [SerializeField]
    private int coinGoal = 0;
    [SerializeField]
    private int starGoal = 0;
    [SerializeField]
    private int foodGoal = 0;
    [SerializeField]
    private int drinkGoal = 0;

    private int money;
    private int star = 0;
    private int foodServed = 0;
    private int drinkServed = 0;
    private int initTime;
    private float remainTime;
    private bool counting;
    private bool coinGoalFin = true;
    private bool starGoalFin = true;
    private bool foodGoalFin = true;
    private bool drinkGoalFin = true;


    public void spendMoney(int cost)
    {
        money -= cost;
        SceneManager.Instance.UI.UpdateMoney(money);
    }

    public void GainMoney(int num)
    {
        money += num;
        SceneManager.Instance.UI.UpdateMoney(money);
        if (coinGoal != 0)
        {
            SceneManager.Instance.UI.UpdateCoinGoal(money, coinGoal);
            if (!coinGoalFin && money >= coinGoal)
            {
                coinGoalFin = true;
                SceneManager.Instance.UI.CheckCoinGoal();
            }
        }
    }

    public void GainStar(int num)
    {
        star += num;
        SceneManager.Instance.UI.UpdateStar(star);
        if (starGoal != 0)
        {
            SceneManager.Instance.UI.UpdateStarGoal(star, starGoal);
            if (!starGoalFin && star >= starGoal)
            {
                starGoalFin = true;
                SceneManager.Instance.UI.CheckStarGoal();
            }
        }
    }

    public bool canAfford(int cost)
    {
        return money >= cost;
    }

    public void GetFoodServe(int num)
    {
        foodServed+=num;
        if (foodGoal != 0)
        {
            SceneManager.Instance.UI.UpdateFoodGoal(foodServed, foodGoal);
            if (!foodGoalFin && foodServed >= foodGoal)
            {
                foodGoalFin = true;
                SceneManager.Instance.UI.CheckFoodGoal();
            }
        }
    }

    public void GetDrinkServe(int num)
    {
        drinkServed+=num;
        if (drinkGoal != 0)
        {
            SceneManager.Instance.UI.UpdateDrinkGoal(drinkServed, drinkGoal);
            if (!drinkGoalFin && drinkServed >= drinkGoal)
            {
                drinkGoalFin = true;
                SceneManager.Instance.UI.CheckDrinkGoal();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        money = initMoney;
        SceneManager.Instance.UI.UpdateMoney(money);
        initTime = initYear * 120 + initMonth * 10;
        remainTime = initTime;
        counting = true;
        if (coinGoal != 0)
        {
            coinGoalFin = false;
            SceneManager.Instance.UI.UpdateCoinGoal(0, coinGoal);
        }
        if (starGoal != 0)
        {
            starGoalFin = false;
            SceneManager.Instance.UI.UpdateStarGoal(0, starGoal);
        }
        if (foodGoal != 0)
        {
            foodGoalFin = false;
            SceneManager.Instance.UI.UpdateFoodGoal(0, foodGoal);
        }
        if (drinkGoal != 0)
        {
            drinkGoalFin = false;
            SceneManager.Instance.UI.UpdateDrinkGoal(0, drinkGoal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            remainTime -= GameItem.PlaySpeed * Time.deltaTime;
            SceneManager.Instance.UI.UpdateTimeSlider(remainTime/initTime);
            if (coinGoalFin&&starGoalFin&&foodGoalFin&&drinkGoalFin)
            {
                counting = false;
            }
            if (remainTime<0)
            {
                counting = false;
            }
        }
    }
}
