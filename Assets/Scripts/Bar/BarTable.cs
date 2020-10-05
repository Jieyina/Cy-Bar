using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTable : GameItem
{
    [SerializeField]
    private float maxEmptyTime= 5f;
    [SerializeField]
    private List<GameObject> customerPrefab = null;

    private int buildCost;

    private bool empty;
    private float emptyTime;
    private float remainTime;
    private GameObject customer;

    public void SetCost(int cost)
    {
        buildCost = cost;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        empty = true;
        emptyTime = Random.Range(1.2f, maxEmptyTime);
        remainTime = emptyTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (empty)
        {
            remainTime -= SceneItemManager.Instance.Player.PlaySpeed * Time.deltaTime;
            if (remainTime < 0)
            {
                empty = false;
                int i = Random.Range(0, customerPrefab.Count);
                customer = Instantiate(customerPrefab[i], transform.Find("spawnPoint"));
                int facing = Random.Range(0, 2);
                if (facing == 1)
                    customer.GetComponent<Customer>().FaceRight = true;
                customer.GetComponent<Customer>().Table = gameObject;
            }
        }
    }

    public void EmptyTable(bool keepTable = true)
    {
        customer = null;
        if (keepTable)
        {
            empty = true;
            emptyTime = Random.Range(1.2f, maxEmptyTime);
            remainTime = emptyTime;
        }
    }

    public override void DestroyItem()
    {

        StartCoroutine(DestroyTable());
    }

    private IEnumerator DestroyTable()
    {
        if (customer)
            customer.GetComponent<GameItem>().DestroyItem();
        while (customer != null)
            yield return null;
        SceneItemManager.Instance.Player.GainMoney((int)Mathf.Floor(buildCost * 0.5f));
        transform.parent.gameObject.layer = 8;
        base.DestroyItem();
    }
}
