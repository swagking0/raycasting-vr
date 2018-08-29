using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class RayController : MonoBehaviour
{

    public GameObject ray;
    string condition;
    RaycastHit hit;
    Vector3 hitpoint;
    float distance;
    string ray_cond;

    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    //variables for controller buttons
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool gripButtonPressed = false;


    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonPressed = false;

    //controller_input
    private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private bool Touch_pad_Press = false;
    //private bool Touch_pad_PressDown = false;
    //added work
    public bool displayplayer;
    public bool displayplayer_1;

    public bool t_press;
    public string touchpad_switch_condition = "false";



    // Use this for initialization
    void Start()
    {
        // trackedObj = GetComponent<SteamVR_TrackedObject>();
        ray = GameObject.FindWithTag("ray");
        ray.GetComponent<Renderer>().enabled = false;

    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


    // Update is called once per frame
    void Update()
    {
        ShowLaser();
        string cond = onCall();
        if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("interactive"))
            {
                hitpoint = hit.point;
                distance = Vector3.Distance(trackedObj.transform.position, hitpoint);
                ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, distance);
                ray.transform.localPosition = new Vector3(0, 0, distance / 2f);

                if (cond == "true")
                {
                    Debug.Log(" Attach True ");
                    Quaternion rotation = hit.transform.localRotation;
                    Vector3 scale = hit.transform.localScale;
                    hit.transform.SetParent(ray.transform.parent, true);
                    /*hitpoint = hit.point;
                    distance = Vector3.Distance(trackedObj.transform.position, hitpoint);
                    ray.transform.localScale = new Vector3 (ray.transform.localScale.x, ray.transform.localScale.y, distance);
                    ray.transform.localPosition = new Vector3(0, 0, distance / 2f);*/
                }

                if (cond == "false")
                {
                    Debug.Log("Attach False");
                    hit.transform.parent = null;
                    //hit.transform.position = gameObject.transform.localPosition;
                }

            }
            if (hit.transform.gameObject.CompareTag("box_example_1"))
            {
                displayplayer = true;
            }
            else if (hit.transform.gameObject.CompareTag("box_example_2"))
            {
                displayplayer_1 = true;
            }

        }
        else
        {
            ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
            ray.transform.localPosition = new Vector3(0, 0, 50);
            displayplayer = false;
            displayplayer_1 = false;
        }


        //for showing clone in other script
        Touch_pad_Press = controller.GetPress(touchpad);
        //Touch_pad_PressDown = controller.GetPressDown(touchpad);

        if (Touch_pad_Press && touchpad_switch_condition == "true")
        {
            t_press = true;
            touchpad_switch_condition = "false";
            Debug.Log(touchpad_switch_condition);
        }
        if (Touch_pad_Press && touchpad_switch_condition == "false")
        {
            t_press = false;
            touchpad_switch_condition = "true";
            Debug.Log(touchpad_switch_condition);
        }
    }

    //key press method
    public string onCall()
    {
        //gripButtonDown = controller.GetPressDown(gripButton);
        //gripButtonUp = controller.GetPressUp(gripButton);
        //gripButtonPressed = controller.GetPress(gripButton);
        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);

        if (triggerButtonDown)
        {
            condition = "true";
        }
        if (triggerButtonUp)
        {
            condition = "false";
        }
        return condition;
    }


    //show laser method
    private void ShowLaser()
    {
        //triggerButtonDown = controller.GetPressDown(triggerButton);
        //triggerButtonUp = controller.GetPressUp(triggerButton);
        gripButtonPressed = controller.GetPress(gripButton);
        //Touch_pad_Press = controller.GetPress(touchpad);

        if (gripButtonPressed)
        {
            Debug.Log("Trigger Button Down is pressed!");
            ray.GetComponent<Renderer>().enabled = !ray.GetComponent<Renderer>().enabled;
            ray.transform.parent = trackedObj.transform;
            ray.transform.position = trackedObj.transform.position;
            ray.transform.localRotation = Quaternion.Euler(Vector3.zero);
            ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
            ray.transform.localPosition = new Vector3(0, 0, 50);
            ray_cond = "true";
            //ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, hit.distance);
        }
        /*if (triggerButtonUp)
        {
            Debug.Log("Trigger Button UP is pressed!");
            ray.GetComponent<Renderer>().enabled = false;
        }*/

    }

}