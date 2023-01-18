using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiFollowObject : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = cam.WorldToScreenPoint(lookAt.position + offset);

        if(transform.position != position){
            transform.position = position;
        }
        
    }
}
