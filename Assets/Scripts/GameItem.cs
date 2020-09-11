using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    protected static int playSpeed = 1;
    private Animator[] animators;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        SetAnimSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void DestroyItem()
    {
        Destroy(gameObject);
    }

    public static void SetPlaySpeed(int newSpeed)
    {
        playSpeed = newSpeed;
    }

    public void SetAnimSpeed()
    {
        if (animators.Length != 0)
        {
            foreach (Animator anim in animators)
                anim.speed = playSpeed;
        }
    }
}
