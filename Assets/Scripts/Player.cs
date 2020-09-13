using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private int coinEarned = 0;
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
        SceneItemManager.Instance.UI.UpdateMoney(money);
    }

    public void GainMoney(int num)
    {
        money += num;
        coinEarned += num;
        SceneItemManager.Instance.UI.UpdateMoney(money);
        if (coinGoal != 0)
        {
            SceneItemManager.Instance.UI.UpdateCoinGoal(money, coinGoal);
            if (!coinGoalFin && coinEarned >= coinGoal)
            {
                coinGoalFin = true;
                SceneItemManager.Instance.UI.CheckCoinGoal();
            }
        }
    }

    public void GainStar(int num)
    {
        star += num;
        SceneItemManager.Instance.UI.UpdateStar(star);
        if (starGoal != 0)
        {
            SceneItemManager.Instance.UI.UpdateStarGoal(star, starGoal);
            if (!starGoalFin && star >= starGoal)
            {
                starGoalFin = true;
                SceneItemManager.Instance.UI.CheckStarGoal();
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
            SceneItemManager.Instance.UI.UpdateFoodGoal(foodServed, foodGoal);
            if (!foodGoalFin && foodServed >= foodGoal)
            {
                foodGoalFin = true;
                SceneItemManager.Instance.UI.CheckFoodGoal();
            }
        }
    }

    public void GetDrinkServe(int num)
    {
        drinkServed+=num;
        if (drinkGoal != 0)
        {
            SceneItemManager.Instance.UI.UpdateDrinkGoal(drinkServed, drinkGoal);
            if (!drinkGoalFin && drinkServed >= drinkGoal)
            {
                drinkGoalFin = true;
                SceneItemManager.Instance.UI.CheckDrinkGoal();
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        money = initMoney;
        SceneItemManager.Instance.UI.UpdateMoney(money);
        initTime = initYear * 120 + initMonth * 10;
        remainTime = initTime;
        counting = true;
        if (coinGoal != 0)
        {
            coinGoalFin = false;
            SceneItemManager.Instance.UI.UpdateCoinGoal(0, coinGoal);
        }
        if (starGoal != 0)
        {
            starGoalFin = false;
            SceneItemManager.Instance.UI.UpdateStarGoal(0, starGoal);
        }
        if (foodGoal != 0)
        {
            foodGoalFin = false;
            SceneItemManager.Instance.UI.UpdateFoodGoal(0, foodGoal);
        }
        if (drinkGoal != 0)
        {
            drinkGoalFin = false;
            SceneItemManager.Instance.UI.UpdateDrinkGoal(0, drinkGoal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            remainTime -= GameItem.PlaySpeed * Time.deltaTime;
            SceneItemManager.Instance.UI.UpdateTimeSlider(remainTime/initTime);
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
