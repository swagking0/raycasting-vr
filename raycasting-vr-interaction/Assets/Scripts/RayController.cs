using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Text.RegularExpressions;

public class RayController : MonoBehaviour
{

    public GameObject ray;
    string condition;
    RaycastHit hit;
    Vector3 hitpoint;
    float distance;
    bool ray_cond = false;
    static bool instantiation = false;
    bool killmyName;
    bool isrotating, isattached, isupdating = false; 
    public static string objectname, objectnameholder;
    private GameObject ObjectGetter, ObjectHolder, box;
    Vector3 SizeGetter;
    public GameObject spawmPointer;
    public Transform ParentBox;


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
        if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit) && ray_cond)
        {
            
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("interactive"))
            {
                hitpoint = hit.point;
                distance = Vector3.Distance(trackedObj.transform.position, hitpoint);
                ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, distance);
                ray.transform.localPosition = new Vector3(0, 0, distance / 2f);
                ObjectGetter = hit.collider.gameObject;
                SizeGetter = hit.collider.GetComponent<BoxCollider>().size;
                instantiation = true;
                clone(ObjectGetter, instantiation, SizeGetter);
                if (!isupdating)
                {
                    ParentBox = ObjectGetter.transform.parent;
                }


                if (isrotating)
                {
                    ObjectHolder.transform.Rotate(new Vector3(0,0, -0.5f));
                }
                if (cond == "true" && !isattached)
                {
                    Quaternion rotation = hit.transform.localRotation;
                    hit.transform.SetParent(ray.transform.parent, true);
                    isattached = true;
                    killmyName = true;
                    isupdating = true;


                }
                
                if (cond == "false" && isattached)
                {
                    hit.transform.parent = null;
                    hit.transform.SetParent(ParentBox, true);
                    isattached = false;
                    killmyName = false;
                    isupdating = false;
                }
                if (killmyName == false)
                {
                    objectname = ParentBox.name;
                }
                if (killmyName == true)
                {

                    objectname = " ";
                }

            }
            else
            {
                instantiation = false;
                clone(ObjectGetter, instantiation, SizeGetter);
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
    void clone(GameObject obj, bool condition, Vector3 dimensions)
    {
       Touch_pad_PressUp = controller.GetPressUp(touchpad);
        Touch_pad_PressDown = controller.GetPressDown(touchpad);
        string x_value = dimensions.x.ToString();
        
        if (Touch_pad_PressDown && condition && Regex.IsMatch(x_value, "^[0-9]\\.[0-9]*"))
        {
            ObjectHolder = Instantiate(obj);
            ObjectHolder.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
            ObjectHolder.transform.position = spawmPointer.transform.position;
            isrotating = true;
            killmyName = true;
        }
        else if (Touch_pad_PressDown && condition && Regex.IsMatch(x_value, "^[0-9].\\.[0-9]*"))
        {
            ObjectHolder = Instantiate(obj);
            ObjectHolder.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            ObjectHolder.transform.position = spawmPointer.transform.position;
            isrotating = true;
            killmyName = true;
        }
        if (Touch_pad_PressUp)
        {
            isrotating = false;
            Destroy(ObjectHolder);
            killmyName = false;
        }
        else if (!condition)
        {
            Destroy(ObjectHolder);
            isrotating = false;
        }

    }

}