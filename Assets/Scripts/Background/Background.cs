using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    [SerializeField] private float parallaxEffect;
    private GameObject cam;
    private float xPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPos + dist, cam.transform.position.y);
    }
}
