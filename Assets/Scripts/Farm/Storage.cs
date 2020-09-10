using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    [SerializeField]
    private int initCapacity = 16;
    [SerializeField]
    private Text capText = null;
    [SerializeField]
    private Transform storageUI = null;

    private int capacity;
    private Dictionary<string, int> rawMat = new Dictionary<string, int>();
    private int stored = 0;

    public bool CheckCapacity()
    {
        return stored < capacity;
    }

    public void AddMaterial(string str, int num)
    {
        Transform matIcon = storageUI.Find(str);
        if (rawMat.ContainsKey(str))
            rawMat[str] += num;
        else
        {
            rawMat.Add(str, num);
            matIcon.gameObject.SetActive(true);
        }
        matIcon.Find("ItemCount").GetComponent<Text>().text = rawMat[str].ToString();
        stored += num;
    }

    public bool CheckEnoughMaterial(Receipe rec)
    {
        foreach (var pair in rec.Ingredients)
        {
            if (!rawMat.ContainsKey(pair.Key) || rawMat[pair.Key] < pair.Value)
                return false;
        }
        return true;
    }

    public void ChangeCapacity(int newCap)
    {
        capacity = newCap;
        capText.text = capacity.ToString();
    }

    public void ConsumeMaterial(Receipe rec)
    {
        foreach (var pair in rec.Ingredients)
        {
            rawMat[pair.Key] -= pair.Value;
            storageUI.Find(pair.Key).Find("ItemCount").GetComponent<Text>().text = rawMat[pair.Key].ToString();
            stored -= pair.Value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        capacity = initCapacity;
        capText.text = capacity.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
