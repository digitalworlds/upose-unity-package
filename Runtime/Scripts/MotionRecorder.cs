namespace edu.ufl.digitalworlds.upose
{
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class MotionSample
{
    public float time;
    public Quaternion hips;
    public Quaternion spine;
    public Quaternion rightArm;
    public Quaternion leftArm;
    public Quaternion leftForeArm;
    public Quaternion rightForeArm;
    public Quaternion rightUpLeg;
    public Quaternion leftUpLeg;
    public Quaternion leftLeg;
    public Quaternion rightLeg; 
}

[Serializable]
public class MotionRecording
{
    public List<MotionSample> samples = new List<MotionSample>();
}

public class MotionRecorder : MonoBehaviour
{

    private MotionRecording recording = new MotionRecording();
    private UPoseSource server;

    private float startTime;
    public bool isRecording;
    public int FPS=0;
    private long lastFrameRecorded;
    public long framesRecorded;

    public long mediapipeFrame;

    private void Start()
    {
        server = FindFirstObjectByType<UPose>();
        if (server == null)
        {
            Debug.LogError("You must have a MotionTracking server in the scene!");
            return;
        }
    }

    private float fps_lastTime;
    private float fps_counter=0;

    private void FixedUpdate()
    {
        mediapipeFrame= server.getFrameCounter();
        if(mediapipeFrame == lastFrameRecorded) return;
        lastFrameRecorded = mediapipeFrame;

        fps_counter++;
        if (fps_counter == 10)
        {
            FPS=Mathf.RoundToInt(10f / (Time.time - fps_lastTime));
            fps_lastTime=Time.time;
            fps_counter=0;
        }

        if (!isRecording) return;

        MotionSample sample = new MotionSample
        {
            time = Time.time - startTime,
            hips = server.GetRotation(Landmark.PELVIS),
            spine = server.GetRotation(Landmark.SHOULDER_CENTER),
            rightArm = server.GetRotation(Landmark.RIGHT_SHOULDER),
            leftArm = server.GetRotation(Landmark.LEFT_SHOULDER),
            leftForeArm = server.GetRotation(Landmark.LEFT_ELBOW),
            rightForeArm = server.GetRotation(Landmark.RIGHT_ELBOW),
            rightUpLeg = server.GetRotation(Landmark.RIGHT_HIP),
            leftUpLeg = server.GetRotation(Landmark.LEFT_HIP),
            leftLeg = server.GetRotation(Landmark.LEFT_KNEE),
            rightLeg = server.GetRotation(Landmark.RIGHT_KNEE)
        };

        recording.samples.Add(sample);
        framesRecorded++;
        
    }

    public bool isTracked(Landmark mark)
    {
        return server!=null && server.isTracked(mark);
    }

    public UPoseSource getServer(){return server;}

    public void StartRecording()
    {
        recording = new MotionRecording();
        startTime = Time.time;
        framesRecorded = 0;
        isRecording = true;
        Debug.Log("Recording started");
    }

    public void StopRecording()
    {
        isRecording = false;
        Debug.Log("Recording stopped");
        SaveToJson();
    }

    public void SaveToJson()
    {
        string json = JsonUtility.ToJson(recording, true); // pretty print

        string path = Path.Combine(Application.persistentDataPath, "motion.json");
        File.WriteAllText(path, json);

        Debug.Log("Saved to: " + path);
    }
}
}