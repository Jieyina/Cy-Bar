using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmButton : MonoBehaviour
{
    [SerializeField]
    private string matName = null;
    [SerializeField]
    private int produceNum = 1;
    [SerializeField]
    private float growTime = 5f;
    [SerializeField]
    private int authorizationCost = 5;
    [SerializeField]
    private int activationCost = 5;
    [SerializeField]
    private GameObject shadePrefab = null;
    [SerializeField]
    private GameObject item = null;

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
                    newItem.transform.parent = hit.transform;
                    newItem.GetComponent<FarmItem>().SetProps(matName, produceNum, growTime, activationCost, authorizationCost);
                    hit.transform.gameObject.layer = 0;
                    SceneItemManager.Instance.Player.SpendMoney(authorizationCost);
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

    public void CreateSeed()
    {
        if (!shadow)
        {
            shadow = Instantiate(shadePrefab);
        }
    }
}
