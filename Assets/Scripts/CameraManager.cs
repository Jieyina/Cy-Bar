using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float moveSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0));
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime * Input.GetAxis("Vertical"), 0));
    }
}
