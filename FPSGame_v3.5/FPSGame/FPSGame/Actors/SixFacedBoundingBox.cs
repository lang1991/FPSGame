using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace FPSGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SixFacedBoundingBox
    {
        BoundingBox[] boundingBoxGroup;


        public SixFacedBoundingBox(BoundingBox box)
        {
            boundingBoxGroup = new BoundingBox[6];

            // -X  
            boundingBoxGroup[0] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Min.X, box.Max.Y, box.Max.Z));

            // +X  
            boundingBoxGroup[1] = new BoundingBox(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z));

            // -Y  
            boundingBoxGroup[2] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Min.Y, box.Max.Z));

            // +Y  
            boundingBoxGroup[3] = new BoundingBox(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z));

            // -Z  
            boundingBoxGroup[4] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Min.Z));

            // +Z  
            boundingBoxGroup[5] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z)); 
        }

        public int? Intersects(BoundingBox box)
        {
            for (int i = 0; i < boundingBoxGroup.Length; i++)
            {
                if (box.Intersects(boundingBoxGroup[i]))
                    return i;
            }
            return null;
        }

        public void updateBoundingBoxGroup(BoundingBox box)
        {
            boundingBoxGroup[0] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Min.X, box.Max.Y, box.Max.Z));

            // +X  
            boundingBoxGroup[1] = new BoundingBox(new Vector3(box.Max.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z));

            // -Y  
            boundingBoxGroup[2] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Min.Y, box.Max.Z));

            // +Y  
            boundingBoxGroup[3] = new BoundingBox(new Vector3(box.Min.X, box.Max.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z));

            // -Z  
            boundingBoxGroup[4] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Min.Z), new Vector3(box.Max.X, box.Max.Y, box.Min.Z));

            // +Z  
            boundingBoxGroup[5] = new BoundingBox(new Vector3(box.Min.X, box.Min.Y, box.Max.Z), new Vector3(box.Max.X, box.Max.Y, box.Max.Z)); 
        }

        public BoundingBox[] getBoxes()
        {
            return boundingBoxGroup;
        }
    }
}
