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
    bool ray_cond = false;
    bool instantiation = false;
    bool isrotating = false; 
    public static string objectname;
    private GameObject ObjectGetter, ObjectHolder;
    public GameObject spawmPointer;


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
    private bool Touch_pad_PressUp = false;
    private bool Touch_pad_PressDown = false;

    


    // Use this for initialization
    void Start()
    {
        objectname = " ";
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
                ObjectGetter = hit.collider.gameObject;
                instantiation = true;
                clone(ObjectGetter, instantiation);
                if (isrotating)
                {
                    ObjectHolder.transform.Rotate(new Vector3(0,0, -0.5f));
                }
                if (cond == "true")
                {
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
                     hit.transform.parent = null;
                }
                if (objectname == " ")
                {
                    objectname = hit.collider.tag;
                }
  

            }
            else
            {
                instantiation = false;
                clone(ObjectGetter, instantiation);
            }
            
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("supermarket"))
            {
                ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
                ray.transform.localPosition = new Vector3(0, 0, 50);
                objectname = " ";
                
            }

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
            ray_cond = !ray_cond;
        }
      

    }
 


    //block for cloning the object
    void clone(GameObject obj, bool condition)
    {
        Debug.Log(condition);
        Touch_pad_PressUp = controller.GetPressUp(touchpad);
        Touch_pad_PressDown = controller.GetPressDown(touchpad);
        Debug.Log("Touchpad is pressed down = " + Touch_pad_PressDown);
        if (Touch_pad_PressDown && condition)
        {
            ObjectHolder = Instantiate(obj);
            ObjectHolder.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
            ObjectHolder.transform.position = spawmPointer.transform.position;
            isrotating = true; 
        }
        if (Touch_pad_PressUp)
        {
            isrotating = false;
            Destroy(ObjectHolder);
        }
        else if (!condition)
        {
            Destroy(ObjectHolder);
        }

    }

}