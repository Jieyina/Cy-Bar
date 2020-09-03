using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmButton : MonoBehaviour
{
    [SerializeField]
    private string matName;
    [SerializeField]
    private int produceNum;
    [SerializeField]
    private float growTime;
    [SerializeField]
    private int authorizationCost;
    [SerializeField]
    private int activationCost;
    [SerializeField]
    private GameObject shadePrefab;
    [SerializeField]
    private GameObject item;

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
            if (Physics.Raycast(ray, out hit, 50000f, 1 << 9))
            {
                shadow.transform.Find("highlight").gameObject.SetActive(true);
                shadow.transform.position = hit.transform.position;
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject newItem = Instantiate(item, shadow.transform.position, shadow.transform.rotation);
                    newItem.transform.parent = GameObject.Find("Farm").transform;
                    newItem.GetComponent<FarmItem>().SetProps(matName, produceNum, growTime, activationCost);
                    hit.transform.gameObject.layer = 0;
                    SceneManager.Instance.Player.spendMoney(authorizationCost);
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
        if (!shadow && SceneManager.Instance.Player.canAfford(authorizationCost))
        {
            shadow = Instantiate(shadePrefab);
        }
    }
}
