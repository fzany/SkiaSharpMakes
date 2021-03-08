using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaSharpMakes.Helpers
{
    public class General
    {

        public static List<string> Colors = new List<string>() { "#107C10", "#FF5A5F", "#4267B2", "#4285F4", "#DB4437", "#F4B400", "#0F9D58", "#737373", "#FFB900", "#00A4EF", "#7FBA00", "#F25022" }; //tips from Google and Microsoft Colors

        public static void PointOnCircle(float radius, float angleInDegrees, float originx, float originy, out float x, out float y)
        {
            // Convert from degrees to radians via multiplication by PI/180        
            x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + originx;
            y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + originy;

        }
    }
}
