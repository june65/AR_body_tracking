using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Runtime.Serialization;

public class BodyTracker3D : MonoBehaviour
{
    // Start is called before the first frame update
    ARHumanBodyManager arHumanBodyManager;
    
    [SerializeField]
    GameObject jointPrefab;

    Dictionary<int, GameObject> jointObjects;

    private void Awake() {
        arHumanBodyManager =  GetComponent<ARHumanBodyManager>();
        jointObjects = new Dictionary<int, GameObject>();
    }

    private void OnEnable() {
        arHumanBodyManager.humanBodiesChanged += OnHumanBodiedChaned;
    }

    private void OnDisable() {
        arHumanBodyManager.humanBodiesChanged -= OnHumanBodiedChaned;
    }

    void OnHumanBodiedChaned(ARHumanBodiesChangedEventArgs eventArgs)
    {
       foreach (ARHumanBody humanBody in eventArgs.updated)
       {
            NativeArray<XRHumanBodyJoint> joints = humanBody.joints;

            foreach (XRHumanBodyJoint joint in joints)
            {
                GameObject obj;
                if (!jointObjects.TryGetValue(joint.index, out obj))
                {
                    obj = Instantiate(jointPrefab);
                    jointObjects.Add(joint.index, obj);
                }

                if(joint.tracked)
                {
                    obj.transform.parent = humanBody.transform;
                    obj.transform.localPosition = joint.anchorPose.position * humanBody.estimatedHeightScaleFactor;
                    obj.transform.localRotation = joint.anchorPose.rotation;
                    obj.SetActive(true);
                }   

                else
                {
                    obj.SetActive(false);
                }
            }

       }
    }
}
