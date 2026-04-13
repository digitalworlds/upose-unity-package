using UnityEngine;

public interface UPoseSource
{
    public Quaternion GetRotation(Landmark i);
    public bool isTracked(Landmark i);
    public long getFrameCounter();
}
