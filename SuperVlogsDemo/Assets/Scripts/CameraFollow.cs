using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float distanceFromTarget = 5;
    public float height = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position - target.transform.forward * distanceFromTarget;
        transform.LookAt(target.transform.position);
        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
    }
}
