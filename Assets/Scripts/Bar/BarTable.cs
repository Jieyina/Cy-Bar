using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTable : MonoBehaviour
{
    [SerializeField]
    private float maxEmptyTime= 5f;
    [SerializeField]
    private GameObject customerPrefab = null;

    private bool empty;
    private float interval;
    private float startTime;

    // Start is called before the first frame update
    void Start()
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
            GameObject customer = Instantiate(customerPrefab, transform.Find("spawnPoint"));
            customer.GetComponent<Customer>().Table = gameObject;
        }
    }

    public void EmptyTable()
    {
        empty = true;
        interval = Random.Range(1.2f, maxEmptyTime);
        startTime = Time.time;
    }
}
