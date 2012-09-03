using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingShooter.Interfaces
{   
    /// <summary>
    /// defines objects that collide with other ICollidable objects
    /// </summary>
    public interface ICollidable
    {
        bool IsInCollisionWith(ICollidable collidable);
    }
}
