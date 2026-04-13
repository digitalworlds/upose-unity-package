using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class AvatarAnimationPlayer : MonoBehaviour, UPoseSource
{
    public string jsonFileName;
    public float fps = 30f;
    public bool loop = true;

    MotionRecording recording;
    float startTime;
    private int current_frame;

    MotionSample rotValues;

    void Start()
    {
        
        LoadData();
        UPoseAvatar avatar = FindFirstObjectByType<UPoseAvatar>();
        if(avatar!=null) avatar.setPoseSource(this);
        startTime = Time.time;
    }

    void LoadData()
    {
        string path = Path.Combine(Application.persistentDataPath, "motion.json");

        if (!File.Exists(path))
        {
            Debug.LogError("File not found: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        Debug.Log("Loaded json: " + json);
        recording=JsonConvert.DeserializeObject<MotionRecording>(json);


        Debug.Log("Loaded frames: " + recording.samples.Count);

    }

    void Update()
    {
        if (recording == null || recording.samples.Count == 0)
            return;

        int frame = Mathf.FloorToInt((Time.time - startTime) * fps);

        if (loop)
            frame %= recording.samples.Count;
        else
            frame = Mathf.Min(frame, recording.samples.Count - 1);

        
        rotValues = recording.samples[frame];
        current_frame=frame;

    }

    public MotionSample getRotations()
    {
        return rotValues;
    }

    public Quaternion GetRotation(Landmark i)
    {
        switch (i)
        {
            case Landmark.PELVIS:
                return rotValues.hips;
            case Landmark.SHOULDER_CENTER:
                return rotValues.spine;
            case Landmark.RIGHT_SHOULDER:
                return rotValues.rightArm;
            case Landmark.LEFT_SHOULDER:
                return rotValues.leftArm;
            case Landmark.LEFT_ELBOW:
                return rotValues.leftForeArm;
            case Landmark.RIGHT_ELBOW:
                return rotValues.rightForeArm;
            case Landmark.RIGHT_HIP:
                return rotValues.rightUpLeg;
            case Landmark.LEFT_HIP:
                return rotValues.leftUpLeg;
            case Landmark.LEFT_KNEE:
                return rotValues.leftLeg;
            case Landmark.RIGHT_KNEE:
                return rotValues.rightLeg;
            default:
                return Quaternion.identity;
        }
    }

    public bool isTracked(Landmark i)
    {
        return true;
    }

    public long getFrameCounter()
    {
        return current_frame;
    }
}
