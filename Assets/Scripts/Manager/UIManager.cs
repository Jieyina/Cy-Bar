using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text moneyText = null;
    [SerializeField]
    private Text starText = null;
    [SerializeField]
    private Transform brush = null;
    [SerializeField]
    private Slider timeSlider = null;
    [SerializeField]
    private Text coinGoalText = null;
    [SerializeField]
    private Text starGoalText = null;
    [SerializeField]
    private Text foodGoalText = null;
    [SerializeField]
    private Text drinkGoalText = null;
    [SerializeField]
    private GameObject winUI = null;
    [SerializeField]
    private GameObject failUI = null;

    private Vector3 initBrushPos;

    public void UpdateMoney(int num)
    {
        moneyText.text = num.ToString();
    }

    public void UpdateStar(int num)
    {
        starText.text = num.ToString();
    }

    public void UpdateTimeSlider(float value)
    {
        timeSlider.value = value;
    }

    public void UpdateCoinGoal(int cur, int goal)
    {
        coinGoalText.text = "☐ Earn " + cur.ToString() +"/" + goal.ToString() + " Bitcoins";
    }

    public void UpdateStarGoal(int cur, int goal)
    {
        starGoalText.text = "☐ Earn " + cur.ToString() + "/" + goal.ToString() + " Stars";
    }

    public void UpdateFoodGoal(int cur, int goal)
    {
        foodGoalText.text = "☐ Serve " + cur.ToString() + "/" + goal.ToString() + " Food";
    }

    public void UpdateDrinkGoal(int cur, int goal)
    {
        drinkGoalText.text = "☐ Serve " + cur.ToString() + "/" + goal.ToString() + " Drink";
    }

    public void CheckCoinGoal()
    {
        coinGoalText.transform.Find("YesMark").gameObject.SetActive(true);
    }

    public void CheckStarGoal()
    {
        starGoalText.transform.Find("YesMark").gameObject.SetActive(true);
    }

    public void CheckFoodGoal()
    {
        foodGoalText.transform.Find("YesMark").gameObject.SetActive(true);
    }

    public void CheckDrinkGoal()
    {
        drinkGoalText.transform.Find("YesMark").gameObject.SetActive(true);
    }

    public void UpdateBrushPos()
    {
        brush.position = Input.mousePosition;
    }

    public void RestoreBrushPos()
    {
        brush.localPosition = initBrushPos;
    }

    public void ShowWinUI()
    {
        winUI.SetActive(true);
    }

    public void ShowFailUI()
    {
        failUI.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        initBrushPos = brush.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
