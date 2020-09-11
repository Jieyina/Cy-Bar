using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTable : GameItem
{
    [SerializeField]
    private float maxEmptyTime= 5f;
    [SerializeField]
    private List<GameObject> customerPrefab = null;

    private bool empty;
    private float interval;
    private float startTime;
    private GameObject customer;

    // Start is called before the first frame update
    protected override void Start()
    {
        empty = true;
        interval = Random.Range(1.2f, maxEmptyTime);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (empty && Time.time - startTime > interval)
        {
            empty = false;
            int i = Random.Range(0, customerPrefab.Count);
            customer = Instantiate(customerPrefab[i], transform.Find("spawnPoint"));
            customer.GetComponent<Customer>().Table = gameObject;
        }
    }

    public void EmptyTable()
    {
        empty = true;
        interval = Random.Range(1.2f, maxEmptyTime);
        startTime = Time.time;
    }

    public override void DestroyItem()
    {
        customer.GetComponent<GameItem>().DestroyItem();
        base.DestroyItem();
    }
}
