using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BodyTracker2D : MonoBehaviour
{
    // Start is called before the first frame update
    ARHumanBodyManager ARHumanBodyManager;
    
    [SerializeField]
    Camera arCamera;

    [SerializeField]
    GameObject vertexPrefab;

    Dictionary<int, GameObject> vertexObjects;

    private void Awake() {
        ARHumanBodyManager = (ARHumanBodyManager) GetComponent<ARHumanBodyManager>();
        vertexObjects = new Dictionary<int, GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        NativeArray<XRHumanBodyPose2DJoint> joints = ARHumanBodyManager.GetHumanBodyPose2DJoints(Allocator.Temp);

        if(!joints.IsCreated){

            return;
        }
        UpdateVertices(joints);

    }
    void UpdateVertices(NativeArray<XRHumanBodyPose2DJoint> joints)
    {
        for(int index = 0; index < joints.Length; index++)
        {
            XRHumanBodyPose2DJoint joint = joints[index];

            GameObject obj;
            if(!vertexObjects.TryGetValue(index, out obj))
            {
                obj = Instantiate(vertexPrefab);
                vertexObjects.Add(index,obj);
            }
            if (joint.tracked)
            {
                obj.transform.position = arCamera.ViewportToWorldPoint(new Vector3(joint.position.x,joint.position.y,2.0f));
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
}
