using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmButton : MonoBehaviour
{
    public string matName;
    public int produceNum;
    public float growTime;
    public int cost;
    public GameObject shadePrefab;
    public GameObject item;

    private GameObject shadow;
    private RaycastHit hit;

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
            if (Physics.Raycast(ray, out hit, 50000f, 1 << 8))
            {
                shadow.transform.Find("highlight").gameObject.SetActive(true);
                shadow.transform.position = hit.transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject newItem = Instantiate(item, shadow.transform.position, shadow.transform.rotation);
                    newItem.transform.parent = SceneManager.Instance.Farm.transform;
                    newItem.GetComponent<FarmItem>().SetProps(matName, produceNum, growTime);
                    hit.transform.gameObject.layer = 0;
                    SceneManager.Instance.Player.spendMoney(cost);
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

    public void createSeed()
    {
        if (!shadow && SceneManager.Instance.Player.canAfford(cost))
        {
            shadow = Instantiate(shadePrefab);
        }
    }
}
