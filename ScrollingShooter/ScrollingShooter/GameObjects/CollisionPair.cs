using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingShooter
{
    /// <summary>
    /// A structure representing a collision pair
    /// </summary>
    public struct CollisionPair : IComparable<CollisionPair>
    {
        public readonly uint A;
        public readonly uint B;

        /// <summary>
        /// Constructs a collision pair.  The lower id always comes first 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public CollisionPair(uint A, uint B)
        {
            if (A < B)
            {
                this.A = A;
                this.B = B;
            }
            else
            {
                this.A = B;
                this.B = A;
            }
        }

        /// <summary>
        /// Compares this to another collision pair
        /// </summary>
        /// <param name="pair">The pair to compare to</param>
        /// <returns></returns>
        public int CompareTo(CollisionPair pair)
        {
            return this.A.CompareTo(pair.A) + this.B.CompareTo(pair.B);
        }
    }
}
