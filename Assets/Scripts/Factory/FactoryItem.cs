using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryItem : GameItem
{
    [SerializeField]
    private TextMesh progress = null;

    private static Receipe receipe;
    private float produceTime;
    private int profit;
    private static float chargeRate = 0.4f;

    private bool producing = false;
    private float remainTime;
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
        remainTime = produceTime;
        orderPair = pair;
        progress.text = "0%";
    }

    public void StopProduction()
    {
        producing = false;
        progress.text = "0%";
        SceneManager.Instance.Factory.AddFactory(receipe, gameObject);
    }

    private IEnumerator clearProgress()
    {
        yield return new WaitForSeconds(0.5f);
        progress.text = "0%";
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SceneManager.Instance.Factory.AddFactory(receipe,gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (producing)
        {
            remainTime -= playSpeed * Time.deltaTime;
            if (remainTime < 0)
            {
                producing = false;
                progress.text = "100%";
                StartCoroutine(clearProgress());
                SceneManager.Instance.Factory.FinishProduction(orderPair);
                orderPair.Value.GetComponent<Customer>().GetOrder(orderPair, (int)(profit * (1 - chargeRate) + 0.5f));
            }
            else
                progress.text = Mathf.Floor((produceTime - remainTime) / produceTime * 100).ToString() + "%";
        }
    }
}
