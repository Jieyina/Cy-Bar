using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    private Dictionary<string, int> rawMat = new Dictionary<string, int>();
    private List<GameObject> growedItems = new List<GameObject>();

    public void AddGrowedItem(GameObject item)
    {
        growedItems.Add(item);
    }

    public void AddMaterial(string str, int num)
    {
        if (rawMat.ContainsKey(str))
            rawMat[str] += num;
        else
            rawMat.Add(str, num);
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
