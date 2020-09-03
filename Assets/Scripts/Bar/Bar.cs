using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    private List<KeyValuePair<Receipe, GameObject>> orderedDish = new List<KeyValuePair<Receipe, GameObject>>();
    private Dictionary<GameObject, List<Receipe>> orders = new Dictionary<GameObject, List<Receipe>>();
    private Dictionary<GameObject, int> bills = new Dictionary<GameObject, int>();

    public void AddOrder(Receipe rec, GameObject table)
    {
        orderedDish.Add(new KeyValuePair<Receipe, GameObject>(rec, table));
        if (!orders.ContainsKey(table))
            orders.Add(table, new List<Receipe>());
        orders[table].Add(rec);
    }

    public void FinishOrder(KeyValuePair<Receipe, GameObject> pair)
    {
        orderedDish.Remove(pair);
        orders[pair.Value].Remove(pair.Key);
        if (orders[pair.Value].Count == 0)
        {
            orders.Remove(pair.Value);
            SceneManager.Instance.Player.GainMoney(bills[pair.Value]);
            pair.Value.GetComponent<BarTable>().EmptyTable();
        }
    }

    public void AddBill(KeyValuePair<Receipe, GameObject> pair, int amount)
    {
        if (!bills.ContainsKey(pair.Value))
            bills.Add(pair.Value, amount);
        else
            bills[pair.Value] += amount;
    }

    private void CheckOrderCompletion()
    {
        if (orderedDish.Count != 0)
        {
            foreach (KeyValuePair<Receipe, GameObject> pair in orderedDish)
            {
                if (SceneManager.Instance.Storage.CheckEnoughMaterial(pair.Key) && SceneManager.Instance.Factory.HasIdleFactory(pair.Key))
                {
                    SceneManager.Instance.Storage.ConsumeMaterial(pair.Key);
                    SceneManager.Instance.Factory.StartProduction(pair);
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
