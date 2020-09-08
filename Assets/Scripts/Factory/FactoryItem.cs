﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryItem : MonoBehaviour
{
    private static Receipe receipe;
    private float produceTime;
    private int profit;
    private float chargeRate = 0.4f;

    private bool producing = false;
    private float startTime;
    private KeyValuePair<Receipe, GameObject> orderPair;

    public void SetReceipe(Receipe rec, float time, int prof)
    {
        receipe = rec;
        produceTime = time;
        profit = prof;
    }

    public void StartProduction(KeyValuePair<Receipe, GameObject> pair)
    {
        producing = true;
        startTime = Time.time;
        orderPair = pair;
        Debug.Log("start produce" + pair.Key.ReceipeName);
    }

    public void StopProduction()
    {
        Debug.Log("stop produce" + orderPair.Key.ReceipeName);
        producing = false;
        SceneManager.Instance.Factory.AddFactory(receipe, gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.Instance.Factory.AddFactory(receipe,gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (producing && Time.time - startTime > produceTime)
        {
            producing = false;
            SceneManager.Instance.Factory.FinishProduction(orderPair, gameObject);
            //SceneManager.Instance.Bar.AddBill(orderPair, (int)(profit * (1 - chargeRate) + 0.5f));
            SceneManager.Instance.Bar.FinishOrder(orderPair, (int)(profit * (1 - chargeRate) + 0.5f));
        }
    }
}
