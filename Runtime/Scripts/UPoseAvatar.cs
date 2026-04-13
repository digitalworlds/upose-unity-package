
using UnityEngine;
using GLTFast;
using System.Linq;
using System.Xml.Serialization;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class UPoseAvatar : MonoBehaviour
{

    private UPoseSource pose_source=null;

    private Transform Hips;
    private Transform Spine;
    private Transform LeftUpLeg;
    private Transform LeftLeg;
    private Transform LeftFoot;
    private Transform RightUpLeg;
    private Transform RightLeg;
    private Transform RightFoot;
    private Transform LeftShoulder;
    private Transform LeftArm;
    private Transform LeftForeArm;
    private Transform LeftHand;
    private Transform LeftPalm;
    private Transform RightShoulder;
    private Transform RightArm;
    private Transform RightForeArm;
    private Transform RightHand;
    private Transform RightPalm;

    private bool AVATAR_LOADED=false;

    public bool moveToFloor = false;
    public float floorLevel = -1;
    private void Start()
    {
        UPoseSource s= FindFirstObjectByType<UPose>();
        if(s!=null && pose_source==null)setPoseSource(s);
        InitializeAvatar();
    }

    private void InitializeAvatar(){
            
        Hips = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Hips");
        Spine = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Spine");
        Transform Spine1 = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Spine1");
        Spine1.localRotation=Quaternion.Euler(0,0,0);
        Transform Spine2 = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "Spine2");
        Spine2.localRotation=Quaternion.Euler(0,0,0);

        LeftUpLeg = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LeftUpLeg");
        LeftLeg = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LeftLeg");
        LeftFoot=GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LeftFoot");
        
        RightUpLeg = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "RightUpLeg");
        RightLeg = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "RightLeg");
        RightFoot=GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "RightFoot");
    
        LeftShoulder = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LeftShoulder");
        LeftShoulder.localRotation=Quaternion.Euler(0,0,90);
        LeftArm = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LeftArm");
        LeftForeArm = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LeftForeArm");
        LeftHand=GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "LeftHand");

        RightShoulder = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "RightShoulder");
        RightShoulder.localRotation=Quaternion.Euler(0,0,-90);
        RightArm = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "RightArm");
        RightForeArm = GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "RightForeArm");
        RightHand=GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == "RightHand");
    
        if(Hips==null || Spine==null || LeftUpLeg==null || LeftLeg==null || LeftFoot==null || RightUpLeg==null || RightLeg==null || RightFoot==null || LeftShoulder==null || LeftArm==null || LeftForeArm==null || LeftHand==null || RightShoulder==null || RightArm==null || RightForeArm==null || RightHand==null){
            Debug.LogError("ERROR: Please make sure the object contains the correct humanoid hierarchy with the correct naming.");
            return;
        }

        GameObject colliderHolder = new GameObject("LeftFootCollider");
        colliderHolder.transform.SetParent(LeftFoot);
        colliderHolder.transform.localPosition = new Vector3(0, 0.125f, 0);
        colliderHolder.transform.localRotation = Quaternion.Euler(-55,0,0);
        Rigidbody rb=colliderHolder.AddComponent<Rigidbody>();
        rb.isKinematic=true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        BoxCollider collider = colliderHolder.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.15f, 0.1f, 0.3f);

        
        colliderHolder = new GameObject("RightFootCollider");
        colliderHolder.transform.SetParent(RightFoot);
        colliderHolder.transform.localPosition = new Vector3(0, 0.125f, 0);
        colliderHolder.transform.localRotation = Quaternion.Euler(-55,0,0);
        rb=colliderHolder.AddComponent<Rigidbody>();
        rb.isKinematic=true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        collider = colliderHolder.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.15f, 0.1f, 0.3f);

        
        GameObject leftPalm = new GameObject("LeftPalm");
        leftPalm.transform.parent=LeftHand;
        leftPalm.transform.localPosition = new Vector3(0, 0.07f, 0.04f);
        leftPalm.transform.localRotation = Quaternion.Euler(0,0,0);
        LeftPalm=leftPalm.transform;

        colliderHolder = new GameObject("LeftHandCollider");
        colliderHolder.transform.SetParent(LeftHand);
        colliderHolder.transform.localPosition = new Vector3(0, 0.1f, 0);
        colliderHolder.transform.localRotation = Quaternion.Euler(-90,0,0);
        rb=colliderHolder.AddComponent<Rigidbody>();
        rb.isKinematic=true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        collider = colliderHolder.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.15f, 0.1f, 0.2f);

        
        GameObject rightPalm = new GameObject("RightPalm");
        rightPalm.transform.parent=RightHand;
        rightPalm.transform.localPosition = new Vector3(0, 0.07f, 0.04f);
        rightPalm.transform.localRotation = Quaternion.Euler(0,0,0);
        RightPalm=rightPalm.transform;

        colliderHolder = new GameObject("RightHandCollider");
        colliderHolder.transform.SetParent(RightHand);
        colliderHolder.transform.localPosition = new Vector3(0, 0.1f, 0);
        colliderHolder.transform.localRotation = Quaternion.Euler(-90,0,0);
        rb=colliderHolder.AddComponent<Rigidbody>();
        rb.isKinematic=true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        collider = colliderHolder.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.15f, 0.1f, 0.2f);
                
        AVATAR_LOADED=true;

    }

    public bool isLoaded(){return AVATAR_LOADED;}
    public GameObject getLeftHand(){return LeftHand.gameObject;}
    public GameObject getRightHand(){return RightHand.gameObject;}
    public GameObject getLeftFoot(){return LeftFoot.gameObject;}
    public GameObject getRightFoot(){return RightFoot.gameObject;}
    public GameObject getLeftForeArm(){return LeftForeArm.gameObject;}
    public GameObject getRightForeArm(){return RightForeArm.gameObject;}
    public GameObject getLeftLeg(){return LeftLeg.gameObject;}
    public GameObject getRightLeg(){return RightLeg.gameObject;}
    public GameObject getLeftShoulder(){return LeftShoulder.gameObject;}
    public GameObject getRightShoulder(){return RightShoulder.gameObject;}
    public GameObject getLeftUpLeg(){return LeftUpLeg.gameObject;}
    public GameObject getRightUpLeg(){return RightUpLeg.gameObject;}
    public GameObject getLeftPalm(){return LeftPalm.gameObject;}
    public GameObject getRightPalm(){return RightPalm.gameObject;}

    public Quaternion getRightHipRotation() { return pose_source.GetRotation(Landmark.RIGHT_HIP); }
    public Quaternion getLeftHipRotation() { return pose_source.GetRotation(Landmark.LEFT_HIP); }
    public Quaternion getRightElbowRotation() { return pose_source.GetRotation(Landmark.RIGHT_ELBOW); }
    public Quaternion getLeftElbowRotation() { return pose_source.GetRotation(Landmark.LEFT_ELBOW); }

    public void MoveToFloor(float floorY)
    {
        Vector3 pos = transform.position;
        float min = Mathf.Min(LeftFoot.position.y, RightFoot.position.y);
        transform.position = new Vector3(pos.x, pos.y + (floorY - min), pos.z);
    }
    public void setPoseSource(UPoseSource source){
        pose_source=source;
        Debug.Log(this.ToString()+" pose source set to " + source.ToString());
            
    }   
    private void Update()
    {
        if (!AVATAR_LOADED || pose_source == null) return;
  
        //Get pelvis local rotation and apply it to the avatar
        Hips.localRotation = pose_source.GetRotation(Landmark.PELVIS);
        //Get torso local rotation and apply it to the avatar
        Spine.localRotation = pose_source.GetRotation(Landmark.SHOULDER_CENTER);
        //Get right upper arm rotation and apply it to the avatar
        RightArm.localRotation = Quaternion.Euler(0, 0, 90) * pose_source.GetRotation(Landmark.RIGHT_SHOULDER);
        //Get left upper arm rotation and apply it to the avatar
        LeftArm.localRotation = Quaternion.Euler(0, 0, -90) * pose_source.GetRotation(Landmark.LEFT_SHOULDER);
        //Get left fore arm rotation and apply it to the avatar
        LeftForeArm.localRotation = pose_source.GetRotation(Landmark.LEFT_ELBOW);
        //Get right fore arm rotation and apply it to the avatar
        RightForeArm.localRotation = pose_source.GetRotation(Landmark.RIGHT_ELBOW);
        //Get right thigh arm rotation and apply it to the avatar  
        RightUpLeg.localRotation = pose_source.GetRotation(Landmark.RIGHT_HIP);
        //Get left thigh rotation and apply it to the avatar
        LeftUpLeg.localRotation = pose_source.GetRotation(Landmark.LEFT_HIP);
        //Get left leg rotation and apply it to the avatar
        LeftLeg.localRotation = pose_source.GetRotation(Landmark.LEFT_KNEE);
        //Get right leg rotation and apply it to the avatar
        RightLeg.localRotation = pose_source.GetRotation(Landmark.RIGHT_KNEE);

        if (moveToFloor) MoveToFloor(floorLevel);
    }

}
