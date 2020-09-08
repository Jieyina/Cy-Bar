using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField]
    private int initCapacity = 16;

    private int capacity;
    private Dictionary<string, int> rawMat = new Dictionary<string, int>();
    private int stored = 0;

    public bool CheckCapacity()
    {
        return stored < capacity;
    }

    public void AddMaterial(string str, int num)
    {
        if (rawMat.ContainsKey(str))
            rawMat[str] += num;
        else
            rawMat.Add(str, num);
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
    }

    public void ConsumeMaterial(Receipe rec)
    {
        foreach (var pair in rec.Ingredients)
        {
            rawMat[pair.Key] -= pair.Value;
            //Debug.Log("consumed " + pair.Key + " x " + pair.Value);
            stored -= pair.Value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        capacity = initCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
