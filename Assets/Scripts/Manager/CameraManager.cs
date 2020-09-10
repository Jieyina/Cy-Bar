using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private int boundary = 5;
    private Vector3 mousePos;
    private int screenWidth;
    private int screenHeight;

    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0));
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime * Input.GetAxis("Vertical"), 0));
        mousePos = Input.mousePosition;
        if (mousePos.x < boundary)
            transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
        if (mousePos.x > screenWidth - boundary)
            transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
        if (mousePos.y < boundary)
            transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
        if (mousePos.y > screenHeight - boundary)
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
    }
}
