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

    // Start is called before the first frame update
    void Start()
    {
        empty = true;
        interval = Random.Range(0.1f, maxEmptyTime);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (empty && Time.time - startTime > interval)
        {
            empty = false;
            GameObject customer = Instantiate(customerPrefab, transform.Find("spawnPoint"));
            Receipe foodOrder = SceneManager.Instance.Factory.GetRandomFood();
        }
    }
}
