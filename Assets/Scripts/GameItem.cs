using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    protected Animator[] animators;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        if (animators.Length != 0)
        {
            foreach (Animator anim in animators)
                anim.speed = SceneItemManager.Instance.Player.PlaySpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void DestroyItem()
    {
        Destroy(gameObject);
    }

}
