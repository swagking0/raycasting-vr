using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTag : MonoBehaviour {

    // Use this for initialization
    public static string objectname;
    void Start () {
        objectname = "null";
	}
	
	// Update is called once per frame
	void Update () {
        objectname = gameObject.tag;
        Debug.Log(objectname);
    }
}
