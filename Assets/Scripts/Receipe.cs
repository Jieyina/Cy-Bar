using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receipe : ScriptableObject
{
    private string receipeName;
    private int type;
    private Dictionary<string, int> ingredients;

    public Receipe(string str, int t)
    {
        receipeName = str;
        type = t;
        ingredients = new Dictionary<string, int>();
    }

    public void addIngredient(string str, int num)
    {
        ingredients.Add(str,num);
    }
}
