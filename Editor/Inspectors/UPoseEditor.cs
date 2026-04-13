namespace edu.ufl.digitalworlds.upose
{
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(UPose))]
public class UPoseEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        /*Texture2D logo = Resources.Load<Texture2D>("logo");
        var image = new Image();
        image.image = logo;
        image.scaleMode = ScaleMode.ScaleToFit;
        image.style.height = 50;
        root.Add(image);*/

    

        var upose = (UPose)target;
       
        var stickFigure = new StickFigureElement(upose);
        stickFigure.style.height = 200; 
        root.Add(stickFigure);


        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.alignItems = Align.Center;

        //DropDown for selecting landmark
        var landmarks = new List<Landmark>(new Landmark[]{Landmark.LEFT_SHOULDER,Landmark.LEFT_ELBOW,Landmark.LEFT_HIP,Landmark.LEFT_KNEE,Landmark.PELVIS,Landmark.RIGHT_SHOULDER,Landmark.RIGHT_ELBOW,Landmark.RIGHT_HIP,Landmark.RIGHT_KNEE,Landmark.SHOULDER_CENTER});
        var landmarkDropdown = new PopupField<Landmark>("",landmarks,0);

        // Axis dropdown
        var axes = new List<string> { "X", "Y", "Z" };
        var axisDropdown = new PopupField<string>("",axes,0);

        VisualElement plotContainer = new VisualElement();
        plotContainer.style.flexDirection = FlexDirection.Column;
        plotContainer.style.marginTop = 10;

        //List to keep track of added plots
        List<LinePlotElement> plots = new List<LinePlotElement>();

        var addButton = new Button(() =>
        {
            VisualElement container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;
            int currentSlot=plots.Count>0? plots.First().getCurrentSlot():0;
            LinePlotElement linePlot = new LinePlotElement(upose, landmarkDropdown.value, axisDropdown.value, currentSlot);
            linePlot.style.height = 50;

            var title = new VisualElement();
            title.style.flexDirection = FlexDirection.Row;
            title.style.justifyContent = Justify.SpaceBetween;
            title.style.marginBottom = 10;
            title.Add(new Label(landmarkDropdown.value.ToString() + " - Angle " + axisDropdown.value));
            var button=new Button(() =>{ 
                container.RemoveFromHierarchy();
                plots.Remove(linePlot);})
            {
                text = "X"
            };
            title.Add(button);
            container.Add(title); 
            container.Add(linePlot);
            plotContainer.Add(container);
            plots.Add(linePlot);
        })
        {
            text = "Add"
        };

        landmarkDropdown.style.flexGrow = 1;
        axisDropdown.style.flexGrow = 1;
        addButton.style.flexGrow = 0;


        row.Add(landmarkDropdown);
        row.Add(axisDropdown);
        row.Add(addButton);
        root.Add(row);

        root.Add(plotContainer);

        

        // Default inspector
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        return root;
    }
}
}