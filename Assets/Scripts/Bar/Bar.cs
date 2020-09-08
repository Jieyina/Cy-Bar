using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private List<KeyValuePair<Receipe, GameObject>> orderedDish = new List<KeyValuePair<Receipe, GameObject>>();
    private Dictionary<GameObject, List<Receipe>> orders = new Dictionary<GameObject, List<Receipe>>();

    public void AddOrder(KeyValuePair<Receipe, GameObject> pair)
    {
        orderedDish.Add(pair);
        if (!orders.ContainsKey(pair.Value))
            orders.Add(pair.Value, new List<Receipe>());
        orders[pair.Value].Add(pair.Key);
    }

    public void RemoveOrder(KeyValuePair<Receipe, GameObject> pair)
    {
        if (SceneManager.Instance.Factory.CheckProduction(pair))
            SceneManager.Instance.Factory.StopProduction(pair);
        else
        {
            orderedDish.Remove(pair);
            orders[pair.Value].Remove(pair.Key);
            if (orders[pair.Value].Count == 0)
                orders.Remove(pair.Value);
        }
    }

    private void CheckOrderCompletion()
    {
        if (orderedDish.Count != 0)
        {
            foreach (KeyValuePair<Receipe, GameObject> pair in orderedDish.ToArray())
            {
                if (SceneManager.Instance.Storage.CheckEnoughMaterial(pair.Key) && SceneManager.Instance.Factory.HasIdleFactory(pair.Key))
                {
                    SceneManager.Instance.Storage.ConsumeMaterial(pair.Key);
                    SceneManager.Instance.Factory.StartProduction(pair);
                    orderedDish.Remove(pair);
                    orders[pair.Value].Remove(pair.Key);
                    if (orders[pair.Value].Count == 0)
                        orders.Remove(pair.Value);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckOrderCompletion();
    }
}
