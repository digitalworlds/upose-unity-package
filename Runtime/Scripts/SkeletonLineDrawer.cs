//using UnityEditor.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Upose_MotionCapture.SkeletonLineDrawer
{
    [RequireComponent(typeof(LineRenderer))]
    public class SkeletonLineDrawer: MonoBehaviour
{
    [Header("Settings")]
    public Transform avatarRoot; 
    [Header("Bone Chain (Optional Override)")]    
    public Transform[] boneChain;
    Transform[] AllBones;
    Dictionary<String, Transform> FindJoint = new Dictionary<string, Transform>();
    String[] Names = {"LeftHand","RightHand","LeftForeArm","Neck","Head","RightForeArm","LeftArm","RightArm","LeftShoulder","RightShoulder", "Spine2", "Spine1", "Spine", "Hips", "LeftUpLeg","RightUpLeg","LeftLeg","RightLeg","LeftFoot","RightFoot","LeftToeBase","RightToeBase"};
    String[] Orderofnames = {"LeftHand","LeftForeArm","LeftArm","LeftShoulder","Neck","Head","Neck","RightShoulder","RightArm","RightForeArm","RightHand","RightForeArm","RightArm", "Spine2", "Spine1", "Spine", "Hips", "LeftUpLeg","LeftLeg","LeftFoot","LeftToeBase","LeftFoot","LeftLeg","LeftUpLeg","Hips","RightUpLeg","RightLeg","RightFoot","RightToeBase"};

    public float forwardOffset = 0.5f;  
    private LineRenderer line;

    public static bool TurnonLineRenderer=false;
    private bool state = false;
  //  public static bool IsLineRendererEnabled = false;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        if (line == null)
            {
                enabled = false;
                return;
            }

        if (avatarRoot == null)
            {
                avatarRoot = transform;
            }
        AllBones = avatarRoot.GetComponentsInChildren<Transform>();
        if (boneChain == null || boneChain.Length == 0)
            {
                
            }

          line.positionCount = Orderofnames.Length;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
                line.positionCount = 0;
        foreach(Transform bone in AllBones)
            {
                foreach(String nameofbone in Names)
                {
                    if (bone.name.CompareTo(nameofbone)==0)
                    {
                        FindJoint.Add(nameofbone, bone);
                    }
                }
            }
        AllBones = new Transform[Orderofnames.Length];
        for(int i=0; i<Orderofnames.Length; i++)
            {
                AllBones[i]=FindJoint[Orderofnames[i]];
            }
    }


    public void Turnon(bool thestate)
    {
        if (thestate)
        {
            GetComponent<LineRenderer>().enabled = true;
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    void Update()
    {
        if (TurnonLineRenderer != state)
        {
            Turnon(TurnonLineRenderer);
            state=TurnonLineRenderer;
        }
        if (boneChain == null || avatarRoot == null) return;

        for (int i = 0; i < boneChain.Length; i++)
        {
            if (boneChain[i] != null)
            {
               
                Vector3 worldPos = boneChain[i].position;
               
               
                Vector3 offsetPos = worldPos + avatarRoot.forward * forwardOffset;

                line.SetPosition(i, offsetPos);
            }
        }
    }
}

};

