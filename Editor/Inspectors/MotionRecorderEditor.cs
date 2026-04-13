using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(MotionRecorder))]
public class MotionRecorderEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();


        var recorder = (MotionRecorder)target;

        var startButton = new Button(() => recorder.StartRecording())
        {
            text = "Start Recording"
        };

        var stopButton = new Button(() => recorder.StopRecording())
        {
            text = "Stop Recording"
        };
        //Create horizontal container for buttons
        var buttonContainer = new VisualElement();
        buttonContainer.style.flexDirection = FlexDirection.Row;
        buttonContainer.style.justifyContent = Justify.Center;
        buttonContainer.style.marginBottom = 10;    

        buttonContainer.Add(startButton);
        buttonContainer.Add(stopButton);
        root.Add(buttonContainer);
       
    
        

        // Default inspector
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        return root;
    }
}