using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receipe
{
    public string ReceipeName { get; }
    public int Type { get; }
    public Dictionary<string, int> Ingredients { get; }

    public Receipe(string str, int t)
    {
        ReceipeName = str;
        Type = t;
        Ingredients = new Dictionary<string, int>();
    }

    public void AddIngredient(string str, int num)
    {
        Ingredients.Add(str,num);
    }

    public void PrintIngredients()
    {
        foreach (KeyValuePair<string,int> pair in Ingredients)
        {
            Debug.Log(pair.Key + " x " +pair.Value.ToString());
        }
    }
}
