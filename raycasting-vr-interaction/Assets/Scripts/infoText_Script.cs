using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoText_Script : MonoBehaviour {


    Text infoText;
    public static string stringInfo;
	// Use this for initialization
	void Start () {
        infoText = GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void Update () {
        infoText.text = stringInfo;
        infoText.color = Color.black;
		
	}
}
