using UnityEditor.UI;
using UnityEngine;

public class SkeletonLineDrawer : MonoBehaviour
{
   
    public Transform[] boneChain;

 
    public float forwardOffset = 0.5f;
    public Transform avatarRoot;    
    private LineRenderer line;



    public static bool TurnonLineRenderer=false;
    bool state = false;

    void Start()
    {

        line = GetComponent<LineRenderer>();
        if (boneChain != null)
        {
            line.positionCount = boneChain.Length;
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
