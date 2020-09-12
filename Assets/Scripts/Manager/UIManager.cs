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
    [SerializeField]
    private Transform brush = null;
    [SerializeField]
    private Slider timeSlider = null;

    private bool destroy = false;
    private RaycastHit hit;
    private Vector3 initBrushPos;

    public void UpdateMoney(int num)
    {
        moneyText.text = num.ToString();
    }

    public void UpdateStar(int num)
    {
        starText.text = num.ToString();
    }

    public void UpdateTimeSlider(float value)
    {
        timeSlider.value = value;
    }

    public void SpeedUp()
    {
        GameItem.PlaySpeed = 2;
        //GameItem[] items = FindObjectsOfType<GameItem>();
        //if (items.Length != 0)
        //{
        //    foreach (var item in items)
        //        item.SetAnimSpeed();
        //}
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 2;
        }
    }

    public void RestoreSpeed()
    {
        GameItem.PlaySpeed = 1;
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 1;
        }
    }

    public void Pause()
    {
        GameItem.PlaySpeed = 0;
        Animator[] anims = FindObjectsOfType<Animator>();
        if (anims.Length != 0)
        {
            foreach (var anim in anims)
                anim.speed = 0;
        }
    }

    public void DestroyItem()
    {
        destroy = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        initBrushPos = brush.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            brush.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50000f, 1 << 11))
                {
                    hit.transform.parent.gameObject.GetComponent<GameItem>().DestroyItem();
                }
                brush.localPosition = initBrushPos;
                destroy = false;
            }
        }
    }
}
