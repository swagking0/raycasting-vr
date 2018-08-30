using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class RayController1 : MonoBehaviour
{

    public GameObject ray;
    string condition;
    RaycastHit hit;
    Vector3 hitpoint;
    float distance;
    bool ray_cond = false;
    public Material ray_material;

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

    private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private bool touchpadPressUp = false;
    private bool touchpadPressDown = false;


    //added work
    //public bool displayplayer;
    //public bool displayplayer_1;

    public bool t_press;
    //public string touchpad_switch_condition = "false";



    // Use this for initialization
    void Start()
    {
        // trackedObj = GetComponent<SteamVR_TrackedObject>();
        ray = GameObject.FindWithTag("ray");
        ray.GetComponent<Renderer>().enabled = false;
        ray_material = ray.GetComponent<Renderer>().material;

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
        Invoke("touchpad_pad_switch", 2); //Invoke is used as a freezing time for calling the method. 2 represents two seconds.
        if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit) && ray_cond) //ray_cond is used when the ray is casted to see the image or clone of the box. 
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("interactive"))
            {
                hitpoint = hit.point;
                distance = Vector3.Distance(trackedObj.transform.position, hitpoint);
                ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, distance);
                ray.transform.localPosition = new Vector3(0, 0, distance / 2f);
                ray_material.color = Color.green;

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
                    //ray_material.color = Color.green;
                }

                if (cond == "false")
                {
                    Debug.Log("Attach False");
                    hit.transform.parent = null;
                    //hit.transform.position = gameObject.transform.localPosition;
                    //ray_material.color = Color.red;
                }

            }
            //else {
                //ray_material.color = Color.red;
            //}
            //if (hit.transform.gameObject.CompareTag("box_example_1"))
            //{
                //displayplayer = true;
            //}
            //else if (hit.transform.gameObject.CompareTag("box_example_2"))
            //{
                //displayplayer_1 = true;
            //}

        }
        else
        {
            ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
            ray.transform.localPosition = new Vector3(0, 0, 50);
            ray_material.color = Color.red;
            //displayplayer = false;
            //displayplayer_1 = false;
        }



    }

    //for showing clone in other script
    //as Dominik said to do like press Up and Down, so changing it to that
    public bool touchpad_press_switch()
    {
        touchpadPressUp = controller.GetPressUp(touchpad);
        touchpadPressDown = controller.GetPressDown(touchpad);

        if (touchpadPressDown)
        {
            t_press = true;
        }
        if (touchpadPressUp)
        {
            t_press = false;
        }
        return t_press;
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
            ray_cond = !ray_cond; //changes the condition every time when the grip button is pressed. 
            //ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, hit.distance);
        }
        /*if (triggerButtonUp)
        {
            Debug.Log("Trigger Button UP is pressed!");
            ray.GetComponent<Renderer>().enabled = false;
        }*/

    }

}