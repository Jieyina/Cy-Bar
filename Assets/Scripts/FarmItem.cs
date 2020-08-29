using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmItem : MonoBehaviour
{
    private string matName;
    private int amount;
    private float produceTime;

    public void setProps(string str, int num, float time)
    {
        matName = str;
        amount = num;
        produceTime = time;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
