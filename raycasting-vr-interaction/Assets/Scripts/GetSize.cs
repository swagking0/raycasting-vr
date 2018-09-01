using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSize : MonoBehaviour {

    public static float in_x;
    public static float in_y;
    public static float in_z;
    // Use this for initialization
    void Start () {
        Vector3 boxSize = GetComponent<Collider>().bounds.size;
        in_x = boxSize.x;
        in_y = boxSize.y;
        in_z = boxSize.z;
        Debug.Log(in_x);
        //Debug.Log(in_y);
        //Debug.Log(in_z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
