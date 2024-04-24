using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wand : MonoBehaviour
{
    public bool keyboard;
    public Transform xboxMouse;


    // private variables
    GameController gc;
    CameraFollowTarget fl;
    Vector2 mousePos;
    Vector2 xboxMousePos;
    // Start is called before the first frame update
    void Start()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gc = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gc.keyboardToggle.isOn)
        {
            keyboard = true;
        }
        else if (gc.xboxToggle.isOn)
        {
            keyboard = false;
        }

        if (keyboard)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookDirection = mousePos - GetComponent<Rigidbody2D>().position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            GetComponent<Rigidbody2D>().rotation = angle;
        }
        else
        {
            xboxMousePos = xboxMouse.transform.position;
            Vector3 lookDirection = xboxMousePos - GetComponent<Rigidbody2D>().position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
            GetComponent<Rigidbody2D>().rotation = angle;
        }
    }
}
