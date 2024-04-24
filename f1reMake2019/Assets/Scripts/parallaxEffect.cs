using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxEffect : MonoBehaviour
{
    // Script contains the code that makes the "parallax effect"
    private float length, startpos;

    public GameObject cam;
    public Transform player;

    public float parallexEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    void Update()
    {

        float temp = (player.transform.position.x * (1 - parallexEffect));
        float dist = (player.transform.position.x * parallexEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }

    }
}
