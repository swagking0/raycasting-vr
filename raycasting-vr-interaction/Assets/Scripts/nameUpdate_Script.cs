using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nameUpdate_Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
         infoText_Script.stringInfo = RayController.objectname;
       
    }
}
