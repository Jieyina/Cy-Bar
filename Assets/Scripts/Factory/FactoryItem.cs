using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryItem : GameItem
{
    [SerializeField]
    private TextMesh progress = null;

    private Receipe receipe;
    private float produceTime;
    private int profit;
    private int buildCost;
    private static float chargeRate = 0.4f;

    private bool producing = false;
    private float remainTime;
    private KeyValuePair<Receipe, GameObject> orderPair;
    private bool transmitting = false;
    private Transform product;
    private Vector3 initProductPos;
    private GameObject line;
    private LineRenderer lineRenderer;
    private Transform destination;

    public void SetReceipe(Receipe rec, float time, int prof, int cost)
    {
        receipe = rec;
        produceTime = time;
        profit = prof;
        buildCost = cost;
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
        if (transmitting)
        {
            StopAllCoroutines();
            product.gameObject.SetActive(false);
            product.position = initProductPos;
            line.SetActive(false);
        }
        else
        {
            producing = false;
            progress.text = "0%";
        }
        SceneItemManager.Instance.Factory.AddIdleFactory(receipe, gameObject);
    }

    private IEnumerator FinishProduction()
    {
        transmitting = true;
        product.gameObject.SetActive(true);
        line.SetActive(true);
        float progress = 0f;
        destination = orderPair.Value.transform.Find("receivePoint");
        while (Vector3.Distance(product.position, destination.position) > 0.2f)
        {
            product.position = Vector3.Lerp(initProductPos, destination.position, progress);
            progress += SceneItemManager.Instance.Player.PlaySpeed* Time.deltaTime;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, orderPair.Value.transform.position);
            yield return null;
        }
        product.gameObject.SetActive(false);
        product.position = initProductPos;
        line.SetActive(false);
        transmitting = false;
        SceneItemManager.Instance.Factory.FinishProduction(orderPair);
        orderPair.Value.GetComponent<Customer>().GetOrder(orderPair, (int)(profit * (1 - chargeRate) + 0.5f));
    }

    public override void DestroyItem()
    {
        if (producing || transmitting)
        {
            StopAllCoroutines();
            SceneItemManager.Instance.Factory.RemoveWorkingFac(orderPair);
            SceneItemManager.Instance.Bar.AddOrder(orderPair);
        }
        else
        {
            SceneItemManager.Instance.Factory.RemoveIdleFactory(receipe, gameObject);
        }
        SceneItemManager.Instance.Player.GainMoney((int)Mathf.Floor(buildCost*0.5f));
        transform.parent.gameObject.layer = 10;
        base.DestroyItem();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        product = transform.Find("product");
        initProductPos = product.position;
        line = transform.Find("line").gameObject;
        lineRenderer = line.GetComponent<LineRenderer>();
        SceneItemManager.Instance.Factory.AddIdleFactory(receipe,gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (producing)
        {
            remainTime -= SceneItemManager.Instance.Player.PlaySpeed * Time.deltaTime;
            if (remainTime < 0)
            {
                producing = false;
                progress.text = "0%";
                //SceneItemManager.Instance.Factory.FinishProduction(orderPair);
                //orderPair.Value.GetComponent<Customer>().GetOrder(orderPair, (int)(profit * (1 - chargeRate) + 0.5f));
                StartCoroutine(FinishProduction());
            }
            else
                progress.text = Mathf.Floor((produceTime - remainTime) / produceTime * 100).ToString() + "%";
        }
    }
}
