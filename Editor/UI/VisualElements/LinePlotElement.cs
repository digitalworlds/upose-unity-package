namespace edu.ufl.digitalworlds.upose
{
using UnityEngine;
using UnityEngine.UIElements;

public class LinePlotElement : VisualElement
{
    private UPoseSource source;

    private float[] values;
    private float min_value;
    private float max_value;
    private int current_slot=0;
    private int number_of_slots;
    private bool min_max_set=false;

    Landmark landmark;
    int axis;
    
    public LinePlotElement(UPose source, Landmark landmark, string axis, int current_slot)
    {
        this.current_slot=current_slot;
        this.source = source;
        this.landmark=landmark;
        switch (axis){
            case "X":
                this.axis=0;
                break;
            case "Y":
                this.axis=1;
                break;
            case "Z":
                this.axis=2;    
                break;
            default:
                this.axis=0;
                break;
        }
        number_of_slots=100;
        values=new float[number_of_slots];

        generateVisualContent += OnGenerateVisualContent;

        // Refresh continuously
        schedule.Execute(() => MarkDirtyRepaint()).Every(32); // ~30 FPS
    }

    public int getCurrentSlot(){
        return current_slot;
    }

    private void OnGenerateVisualContent(MeshGenerationContext ctx)
    {
        var rect = contentRect;

        float width = rect.width;
        float height = rect.height;

        var painter = ctx.painter2D;

        painter.strokeColor = Color.green;
        painter.fillColor = Color.black;
        painter.lineWidth = 2;

        //new value 
        float new_value=source.GetRotation(landmark).eulerAngles[axis];

        values[current_slot]=new_value;
        if(min_value>new_value)min_value=new_value;
        if(max_value<new_value)max_value=new_value;
        if (!min_max_set)
        {
            min_value=new_value;
            max_value=new_value;
            for(int i=0;i<number_of_slots;i++){
                values[i]=new_value;
            }
            min_max_set=true;
        }
        current_slot++;
        if(current_slot>=number_of_slots)current_slot=0;


        float scale=max_value-min_value;
        if(scale==0)scale=1;

        painter.BeginPath();
        painter.MoveTo(new Vector2(0, height*(1-(values[0]-min_value) / scale)));
            
        for (int i = 1; i < number_of_slots; i++)        {
            
            if(current_slot==i) {
                painter.Stroke();
                painter.BeginPath();
                painter.MoveTo(new Vector2(width*i/(number_of_slots-1), height * (1-(values[i]-min_value) / scale)));
     
            }else{
                painter.LineTo(new Vector2(width*i/(number_of_slots-1), height * (1-(values[i]-min_value) / scale)));
            }
        }
        painter.Stroke();
    }


}
}