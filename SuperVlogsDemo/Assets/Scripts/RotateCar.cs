using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCar : MonoBehaviour
{
    //public float rotateSpeed = 500f;

    public KeyCode leftPress;
    public KeyCode rightPress;
    private Quaternion rot2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(leftPress))
        {
            //rot2 = Quaternion.Euler(-90, 0, 180);
            GetComponent<Transform>().eulerAngles = new Vector3(-90, 0, 180);
        }
        if (Input.GetKeyDown(rightPress))
        {
            //rot2 = Quaternion.Euler(-90, 0, 0);
            GetComponent<Transform>().eulerAngles = new Vector3(-90, 0, 0);
        }

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot2, Time.deltaTime * 135);
    }
}
