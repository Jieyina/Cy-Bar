using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receipe
{
    public string ReceipeName { get; }
    public int Type { get; }
    private Dictionary<string, int> ingredients;
    public Dictionary<string, int> Ingredients => ingredients;

    public Receipe(string str, int t)
    {
        ReceipeName = str;
        Type = t;
        ingredients = new Dictionary<string, int>();
    }

    public void AddIngredient(string str, int num)
    {
        ingredients.Add(str,num);
    }

    public void PrintIngredients()
    {
        foreach (KeyValuePair<string,int> pair in ingredients)
        {
            Debug.Log(pair.Key + " x " +pair.Value.ToString());
        }
    }
}
