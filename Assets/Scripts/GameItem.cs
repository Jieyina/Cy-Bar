using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    protected int playSpeed = 1;
    private Animator[] animators;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        Debug.Log(animators.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpeedUp()
    {
        playSpeed = 2;
        if (animators.Length != 0)
        {
            foreach (Animator anim in animators)
                anim.speed = 2;
        }
    }

    public void RestoreSpeed()
    {
        playSpeed = 1;
        if (animators.Length != 0)
        {
            foreach (Animator anim in animators)
                anim.speed = 1;
        }
    }

    public void Pause()
    {
        playSpeed = 0;
        if (animators.Length != 0)
        {
            foreach (Animator anim in animators)
                anim.speed = 0;
        }
    }
}
