using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTable : MonoBehaviour
{
    public float maxEmptyTime;
    public GameObject customerPrefab;

    private bool empty;
    private float interval;
    private float startTime;
    private GameObject customer;

    // Start is called before the first frame update
    void Start()
    {
        empty = true;
        interval = Random.Range(1f, maxEmptyTime);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (empty && Time.time - startTime > interval)
        {
            empty = false;
            customer = Instantiate(customerPrefab, transform.Find("spawnPoint"));
            Receipe foodOrder = SceneManager.Instance.Factory.GetRandomFood();
            if (foodOrder != null)
            {
                Debug.Log("ordered " + foodOrder.ReceipeName);
                SceneManager.Instance.Bar.AddOrder(foodOrder, gameObject);
            }
            Receipe drinkOrder = SceneManager.Instance.Factory.GetRandomDrink();
            if (drinkOrder != null)
            {
                SceneManager.Instance.Bar.AddOrder(drinkOrder, gameObject);
            }
        }
    }

    public void EmptyTable()
    {
        Destroy(customer);
        customer = null;
        empty = true;
        interval = Random.Range(1f, maxEmptyTime);
        startTime = Time.time;
    }
}
