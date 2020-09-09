using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text moneyText = null;
    [SerializeField]
    private Text starText = null;

    public void updateMoney(int num)
    {
        moneyText.text = num.ToString();
    }

    public void updateStar(int num)
    {
        starText.text = num.ToString();
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
