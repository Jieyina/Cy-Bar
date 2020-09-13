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

    private int playSpeed = 1;
    public int PlaySpeed { get { return playSpeed; } set { playSpeed = value; } }

    private bool destroy = false;
    private RaycastHit hit;

    public void SpendMoney(int cost)
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
            SceneItemManager.Instance.UI.UpdateCoinGoal(coinEarned, coinGoal);
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

    public bool CanAfford(int cost)
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

    public void SpeedUp()
    {
        playSpeed = 2;
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 2;
        }
    }

    public void RestoreSpeed()
    {
        playSpeed = 1;
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 1;
        }
    }

    public void Pause()
    {
        playSpeed = 0;
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
            remainTime -= playSpeed * Time.deltaTime;
            SceneItemManager.Instance.UI.UpdateTimeSlider(remainTime/initTime);
            if (coinGoalFin&&starGoalFin&&foodGoalFin&&drinkGoalFin)
            {
                counting = false;
                playSpeed = 0;
                SceneItemManager.Instance.UI.ShowWinUI();
            }
            if (remainTime<0)
            {
                counting = false;
                playSpeed = 0;
                SceneItemManager.Instance.UI.ShowFailUI();
            }
        }

        if (destroy)
        {
            SceneItemManager.Instance.UI.UpdateBrushPos();
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50000f, 1 << 11))
                {
                    hit.transform.parent.gameObject.GetComponent<GameItem>().DestroyItem();
                }
                SceneItemManager.Instance.UI.RestoreBrushPos();
                destroy = false;
            }
        }
    }
}
