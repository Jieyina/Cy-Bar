using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryButton : MonoBehaviour
{
    [SerializeField]
    private string receipeName = null;
    [SerializeField]
    [Tooltip("1 for food, 2 for drink")]
    private int type = 1;
    [SerializeField]
    [Tooltip("required material names")]
    private List<string> materials = null;
    [SerializeField]
    [Tooltip("corresponding amount")]
    private List<int> amount = null;
    [SerializeField]
    private float produceTime = 5f;
    [SerializeField]
    private int price = 5;
    [SerializeField]
    private int buildCost = 5;
    [SerializeField]
    private GameObject shadePrefab = null;
    [SerializeField]
    private GameObject item = null;
    [SerializeField]
    private Text priceText = null;
    [SerializeField]
    private Text productionText = null;
    [SerializeField]
    private Transform matDescription = null;

    private GameObject shadow;
    private RaycastHit hit;

    private Receipe CreateReceipe()
    {
        Receipe rec;
        if (type == 1)
        {
            rec = SceneItemManager.Instance.Factory.LearnedFood(receipeName);
            if (rec == null)
            {
                rec = new Receipe(receipeName, type);
                for (int i = 0; i < materials.Count; i++)
                {
                    rec.AddIngredient(materials[i], amount[i]);
                }
                SceneItemManager.Instance.Factory.AddFood(rec);
            }
        }
        else
        {
            rec = SceneItemManager.Instance.Factory.LearnedDrink(receipeName);
            if (rec == null)
            {
                rec = new Receipe(receipeName, type);
                for (int i = 0; i < materials.Count; i++)
                {
                    rec.AddIngredient(materials[i], amount[i]);
                }
                SceneItemManager.Instance.Factory.AddDrink(rec);
            }
        }
        return rec;
    }

    public void BuildFactory()
    {
        if (!shadow)
        {
            shadow = Instantiate(shadePrefab);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = buildCost.ToString();
        productionText.text = "Price " + price + " / " + produceTime + " s";
        matDescription.GetChild(0).GetComponent<Text>().text = receipeName;
        for (int i = 0; i < materials.Count; i++)
        {
            GameObject matText = matDescription.GetChild(i+1).gameObject;
            matText.SetActive(true);
            matText.GetComponent<Text>().text = materials[i] + " x " + amount[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //right click to destroy
        if (Input.GetMouseButtonDown(1) && shadow)
        {
            Destroy(shadow);
            shadow = null;
        }

        //follow mouse movement
        if (shadow)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 50000f, 1 << 10))
            {
                shadow.transform.Find("highlight").gameObject.SetActive(true);
                shadow.transform.position = hit.transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                    Receipe rec = CreateReceipe();
                    GameObject newItem = Instantiate(item, shadow.transform.position, shadow.transform.rotation);
                    newItem.transform.parent = hit.transform;
                    newItem.GetComponent<FactoryItem>().SetReceipe(rec,produceTime,price,buildCost);
                    hit.transform.gameObject.layer = 0;
                    SceneItemManager.Instance.Player.SpendMoney(buildCost);
                }
            }
            else
            {
                shadow.transform.Find("highlight").gameObject.SetActive(false);
                Vector3 pos = Input.mousePosition;
                pos.z = 10f;
                shadow.transform.position = Camera.main.ScreenToWorldPoint(pos);
            }
        }
    }
}
