using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private List<Receipe> food = new List<Receipe>();
    private List<Receipe> drink = new List<Receipe>();
    private Dictionary<Receipe, List<GameObject>> idleFactories = new Dictionary<Receipe, List<GameObject>>();
    private Dictionary<KeyValuePair<Receipe, GameObject>,GameObject> workingFacs = new Dictionary<KeyValuePair<Receipe, GameObject>,GameObject>();

    public Receipe LearnedFood(string name)
    {
        foreach (Receipe rece in food)
        {
            if (rece.ReceipeName == name)
                return rece;
        }
        return null;
    }

    public Receipe LearnedDrink(string name)
    {
        foreach (Receipe rece in drink)
        {
            if (rece.ReceipeName == name)
                return rece;
        }
        return null;
    }

    public void AddIdleFactory(Receipe receipe, GameObject fac)
    {
        if (!idleFactories.ContainsKey(receipe))
        {
            idleFactories.Add(receipe, new List<GameObject>());
        }
        idleFactories[receipe].Add(fac);
    }

    public void RemoveIdleFactory(Receipe receipe, GameObject fac)
    {
        idleFactories[receipe].Remove(fac);
    }

    public void RemoveWorkingFac(KeyValuePair<Receipe, GameObject> pair)
    {
        workingFacs.Remove(pair);
    }

    public void AddFood(Receipe rec)
    {
        //Debug.Log("new food " + rec.ReceipeName);
        food.Add(rec);
    }

    public void AddDrink(Receipe rec)
    {
        //Debug.Log("new drink " + rec.ReceipeName);
        drink.Add(rec);
    }

    public Receipe GetRandomFood()
    {
        if (food.Count != 0)
        {
            int index = Random.Range(0, food.Count);
            return food[index];
        }
        else
            return null;
    }

    public Receipe GetRandomDrink()
    {
        if (drink.Count != 0)
        {
            int index = Random.Range(0, drink.Count);
            return drink[index];
        }
        else
            return null;
    }

    public bool HasIdleFactory(Receipe rec)
    {
        return idleFactories[rec].Count > 0;
    }

    public void StartProduction(KeyValuePair<Receipe, GameObject> pair)
    {
        GameObject factory = idleFactories[pair.Key][0];
        idleFactories[pair.Key].Remove(factory);
        factory.GetComponent<FactoryItem>().StartProduction(pair);
        workingFacs.Add(pair,factory);
    }

    public bool CheckProduction(KeyValuePair<Receipe, GameObject> pair)
    {
        return workingFacs.ContainsKey(pair);
    }

    public void StopProduction(KeyValuePair<Receipe, GameObject> pair)
    {
        workingFacs[pair].GetComponent<FactoryItem>().StopProduction();
        workingFacs.Remove(pair);
    }

    public void FinishProduction(KeyValuePair<Receipe, GameObject> pair)
    {
        idleFactories[pair.Key].Add(workingFacs[pair]);
        workingFacs.Remove(pair);
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
