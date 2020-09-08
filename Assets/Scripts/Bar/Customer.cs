using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
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
    private Text MoneyText = null;
    [SerializeField]
    private Animator leaveAnim = null;
    [SerializeField]
    private GameObject foodLeft = null;
    [SerializeField]
    private GameObject foodRight = null;

    private GameObject table;
    public GameObject Table { set { table = value; } }

    private bool waiting;
    private float startWaitTime;
    private KeyValuePair<Receipe, GameObject> order1;
    private KeyValuePair<Receipe, GameObject> order2;
    private bool waitOrder1 = false;
    private bool waitOrder2 = false;
    private int payment = 0;

    public void getOrder(KeyValuePair<Receipe, GameObject> order, int bill)
    {
        if (order.Key.ReceipeName == order1.Key.ReceipeName)
            waitOrder1 = false;
        else waitOrder2 = false;
        payment += bill;
        if (!waitOrder1 && !waitOrder2)
        {
            waiting = false;
            StartCoroutine(finishOrder());
        }
        else
            StartCoroutine(startEat());
    }

    private IEnumerator startEat()
    {
        foodLeft.SetActive(true);
        customerAnim.SetTrigger("eat");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("hold"))
            yield return null;
        foodLeft.SetActive(false);
    }

    private IEnumerator finishOrder()
    {
        Debug.Log("eat");
        foodLeft.SetActive(true);
        customerAnim.SetTrigger("eat");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("eat"))
            yield return null;
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsTag("hold"))
            yield return null;
        foodLeft.SetActive(false);
        Debug.Log("pay");
        MoneyText.text = "+ " + payment.ToString();
        moneyAnim.SetTrigger("start");
        while (!moneyAnim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            yield return null;
        SceneManager.Instance.Player.GainMoney(payment);
        customerAnim.SetTrigger("leave");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            yield return null;
        table.GetComponent<BarTable>().EmptyTable();
        Destroy(gameObject);
    }

    private IEnumerator Leave()
    {
        leaveAnim.SetTrigger("start");
        while (!leaveAnim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            yield return null;
        customerAnim.SetTrigger("leave");
        while (!customerAnim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            yield return null;
        table.GetComponent<BarTable>().EmptyTable();
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Receipe foodOrder = SceneManager.Instance.Factory.GetRandomFood();
        if (foodOrder != null)
        {
            Debug.Log("ordered " + foodOrder.ReceipeName);
            order1 = new KeyValuePair<Receipe, GameObject>(foodOrder, gameObject);
            waitOrder1 = true;
            SceneManager.Instance.Bar.AddOrder(order1);
        }
        Receipe drinkOrder = SceneManager.Instance.Factory.GetRandomDrink();
        if (drinkOrder != null)
        {
            Debug.Log("ordered " + drinkOrder.ReceipeName);
            order2 = new KeyValuePair<Receipe, GameObject>(drinkOrder, gameObject);
            waitOrder2 = true;
            SceneManager.Instance.Bar.AddOrder(order2);
        }
        waiting = true;
        startWaitTime = Time.time;
        countDownText.text = ((int)waitTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            float timePast = Time.time - startWaitTime;
            if (timePast > waitTime)
            {
                waiting = false;
                timeSlider.value = 0;
                countDownText.text = "0";
                if (!order1.Equals(default(KeyValuePair<Receipe,GameObject>)))
                    SceneManager.Instance.Bar.RemoveOrder(order1);
                if (!order2.Equals(default(KeyValuePair<Receipe, GameObject>)))
                    SceneManager.Instance.Bar.RemoveOrder(order2);
                StartCoroutine(Leave());
            }
            timeSlider.value = 1 - timePast / waitTime;
            countDownText.text = Mathf.Ceil(waitTime - timePast).ToString();
        }
    }
}
