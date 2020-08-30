using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryButton : MonoBehaviour
{
    public string receipeName;
    [Tooltip("1 for food, 2 for drink")]
    public int type;
    [Tooltip("required material names")]
    public List<string> materials;
    [Tooltip("corresponding amount")]
    public List<int> amount;
    public float produceTime;
    public int price;
    public int buildCost;

    public GameObject shadePrefab;
    public GameObject item;

    private GameObject shadow;
    private RaycastHit hit;

    private Receipe CreateReceipe()
    {
        Receipe rece = new Receipe(receipeName, type);
        for (int i = 0; i < materials.Count; i++)
        {
            rece.AddIngredient(materials[i], amount[i]);
        }
        Receipe rec;
        if (rece.Type == 1)
        {
            rec = SceneManager.Instance.Factory.LearnedFood(rece);
            if (rec != null)
                rece = rec;
            else
                SceneManager.Instance.Factory.AddFood(rece);
        }
        else if (rece.Type == 2)
        {
            rec = SceneManager.Instance.Factory.LearnedDrink(rece);
            if (rec != null)
                rece = rec;
            else
                SceneManager.Instance.Factory.AddDrink(rece);
        }
        return rece;
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
                    newItem.transform.parent = SceneManager.Instance.Factory.transform;
                    newItem.GetComponent<FactoryItem>().SetReceipe(rec,produceTime,price);
                    hit.transform.gameObject.layer = 0;
                    SceneManager.Instance.Player.spendMoney(buildCost);
                    Destroy(shadow);
                    shadow = null;
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
