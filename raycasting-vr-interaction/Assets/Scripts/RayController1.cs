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
    float distance, dist1;
    bool ray_cond, isattached = false;
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
  
    // Use this for initialization
    void Start()
    {
        ray = GameObject.FindWithTag("green_ray");
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
        if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit) && ray_cond) //ray_cond is used when the ray is casted to see the image or clone of the box. 
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("interactive"))
            {
                hitpoint = hit.point;
                distance = Vector3.Distance(trackedObj.transform.position, hitpoint);
                ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, distance);
                ray.transform.localPosition = new Vector3(0, 0, distance / 2f);
                ray_material.color = Color.green;

                if (cond == "true" && !isattached)
                {
                    Quaternion rotation = hit.transform.localRotation;
                    hit.transform.SetParent(ray.transform.parent, true);
                    isattached = true;
                    
                }

                if (cond == "false")
                {
                    hit.transform.parent = null;
                    isattached = false;
                }

            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("supermarket"))
            {
                ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
                ray.transform.localPosition = new Vector3(0, 0, 50);
                ray_material.color = Color.red;
            }
        }
        else
        {
            ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
            ray.transform.localPosition = new Vector3(0, 0, 50);
            ray_material.color = Color.red;
       }



    }


  
    //key press method
    public string onCall()
    {
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
        gripButtonPressed = controller.GetPress(gripButton);
   
        if (gripButtonPressed)
        {
            ray.GetComponent<Renderer>().enabled = !ray.GetComponent<Renderer>().enabled;
            ray.transform.parent = trackedObj.transform;
            ray.transform.position = trackedObj.transform.position;
            ray.transform.localRotation = Quaternion.Euler(Vector3.zero);
            ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
            ray.transform.localPosition = new Vector3(0, 0, 50);
            ray_cond = !ray_cond; //changes the condition every time when the grip button is pressed. 
        }
     }

}