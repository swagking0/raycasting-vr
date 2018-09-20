using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Ray_Controller1 : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    bool clone_enabled;
    Vector3 box_position;

    //variables for controller buttons
    //private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    Vector2 touchpad;
    Quaternion rotation;

    // Use this for initialization
    void Start()
    {
        clone_enabled = false;
        box_position = new Vector3(0, 0, 0);
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    // Update is called once per frame
    void Update()
    {
        clone_enabled = Ray_Controller.boxEnabled1;
        box_position = Ray_Controller.clone_position1;
        rotation = Ray_Controller.clone_rotation;
        if (clone_enabled)
        {
            touchpad = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            //Debug.Log(touchpad);
            //Debug.Log(box_position.z);
            Debug.Log(rotation);
        }
       
    }
}