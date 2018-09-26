using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Text.RegularExpressions;

public class Ray_Controller : MonoBehaviour
{

    public GameObject ray;
    string condition;
    RaycastHit hit;
    Vector3 hitpoint;
    float distance;
    bool ray_cond = false;
    bool killmyName;
    bool isrotating, isattached, isupdating, boxEnabled = false; 
    public static string objectname, objectnameholder;
    private GameObject ObjectGetter, ObjectHolder, box;
    Vector3 SizeGetter, clone_position;
    public GameObject spawmPointer;
    public Transform ParentBox;
    public Material ray_material;
    public static bool boxEnabled1;
    public static Vector3 clone_position1;
    public static Quaternion clone_rotation;
    bool instantiation = false;


    private SteamVR_TrackedObject trackedObj;
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }

    public SteamVR_Camera head;

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
        ray_material = ray.GetComponent<Renderer>().material;
        clone_position = new Vector3(0, 0, 0);
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
                ray_material.color = Color.green;
                instantiation = true;
                clone(ObjectGetter, instantiation , SizeGetter);

                if (!isupdating)
                {
                    ParentBox = ObjectGetter.transform.parent;
                }


                if (Ray_Controller1.istouching)
                {
                    clone_rotation = Ray_Controller1.rotation1;
                    ObjectHolder.transform.Rotate(new Vector3(clone_rotation.x, clone_rotation.y, clone_rotation.z));
                    clone_position = Ray_Controller1.box_position;
                    ObjectHolder.transform.position = new Vector3(clone_position.x, clone_position.y, clone_position.z);
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
                if (!killmyName)
                {
                    objectname = ParentBox.name;
                }
                if (killmyName)
                {

                    objectname = " ";
                }
            }
            else
            {
                instantiation = true;
                clone(ObjectGetter, instantiation, SizeGetter);
                if (Ray_Controller1.istouching)
                {
                    clone_rotation = Ray_Controller1.rotation1;
                    ObjectHolder.transform.Rotate(new Vector3(clone_rotation.x, clone_rotation.y, clone_rotation.z));
                    clone_position = Ray_Controller1.box_position;
                    ObjectHolder.transform.position = new Vector3(clone_position.x, clone_position.y, clone_position.z);
                }
            }

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("supermarket"))
            {
                ray.transform.localScale = new Vector3(ray.transform.localScale.x, ray.transform.localScale.y, 100);
                ray.transform.localPosition = new Vector3(0, 0, 50);
                objectname = " ";
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
            ray_cond = !ray_cond;
        }
      

    }

    //block for cloning the object
    void clone(GameObject obj, bool InstantObj , Vector3 dimensions)
    {
       Touch_pad_PressUp = controller.GetPressUp(touchpad);
       Touch_pad_PressDown = controller.GetPressDown(touchpad);
       string x_value = dimensions.x.ToString();
        
        if (Touch_pad_PressDown && Regex.IsMatch(x_value, "^[0-9]\\.[0-9]*") && InstantObj)
        {
            ObjectHolder = Instantiate(obj);
            ObjectHolder.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
            ObjectHolder.transform.position = spawmPointer.transform.position;
            clone_position1 = spawmPointer.transform.position;
            boxEnabled1 = true;
            isrotating = true;
            killmyName = true;
        }
        else if (Touch_pad_PressDown && Regex.IsMatch(x_value, "^[0-9].\\.[0-9]*") && InstantObj)
        {
            ObjectHolder = Instantiate(obj);
            ObjectHolder.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            ObjectHolder.transform.position = spawmPointer.transform.position;
            clone_position1 = spawmPointer.transform.position;
            boxEnabled1 = true;
            isrotating = true;
            killmyName = true;
          
        }
        if (Touch_pad_PressUp)
        {
            isrotating = false;
            Destroy(ObjectHolder);
            killmyName = false;
            boxEnabled = false;
            InstantObj = false;
        }
       if (!InstantObj)
        {
            Destroy(ObjectHolder);
        }
    }

}