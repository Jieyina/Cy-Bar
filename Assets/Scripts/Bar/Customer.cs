using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : GameItem
{
    [SerializeField]
    private Animator customerAnim = null;
    [SerializeField]
    private float waitTime = 60f;
    [SerializeField]
    private Text countDownText = null;
    [SerializeField]
    private Slider timeSlider = null;
    [SerializeField]
    private Animator moneyAnim = null;
    [SerializeField]
    private Text moneyText = null;
    [SerializeField]
    private Animator starAnim = null;
    [SerializeField]
    private Text starText = null;
    [SerializeField]
    private Animator leaveAnim = null;
    [SerializeField]
    private List<SpriteRenderer> bodySprites = null;
    [SerializeField]
    private GameObject foodLeft = null;
    [SerializeField]
    private GameObject foodRight = null;

    private GameObject table;
    public GameObject Table { set { table = value; } }
    private bool faceRight = false;
    public bool FaceRight { set { faceRight = value; } }

    private bool waiting;
    private float remainTime;
    private KeyValuePair<Receipe, GameObject> order1;
    private KeyValuePair<Receipe, GameObject> order2;
    private bool waitOrder1 = false;
    private bool waitOrder2 = false;
    private int payment = 0;

    public void GetOrder(KeyValuePair<Receipe, GameObject> order, int bill)
    {
        if (!order1.Equals(default(KeyValuePair<Receipe, GameObject>)) && order.Key.ReceipeName == order1.Key.ReceipeName)
        {
            waitOrder1 = false;
            SceneItemManager.Instance.Player.GetFoodServe(1);
        }
        else
        {
            waitOrder2 = false;
            SceneItemManager.Instance.Player.GetDrinkServe(1);
        }
        payment += bill;
        if (!waitOrder1 && !waitOrder2)
        {
            waiting = false;
            StartCoroutine(FinishOrder());
        }
        else
            StartCoroutine(StartEat());
    }

    private IEnumerator StartEat()
    {
        if (faceRight)
            foodRight.SetActive(true);
        else
            foodLeft.SetActive(true);
        customerAnim.SetTrigger("eat");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("eat"))
            yield return null;
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("hold"))
            yield return null;
        if (faceRight)
            foodRight.SetActive(false);
        else
            foodLeft.SetActive(false);
    }

    private IEnumerator FinishOrder()
    {
        timeSlider.gameObject.SetActive(false);
        if (faceRight)
            foodRight.SetActive(true);
        else
            foodLeft.SetActive(true);
        customerAnim.SetTrigger("eat");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("eat"))
            yield return null;
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("hold"))
            yield return null;
        if (faceRight)
            foodRight.SetActive(false);
        else
            foodLeft.SetActive(false);
        int rating = (int)Mathf.Floor(remainTime / waitTime * 5) + 1;
        if (rating == 6) rating = 5;
        starText.text = "+ " + rating.ToString();
        moneyText.text = "+ " + payment.ToString();
        moneyAnim.SetTrigger("start");
        starAnim.SetTrigger("start");
        SceneItemManager.Instance.Player.GainMoney(payment,true);
        SceneItemManager.Instance.Player.GainStar(rating);
        while (!starAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
            yield return null;
        customerAnim.SetTrigger("leave");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
            yield return null;
        table.GetComponent<BarTable>().EmptyTable();
        Destroy(gameObject);
    }

    private IEnumerator Leave()
    {
        timeSlider.gameObject.SetActive(false);
        leaveAnim.SetTrigger("start");
        starText.text = "- 1";
        starAnim.SetTrigger("start");
        SceneItemManager.Instance.Player.GainStar(-1);
        while (!starAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
            yield return null;
        customerAnim.SetTrigger("leave");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
            yield return null;
        table.GetComponent<BarTable>().EmptyTable();
        Destroy(gameObject);
    }

    private IEnumerator NoOrderLeave()
    {
        timeSlider.gameObject.SetActive(false);
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("hold"))
            yield return null;
        customerAnim.SetTrigger("leave");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
            yield return null;
        table.GetComponent<BarTable>().EmptyTable();
        Destroy(gameObject);
    }

    public override void DestroyItem()
    {
        timeSlider.gameObject.SetActive(false);
        StopAllCoroutines();
        if (waitOrder1)
            SceneItemManager.Instance.Bar.RemoveOrder(order1);
        if (waitOrder2)
            SceneItemManager.Instance.Bar.RemoveOrder(order2);
        StartCoroutine(KickOut());
    }

    private IEnumerator KickOut()
    {
        if (starAnim.GetCurrentAnimatorStateInfo(0).IsTag("star"))
        {
            while (!starAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
                yield return null;
        }
        leaveAnim.SetTrigger("kickOut");
        starText.text = "- 5";
        starAnim.SetTrigger("kickOut");
        SceneItemManager.Instance.Player.GainStar(-5);
        while (!starAnim.GetCurrentAnimatorStateInfo(0).IsTag("star"))
            yield return null;
        while (!starAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
            yield return null;
        if (faceRight)
            foodRight.SetActive(false);
        else
            foodLeft.SetActive(false);
        customerAnim.SetTrigger("kickOut");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("end"))
            yield return null;
        table.GetComponent<BarTable>().EmptyTable(false);
        base.DestroyItem();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (faceRight)
        {
            foreach (SpriteRenderer sprite in bodySprites)
                sprite.flipX = true;
        }
        Receipe foodOrder = SceneItemManager.Instance.Factory.GetRandomFood();
        if (foodOrder != null)
        {
            order1 = new KeyValuePair<Receipe, GameObject>(foodOrder, gameObject);
            waitOrder1 = true;
            SceneItemManager.Instance.Bar.AddOrder(order1);
        }
        Receipe drinkOrder = SceneItemManager.Instance.Factory.GetRandomDrink();
        if (drinkOrder != null)
        {
            order2 = new KeyValuePair<Receipe, GameObject>(drinkOrder, gameObject);
            waitOrder2 = true;
            SceneItemManager.Instance.Bar.AddOrder(order2);
        }
        if (foodOrder==null&&drinkOrder==null)
        {
            StartCoroutine(NoOrderLeave());
            return;
        }
        waiting = true;
        remainTime = waitTime;
        countDownText.text = ((int)remainTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            remainTime -= SceneItemManager.Instance.Player.PlaySpeed * Time.deltaTime;
            if (remainTime < 0)
            {
                waiting = false;
                timeSlider.value = 0;
                countDownText.text = "0";
                if (waitOrder1)
                {
                    SceneItemManager.Instance.Bar.RemoveOrder(order1);
                    waitOrder1 = false;
                }
                if (waitOrder2)
                {
                    SceneItemManager.Instance.Bar.RemoveOrder(order2);
                    waitOrder2 = false;
                }
                StartCoroutine(Leave());
            }
            else
            {
                timeSlider.value = remainTime / waitTime;
                countDownText.text = Mathf.Ceil(remainTime).ToString();
            }
        }
    }
}
