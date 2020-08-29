using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    private Dictionary<string, int> rawMat = new Dictionary<string, int>();

    IEnumerator CountDown(string str, int num, float time)
    {
        rawMat.Add(str, 0);
        while (true)
        {
            Debug.Log("Start grow " + str);
            yield return new WaitForSeconds(time);
            Debug.Log("Growed " + str);
            rawMat[str] += num;
        }
    }

    public void GrowMaterial(string str, int num, float time)
    {
        StartCoroutine(CountDown(str, num, time));
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
