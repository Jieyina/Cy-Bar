using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceipeButton : MonoBehaviour
{
    public string receipeName;
    [Tooltip("1 for food, 2 for drink")]
    public int type;
    [Tooltip("required material names")]
    public List<string> materials;
    [Tooltip("corresponding amount")]
    public List<int> amount;
    public int cost;

    public void createReceipe()
    {
        if (SceneManager.Instance.Player.canAfford(cost))
        {
            SceneManager.Instance.Player.spendMoney(cost);
            Receipe rece = new Receipe(receipeName, type);
            for (int i = 0; i < materials.Count; i++)
            {
                rece.AddIngredient(materials[i], amount[i]);
            }
            if (rece.Type == 1)
                SceneManager.Instance.Factory.AddFood(rece);
            else if (rece.Type == 2)
                SceneManager.Instance.Factory.AddDrink(rece);
            Destroy(gameObject);
        }
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
