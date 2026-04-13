using UnityEngine;
using UnityEngine.UIElements;

public class StickFigureElement : VisualElement
{
    private UPoseSource source;

    public StickFigureElement(UPoseSource source)
    {
        this.source = source;

        generateVisualContent += OnGenerateVisualContent;

        // Refresh continuously
        schedule.Execute(() => MarkDirtyRepaint()).Every(32); // ~30 FPS
    }

    private void OnGenerateVisualContent(MeshGenerationContext ctx)
    {
        var rect = contentRect;

        float width = rect.width;
        float height = rect.height;

        var painter = ctx.painter2D;

        painter.strokeColor = Color.white;
        painter.fillColor = Color.black;
        painter.lineWidth = 2;
        
        // --- Center + scale ---
        Vector2 center = new Vector2(width * 0.5f, height * 0.5f);

        float scale = Mathf.Min(width, height);

    

        void SetColor(Landmark mark)
        {
            if(source.isTracked(mark)){
                painter.fillColor = Color.white;
                painter.strokeColor = Color.white;
            }
            else    {
                painter.fillColor = Color.black;
                painter.strokeColor = Color.white;
            }
        }
        
        

        void DrawPolygon(int[] points){
            painter.BeginPath();
            for(int i=0;i<points.Length;i+=2){
                painter.LineTo(center+new Vector2(points[i]/350f*scale-0.29f*scale, points[i+1]/350f*scale-0.5f*scale));
            }
            painter.LineTo(center+new Vector2(points[0]/350f*scale-0.29f*scale, points[1]/350f*scale-0.5f*scale));
            painter.Fill();
            painter.Stroke();
        }

        //Head
        painter.fillColor = Color.black;
        painter.strokeColor = Color.white;
        DrawPolygon(new int[]{90,0, 110,0, 125,10, 130,25, 125,40, 110,50, 90,50, 75,40, 70,25, 75,10});
        //Torso
        SetColor(Landmark.SHOULDER_CENTER);
        DrawPolygon(new int[]{70,60, 130,60, 140,140, 60,140});
        SetColor(Landmark.PELVIS);
        DrawPolygon(new int[]{60,140, 140,140, 135,180, 65,180});
        
        //Right Arm
        SetColor(Landmark.RIGHT_SHOULDER);
        DrawPolygon(new int[]{130,60, 159,68, 180,120, 157,130});
        //Right ForeArm
        SetColor(Landmark.RIGHT_ELBOW);
        DrawPolygon(new int[]{157,130, 180,120, 195,173, 172,182});

        //Left Arm
        SetColor(Landmark.LEFT_SHOULDER);
        DrawPolygon(new int[]{70,60, 41,68, 20,120, 43,130});
        //Left ForeArm 
        SetColor(Landmark.LEFT_ELBOW);
        DrawPolygon(new int[]{43,130, 20,120, 5,173, 28,182});
  
        //Left Thigh
        SetColor(Landmark.LEFT_HIP);
        DrawPolygon(new int[]{65,180, 95,180, 90,250, 70,250});
        //Left Leg
        SetColor(Landmark.LEFT_KNEE);
        DrawPolygon(new int[]{70,250, 90,250, 85,330, 65,330});
        //Right Thigh
        SetColor(Landmark.RIGHT_HIP);
        DrawPolygon(new int[]{105,180, 135,180, 130,250, 110,250});
        //Right Leg
        SetColor(Landmark.RIGHT_KNEE);
        DrawPolygon(new int[]{110,250, 130,250, 135,330, 115,330});
    }


}