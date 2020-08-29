using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private List<Receipe> learnedFood = new List<Receipe>();
    private List<Receipe> learnedDrink = new List<Receipe>();
    private List<KeyValuePair<GameObject,Receipe>> requiredDish = new List<KeyValuePair<GameObject, Receipe>>();

    public void AddFood(Receipe rec)
    {
        Debug.Log("Add receipe " + rec.ReceipeName);
        rec.PrintIngredients();
        learnedFood.Add(rec);
    }

    public void AddDrink(Receipe rec)
    {
        learnedDrink.Add(rec);
    }

    public Receipe GetRandomFood()
    {
        if (learnedFood.Count != 0)
        {
            int index = Random.Range(0, learnedFood.Count - 1);
            return learnedFood[index];
        }
        else
            return null;
    }

    public Receipe GetRandomDrink()
    {
        if (learnedDrink.Count != 0)
        {
            int index = Random.Range(0, learnedDrink.Count - 1);
            return learnedDrink[index];
        }
        else
            return null;
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
