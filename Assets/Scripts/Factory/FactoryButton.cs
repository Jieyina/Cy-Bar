using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject shadePrefab;
    public GameObject item;

    private GameObject shadow;
    private RaycastHit hit;

    private Receipe CreateReceipe()
    {
        Receipe rec;
        if (type == 1)
        {
            rec = SceneManager.Instance.Factory.LearnedFood(receipeName);
            if (rec == null)
            {
                rec = new Receipe(receipeName, type);
                for (int i = 0; i < materials.Count; i++)
                {
                    rec.AddIngredient(materials[i], amount[i]);
                }
                SceneManager.Instance.Factory.AddFood(rec);
            }
        }
        else
        {
            rec = SceneManager.Instance.Factory.LearnedDrink(receipeName);
            if (rec == null)
            {
                rec = new Receipe(receipeName, type);
                for (int i = 0; i < materials.Count; i++)
                {
                    rec.AddIngredient(materials[i], amount[i]);
                }
                SceneManager.Instance.Factory.AddDrink(rec);
            }
        }
        return rec;
    }

    public void BuildFactory()
    {
        if (!shadow && SceneManager.Instance.Player.canAfford(buildCost))
        {
            shadow = Instantiate(shadePrefab);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
                    newItem.transform.parent = hit.transform.parent.parent;
                    newItem.GetComponent<FactoryItem>().SetReceipe(rec,produceTime,price);
                    hit.transform.gameObject.layer = 0;
                    SceneManager.Instance.Player.spendMoney(buildCost);
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
