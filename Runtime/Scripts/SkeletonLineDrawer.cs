//using UnityEditor.UI;
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

        if (boneChain == null || boneChain.Length == 0)
            {
                boneChain = avatarRoot.GetComponentsInChildren<Transform>();
            }

        if (boneChain != null && boneChain.Length > 0)
        {
            line.positionCount = boneChain.Length;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            }
            else
            {
                line.positionCount = 0;
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

