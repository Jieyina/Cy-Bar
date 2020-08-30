using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private List<Receipe> food = new List<Receipe>();
    private List<Receipe> drink = new List<Receipe>();
    private Dictionary<Receipe, List<GameObject>> idleFactories = new Dictionary<Receipe, List<GameObject>>();

    public Receipe LearnedFood(Receipe rec)
    {
        foreach (Receipe rece in food)
        {
            if (rece.ReceipeName == rec.ReceipeName)
                return rece;
        }
        return null;
    }

    public Receipe LearnedDrink(Receipe rec)
    {
        foreach (Receipe rece in drink)
        {
            if (rece.ReceipeName == rec.ReceipeName)
                return rece;
        }
        return null;
    }

    public void AddFactory(Receipe receipe, GameObject obj)
    {
        if (!idleFactories.ContainsKey(receipe))
        {
            Debug.Log("factory of new receipe");
            idleFactories.Add(receipe, new List<GameObject>());
        }
        idleFactories[receipe].Add(obj);
    }

    public void RemoveFactory(Receipe receipe, GameObject obj)
    {
        idleFactories[receipe].Remove(obj);
    }

    public void AddFood(Receipe rec)
    {
        if (!food.Contains(rec))
        {
            Debug.Log("new receipe " + rec.ReceipeName);
            rec.PrintIngredients();
            food.Add(rec);
        }
    }

    public void AddDrink(Receipe rec)
    {
        if (!drink.Contains(rec))
            drink.Add(rec);
    }

    public Receipe GetRandomFood()
    {
        if (food.Count != 0)
        {
            int index = Random.Range(0, food.Count - 1);
            return food[index];
        }
        else
            return null;
    }

    public Receipe GetRandomDrink()
    {
        if (drink.Count != 0)
        {
            int index = Random.Range(0, drink.Count - 1);
            return drink[index];
        }
        else
            return null;
    }

    public bool HasIdleFactory(Receipe rec)
    {
        return idleFactories[rec].Count != 0;
    }

    public void StartProduction(KeyValuePair<Receipe, GameObject> pair)
    {
        GameObject factory = idleFactories[pair.Key][0];
        factory.GetComponent<FactoryItem>().StartProduction(pair);
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
