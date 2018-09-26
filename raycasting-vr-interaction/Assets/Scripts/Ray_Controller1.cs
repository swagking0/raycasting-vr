using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Ray_Controller1 : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    bool clone_enabled;
    public static Vector3 box_position;

    //variables for controller buttons
    //private Valve.VR.EVRButtonId touchpad1 = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    Vector2 touchpad;
    public static Quaternion rotation1;
    public static bool istouching = false;
    public static bool istouchingPos = false;
    //bool touchpad_press;


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
        //rotation1 = Ray_Controller.clone_rotation;
        if (clone_enabled)
        {
            touchpad = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            if (touchpad.x > 0)
            {
                rotation1 = new Quaternion(0, 0, 0.4f, 0.5f);
                istouching = true;
             }
            else if (touchpad.x < 0)
            {
                rotation1 = new Quaternion(0 ,0, -0.4f, 0.5f);
                istouching = true;
            }
            if (touchpad.y > 0 )
            {
                box_position = new Vector3(box_position.x, box_position.y, box_position.z + (touchpad.y*2));
               
            }
            else if (touchpad.y < 0)
            {
                box_position = new Vector3(box_position.x, box_position.y , box_position.z - (-touchpad.y * 2));
               
            }
            else
            {
                istouching = false;
            }
        }
       
    }
}