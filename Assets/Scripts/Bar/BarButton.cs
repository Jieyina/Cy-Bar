﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarButton : MonoBehaviour
{
    [SerializeField]
    private int cost = 5;
    [SerializeField]
    private GameObject shadePrefab = null;
    [SerializeField]
    private GameObject item = null;
    [SerializeField]
    private Text priceText = null;

    private GameObject shadow;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = cost.ToString();
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
                    newItem.transform.parent = hit.transform;
                    newItem.GetComponent<BarTable>().SetCost(cost);
                    hit.transform.gameObject.layer = 0;
                    SceneItemManager.Instance.Player.SpendMoney(cost);
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

    public void CreateItem()
    {
        if (!shadow)
        {
            shadow = Instantiate(shadePrefab);
        }
    }
}
