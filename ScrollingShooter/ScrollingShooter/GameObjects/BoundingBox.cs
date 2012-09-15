﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ScrollingShooter
{
    /// <summary>
    /// Represents the possible types of bounds
    /// </summary>
    public enum BoundType
    {
        Min,
        Max,
    }

    /// <summary>
    /// A struct representing a bound in an axis list
    /// </summary>
    public class Bound : IComparable<Bound>
    {
        public BoundingBox Box;
        public float Value;
        public BoundType Type;

        /// <summary>
        /// Creates a new Bound
        /// </summary>
        /// <param name="box">The bounding box for this bound</param>
        /// <param name="value">The value of the bound</param>
        /// <param name="type">The type of the bound, min or max</param>
        public Bound(BoundingBox box, float value, BoundType type)
        {
            this.Box = box;
            this.Value = value;
            this.Type = type;
        }

        /// <summary>
        /// Compares this bound with another bound.  Bounds are
        /// equivalent if they are the same type and have the same value.
        /// Min bounds always come before max bounds.
        /// </summary>
        /// <param name="otherBound">The bound to compare to</param>
        /// <returns>
        ///  if the bounds are equilvalent, less than 0 if the other bound is 
        ///  smaller, or greater than 0 if the other bound is greater
        /// </returns>
        public int CompareTo(Bound otherBound)
        {
            int relationship = this.Value.CompareTo(otherBound.Value);
            if( relationship == 0 )
                relationship += this.Type.CompareTo(otherBound.Type);
            return relationship;
        }
    }

    /// <summary>
    /// Creates a bounding box structure, holding the four bounds of a game object
    /// </summary>
    public class BoundingBox
    {
        public Bound Bottom;
        public Bound Top;
        public Bound Left;
        public Bound Right;
        public uint GameObjectID;

        /// <summary>
        /// Creates a bounding box from a bounding rectangle
        /// </summary>
        /// <param name="gameObjectID">The game object this bounding box belongs to</param>
        /// <param name="rectangle">A bounding rectangle for the game object</param>
        public BoundingBox(uint gameObjectID, Rectangle rectangle)
        {
            this.GameObjectID = gameObjectID;
            this.Top = new Bound(this, rectangle.Top, BoundType.Min);
            this.Left = new Bound(this, rectangle.Left, BoundType.Min);
            this.Bottom = new Bound(this, rectangle.Bottom, BoundType.Max);
            this.Right = new Bound(this, rectangle.Right, BoundType.Max);
        }

        /// <summary>
        /// Converts a bounding box to a rectangle
        /// </summary>
        /// <returns>The bouding rectangle</returns>
        public Rectangle ToRectange()
        {
            return new Rectangle()
            {
                Y = (int)this.Top.Value,
                X = (int)this.Left.Value,
                Width = (int)(this.Right.Value - this.Left.Value),
                Height = (int)(this.Bottom.Value - this.Top.Value),
            };
        }
    }
}