<<<<<<< HEAD
=======
<<<<<<< HEAD:ScrollingShooter/ScrollingShooter/GameObjectManager.cs
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
        /// <param name="otherBound"></param>
        /// <returns></returns>
        public int CompareTo(Bound otherBound)
        {
            return this.Type.CompareTo(otherBound.Type) + this.Value.CompareTo(otherBound.Value);
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
            return new Rectangle() {
                Y = (int)this.Top.Value,
                X = (int)this.Left.Value,
                Width = (int)(this.Right.Value - this.Left.Value),
                Height = (int)(this.Bottom.Value - this.Top.Value),
            };
        }
    }

    /// <summary>
    /// A structure representing a collision pair
    /// </summary>
    struct CollisionPair : IComparable<CollisionPair>
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


    /// <summary>
    /// A class for managing game objects
    /// </summary>
    public class GameObjectManager
    {
        ContentManager content;

        uint objectCount = 0;
        
        Dictionary<uint, GameObject> gameObjects;
        
        Queue<GameObject> createdGameObjects;
        Queue<GameObject> destroyedGameObjects;

        Dictionary<uint,BoundingBox> boundingBoxes;

        List<Bound> horizontalAxis;

        List<Bound> verticalAxis;

        HashSet<CollisionPair> collisions;

        /// <summary>
        /// Constructs a new GameObjectManager instance
        /// </summary>
        /// <param name="content">A ContentManager to use in loading assets</param>
        public GameObjectManager(ContentManager content)
        {
            this.content = content;

            gameObjects = new Dictionary<uint, GameObject>();

            createdGameObjects = new Queue<GameObject>();
            destroyedGameObjects = new Queue<GameObject>();

            boundingBoxes = new Dictionary<uint, BoundingBox>();
            horizontalAxis = new List<Bound>();
            verticalAxis = new List<Bound>();

            collisions = new HashSet<CollisionPair>();
        }

        public void Update(float elapsedTime)
        {
            // Add newly created game objects
            while (createdGameObjects.Count > 0)
            {
                GameObject go = createdGameObjects.Dequeue();
                AddGameObject(go);
            }

            // Remove destroyed game objects
            while (destroyedGameObjects.Count > 0)
            {
                GameObject go = destroyedGameObjects.Dequeue();
                RemoveGameObject(go);
            } 
            
            // Update our game objects
            foreach (GameObject go in gameObjects.Values)
            {
                // Call the GameObject's own update method
                go.Update(elapsedTime);

                // Update the game object's bounds to reflect 
                // changes this frame
                UpdateGameObject(go.ID);
            }

            // Update our axis lists
            UpdateAxisLists();

            // REMOVE: Test code to see what collisions are added to our list
            string colliders = "";
            foreach (CollisionPair pair in collisions)
            {
                colliders += pair.A + "-" + pair.B + "-";
            }
            ScrollingShooterGame.Game.Window.Title = colliders;
        }

        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject go in gameObjects.Values)
            {
                go.Draw(elapsedTime, spriteBatch);
            }
        }


        #region Query Methods

        /// <summary>
        /// Returns the GameObject instance corresponding to the specified 
        /// game object id
        /// </summary>
        /// <param name="id">The game object's ID</param>
        /// <returns>The specified GameObject</returns>
        public GameObject GetObject(uint id)
        {
            return gameObjects[id];
        }


        /// <summary>
        /// Queries a rectangular region and retuns game object ids for
        /// all game objects within that region
        /// </summary>
        /// <param name="bounds">A bounding rectangle for the region of interest</param>
        /// <returns>An array of game object ids</returns>
        public uint[] QueryRegion(Rectangle bounds)
        {
            HashSet<uint> matches = new HashSet<uint>();

            // Find the minimal index in the horizontal axis list using binary search
            Bound left = new Bound(null, bounds.Left, BoundType.Min);

            int minHorizontalIndex = horizontalAxis.BinarySearch(left);
            if (minHorizontalIndex < 0)
            {
                // If the index returned by binary search is negative,
                // then our current bound value does not exist within the
                // axis (most common case).  The bitwise compliement (~) of
                // that index value indicates the index of the first element 
                // in the axis list *larger* than our bound, and therefore
                // the first potentailly intersecting item
                minHorizontalIndex = ~minHorizontalIndex;
            }

            Bound right = new Bound(null, bounds.Left, BoundType.Max);
            int maxHorizontalIndex = horizontalAxis.BinarySearch(right);
            if (maxHorizontalIndex < 0) maxHorizontalIndex = ~maxHorizontalIndex;

            for (int i = minHorizontalIndex; i < maxHorizontalIndex; i++)
            {
                matches.Add(horizontalAxis[i].Box.GameObjectID);
            }

            Bound top = new Bound(null, bounds.Left, BoundType.Min);
            int minVerticalIndex = verticalAxis.BinarySearch(top);
            if (minVerticalIndex < 0) minVerticalIndex = ~minVerticalIndex;

            Bound bottom = new Bound(null, bounds.Bottom, BoundType.Max);
            int maxVerticalIndex = verticalAxis.BinarySearch(bottom);
            if (maxVerticalIndex < 0) maxVerticalIndex = ~maxVerticalIndex;

            for (int i = minVerticalIndex; i < maxVerticalIndex; i++)
            {
                matches.Add(verticalAxis[i].Box.GameObjectID);
            }

            return matches.ToArray();
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Helper method that provides the next unique game object ID
        /// </summary>
        /// <returns>The next unique game object id</returns>
        public uint NextID()
        {
            uint id = objectCount;
            objectCount++;
            return id;
        }

        /// <summary>
        /// Factory method to create a player ship
        /// </summary>
        /// <param name="playerShipType">The type of player ship to create</param>
        /// <param name="position">The position of the player ship in the game world</param>
        /// <returns>The game object id of the player ship</returns>
        public PlayerShip CreatePlayerShip(PlayerShipType playerShipType, Vector2 position)
        {
            PlayerShip playerShip;
            uint id = NextID();

            switch (playerShipType)
            {
                case PlayerShipType.Shrike:
                    playerShip = new ShrikeShip(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The player ship type " + Enum.GetName(typeof(PlayerShipType), playerShipType) + " is not supported");
            }

            QueueGameObjectForCreation(playerShip);
            return playerShip;
        }

        /// <summary>
        /// Factory method for spawning a projectile
        /// </summary>
        /// <param name="projectileType">The type of projectile to create</param>
        /// <param name="position">The position of the projectile in the game world</param>
        /// <returns>The game object id of the projectile</returns>
        public Projectile CreateProjectile(ProjectileType projectileType, Vector2 position)
        {
            Projectile projectile;
            uint id = NextID();

            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    projectile = new Bullet(id, content, position);
                    break;

                case ProjectileType.Fireball:
                    projectile = new Fireball(id, content, position);
                    break;

                case ProjectileType.JetMinionBullet:
                    projectile = new JetMinionBullet(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The projectile type " + Enum.GetName(typeof(ProjectileType), projectileType) + " is not supported");
            }

            QueueGameObjectForCreation(projectile);
            return projectile;
        }

        /// <summary>
        /// Factory method for spawning enemies.
        /// </summary>
        /// <param name="enemyType">The type of enemy to spawn</param>
        /// <param name="position">The location to spawn the enemy</param>
        /// <returns></returns>
        public Enemy CreateEnemy(EnemyType enemyType, Vector2 position)
        {
            Enemy enemy;
            uint id = NextID();

            switch (enemyType)
            {
                case EnemyType.Dart:
                    enemy = new Dart(id, content, position);
                    break;
                
                case EnemyType.JetMinion:
                    enemy = new JetMinion(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The enemy type " + Enum.GetName(typeof(EnemyType), enemyType) + " is not supported");
            }

            QueueGameObjectForCreation(enemy);
            return enemy;
        }

        #endregion

        #region Axis List Helper Methods

        /// <summary>
        /// Sorts the axis using insertion sort - provided the list is
        /// already nearly sorted, this should happen very quickly 
        /// </summary>
        /// <param name="axis">The axis to sort</param>
        private void UpdateAxisLists()
        {
            HashSet<CollisionPair> overlaps = new HashSet<CollisionPair>();
            int i, j;

            // Sort the horizontal axis
            for (i = 1; i < horizontalAxis.Count; i++)
            {
                Bound bound = horizontalAxis[i];
                j = i - 1;

                // if our bound needs to be moved left... lower in the list
                while ((j >= 0) && horizontalAxis[j].CompareTo(bound) > 0)
                {
                    // What are we passing, and what are we passing it with?
                    if(horizontalAxis[j].Type == BoundType.Min && bound.Type == BoundType.Max)
                    {
                        // when a Max bound passes a min, we remove it from 
                        // the collision set
                        collisions.Remove(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    } 
                    else if(horizontalAxis[j].Type == BoundType.Max && bound.Type == BoundType.Min)  
                    {
                        // when a Min bound passes a Max, we add it to the 
                        // potential collision set
                        overlaps.Add(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }

                    // Shift the elment at j up the list by one index
                    horizontalAxis[j + 1] = horizontalAxis[j];
                    j--;
                }
                horizontalAxis[j + 1] = bound;
            }

            // Sort the vertical axis
            for (i = 1; i < verticalAxis.Count; i++)
            {
                Bound bound = verticalAxis[i];
                j = i - 1;

                // if our bound needs to be moved left... lower in the list
                while ((j >= 0) && verticalAxis[j].CompareTo(bound) > 0)
                {
                    // What are we passing, and what are we passing it with?
                    if (verticalAxis[j].Type == BoundType.Min && bound.Type == BoundType.Max)
                    {
                        // when a Max bound passes a min, we remove it from 
                        // the collision set
                        collisions.Remove(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }
                    else if (verticalAxis[j].Type == BoundType.Max && bound.Type == BoundType.Min)
                    {
                        // when a Min bound passes a Max, we add it to the 
                        // potential collision set
                        overlaps.Add(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }

                    // Shift the elment at j up the list by one index
                    verticalAxis[j + 1] = verticalAxis[j];
                    j--;
                }
                verticalAxis[j + 1] = bound;
            }

            // Check the potential overlaps for intersection
            foreach (CollisionPair pair in overlaps)
            {
                GameObject A = GetObject(pair.A);
                GameObject B = GetObject(pair.B);
                if (A.Bounds.Intersects(B.Bounds))
                {
                    collisions.Add(pair);
                }
            }

        }


        /// <summary>
        /// Inserts a new bound into an axis list.  The list is assumed to be
        /// sorted, so the method uses binary insertion for speed.
        /// </summary>
        /// <param name="axis">The axis list to insert the Bound into</param>
        /// <param name="bound">The Bound to insert</param>
        /// <returns>The index where the bound was inserted</returns>
        private int InsertBoundIntoAxis(List<Bound> axis, Bound bound)
        {
            // Use binary search for fast O(log(n)) indentification
            // of appropriate index for our bound
            int index = axis.BinarySearch(bound);
            if (index < 0)
            {
                // If the index returned by binary search is negative,
                // then our current bound value does not exist within the
                // axis (most common case).  The bitwise compliement (~) of
                // that index value indicates the index of the first element 
                // in the axis list *larger* than our bound, and therefore
                // the appropriate place for our item
                index = ~index;
                axis.Insert(index, bound);
            }
            else
            {
                // If the index returned by binary search is positive, then
                // we have another bound with the *exact same value*.  We'll
                // go ahead and insert at that position.
                axis.Insert(index, bound);
            }

            return index;
        }

        /// <summary>
        /// Enqueue our game object for creation at the start of the next update
        /// cycle.  
        /// </summary>
        /// <param name="go">The ready-to-spawn game object</param>
        private void QueueGameObjectForCreation(GameObject go)
        {
            createdGameObjects.Enqueue(go);
        }

        /// <summary>
        /// Adds a GameObject to the GameObjectManager
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        private void AddGameObject(GameObject gameObject)
        {
            uint id = gameObject.ID;

            // Store the game object in our list of all game objects
            gameObjects.Add(id, gameObject);

            // Create the game object's bounding box
            BoundingBox box = new BoundingBox(id, gameObject.Bounds);
            boundingBoxes.Add(id, box);

            // Store the game object's bounds in our horizontal axis list
            int leftIndex = InsertBoundIntoAxis(horizontalAxis, box.Left);
            int rightIndex = InsertBoundIntoAxis(horizontalAxis, box.Right);

            // Grab any game object ids for game objects whose bounds fall
            // between the min and max bounds of our new game object
            HashSet<uint> overlaps = new HashSet<uint>();
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                overlaps.Add(horizontalAxis[i].Box.GameObjectID);
            }

            // Store the new game object's bounds in our vertical axis list
            int topIndex = InsertBoundIntoAxis(verticalAxis, box.Top);
            int bottomIndex = InsertBoundIntoAxis(verticalAxis, box.Bottom);

            // Grab any game object ids for game objects whose bounds fall
            // between the min and max of our new game object - if they are
            // already on our overlap list, we know there is a collision!
            for (int i = topIndex + 1; i < bottomIndex; i++)
            {
                if (overlaps.Contains(verticalAxis[i].Box.GameObjectID))
                    collisions.Add(new CollisionPair(id, verticalAxis[i].Box.GameObjectID));
            }
        }

        /// <summary>
        /// Updates the position of a GameObject within the axis
        /// lists
        /// </summary>
        /// <param name="gameObjectID">The ID of the game object to update</param>
        private void UpdateGameObject(uint gameObjectID)
        {
            // Grab our bounding box
            BoundingBox box = boundingBoxes[gameObjectID];

            // Grab our game object
            GameObject go = gameObjects[gameObjectID];

            // Apply the changes to bounding box
            box.Left.Value = go.Bounds.Left;
            box.Right.Value = go.Bounds.Right;
            box.Top.Value = go.Bounds.Top;
            box.Bottom.Value = go.Bounds.Bottom;
        }

        /// <summary>
        /// Removes a Game object from the axis
        /// </summary>
        /// <param name="gameObject">The game object to remove</param>
        private void RemoveGameObject(GameObject gameObject)
        {
            uint id = gameObject.ID;

            // Remove the game object from our list of all game objects
            gameObjects.Remove(id);

            // Remove the game object from our collection of collisions
            int i = 0;
            while (i < collisions.Count)
            {
                CollisionPair pair = collisions.ElementAt(i);
                if (pair.A == id || pair.B == id)
                    collisions.Remove(pair);
                i++;
            }

            // Grab the game object's bounding box
            BoundingBox box = boundingBoxes[id];

            // Remove the game objects' bounds from our horizontal axis lists
            horizontalAxis.Remove(box.Left);
            horizontalAxis.Remove(box.Right);
            verticalAxis.Remove(box.Top);
            verticalAxis.Remove(box.Bottom);

            // Remove the game object's bounding box
            boundingBoxes.Remove(id);
        }

        #endregion

    }
}
=======
>>>>>>> updating everything
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
    /// A class for managing game objects
    /// </summary>
    public class GameObjectManager
    {
        ContentManager content;

        uint objectCount = 0;
        
        Dictionary<uint, GameObject> gameObjects;
        
        Queue<GameObject> createdGameObjects;
        Queue<GameObject> destroyedGameObjects;

        Dictionary<uint,BoundingBox> boundingBoxes;

        List<Bound> horizontalAxis;
        List<Bound> verticalAxis;

        HashSet<CollisionPair> horizontalOverlaps;
        HashSet<CollisionPair> verticalOverlaps;
        HashSet<CollisionPair> collisions;


        /// <summary>
        /// Constructs a new GameObjectManager instance
        /// </summary>
        /// <param name="content">A ContentManager to use in loading assets</param>
        public GameObjectManager(ContentManager content)
        {
            this.content = content;

            gameObjects = new Dictionary<uint, GameObject>();

            createdGameObjects = new Queue<GameObject>();
            destroyedGameObjects = new Queue<GameObject>();

            boundingBoxes = new Dictionary<uint, BoundingBox>();
            horizontalAxis = new List<Bound>();
            verticalAxis = new List<Bound>();

            horizontalOverlaps = new HashSet<CollisionPair>();
            verticalOverlaps = new HashSet<CollisionPair>();
            collisions = new HashSet<CollisionPair>();
        }


        /// <summary>
        /// Updates the GameObjectManager, and all objects in the game
        /// </summary>
        /// <param name="elapsedTime">The time between this and the previous frame</param>
        public void Update(float elapsedTime)
        {
            // Add newly created game objects
            while (createdGameObjects.Count > 0)
            {
                GameObject go = createdGameObjects.Dequeue();
                AddGameObject(go);
            }

            // Remove destroyed game objects
            while (destroyedGameObjects.Count > 0)
            {
                GameObject go = destroyedGameObjects.Dequeue();
                RemoveGameObject(go);
            } 
            
            // Update our game objects
            foreach (GameObject go in gameObjects.Values)
            {
                // Call the GameObject's own update method
                go.Update(elapsedTime);

                // Update the game object's bounds to reflect 
                // changes this frame
                UpdateGameObject(go.ID);
            }

            // Update our axis lists
            UpdateAxisLists();
        }


        /// <summary>
        /// Draws all game Objects
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(float elapsedTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject go in gameObjects.Values)
            {
                go.Draw(elapsedTime, spriteBatch);
            }
        }


        #region Query Methods

        /// <summary>
        /// A HashSet containing all unique collisions for
        /// the current frame
        /// </summary>
        public HashSet<CollisionPair> Collisions
        {
            get { return collisions; }
        }

        /// <summary>
        /// Returns the GameObject instance corresponding to the specified 
        /// game object id
        /// </summary>
        /// <param name="id">The game object's ID</param>
        /// <returns>The specified GameObject</returns>
        public GameObject GetObject(uint id)
        {
            return gameObjects[id];
        }


        /// <summary>
        /// Removes the indicated game object from the game
        /// </summary>
        /// <param name="id">The game object's ID</param>
        /// <returns>The specified GameObject</returns>
        public GameObject DestroyObject(uint id)
        {
            GameObject go = gameObjects[id];
            destroyedGameObjects.Enqueue(go);
            return go;
        }


        /// <summary>
        /// Queries a rectangular region and retuns game object ids for
        /// all game objects within that region
        /// </summary>
        /// <param name="bounds">A bounding rectangle for the region of interest</param>
        /// <returns>An array of game object ids</returns>
        public uint[] QueryRegion(Rectangle bounds)
        {
            HashSet<uint> matches = new HashSet<uint>();

            // Find the minimal index in the horizontal axis list using binary search
            Bound left = new Bound(null, bounds.Left, BoundType.Min);

            int minHorizontalIndex = horizontalAxis.BinarySearch(left);
            if (minHorizontalIndex < 0)
            {
                // If the index returned by binary search is negative,
                // then our current bound value does not exist within the
                // axis (most common case).  The bitwise compliement (~) of
                // that index value indicates the index of the first element 
                // in the axis list *larger* than our bound, and therefore
                // the first potentailly intersecting item
                minHorizontalIndex = ~minHorizontalIndex;
            }

            Bound right = new Bound(null, bounds.Left, BoundType.Max);
            int maxHorizontalIndex = horizontalAxis.BinarySearch(right);
            if (maxHorizontalIndex < 0) maxHorizontalIndex = ~maxHorizontalIndex;

            for (int i = minHorizontalIndex; i < maxHorizontalIndex; i++)
            {
                matches.Add(horizontalAxis[i].Box.GameObjectID);
            }

            Bound top = new Bound(null, bounds.Left, BoundType.Min);
            int minVerticalIndex = verticalAxis.BinarySearch(top);
            if (minVerticalIndex < 0) minVerticalIndex = ~minVerticalIndex;

            Bound bottom = new Bound(null, bounds.Bottom, BoundType.Max);
            int maxVerticalIndex = verticalAxis.BinarySearch(bottom);
            if (maxVerticalIndex < 0) maxVerticalIndex = ~maxVerticalIndex;

            for (int i = minVerticalIndex; i < maxVerticalIndex; i++)
            {
                matches.Add(verticalAxis[i].Box.GameObjectID);
            }

            return matches.ToArray();
        }

        #endregion

        #region Factory Methods

        /// <summary>
        /// Helper method that provides the next unique game object ID
        /// </summary>
        /// <returns>The next unique game object id</returns>
        private uint NextID()
        {
            uint id = objectCount;
            objectCount++;
            return id;
        }


        /// <summary>
        /// Helper method for enqueueing our game object for creation at the 
        /// start of the next update cycle.  
        /// </summary>
        /// <param name="go">The ready-to-spawn game object</param>
        private void QueueGameObjectForCreation(GameObject go)
        {
            createdGameObjects.Enqueue(go);
        }


        /// <summary>
        /// Factory method to create a player ship
        /// </summary>
        /// <param name="playerShipType">The type of player ship to create</param>
        /// <param name="position">The position of the player ship in the game world</param>
        /// <returns>The game object id of the player ship</returns>
        public PlayerShip CreatePlayerShip(PlayerShipType playerShipType, Vector2 position)
        {
            PlayerShip playerShip;
            uint id = NextID();

            switch (playerShipType)
            {
                case PlayerShipType.Shrike:
                    playerShip = new ShrikeShip(id, content);
                    break;

                default:
                    throw new NotImplementedException("The player ship type " + Enum.GetName(typeof(PlayerShipType), playerShipType) + " is not supported");
            }

            QueueGameObjectForCreation(playerShip);
            return playerShip;
        }


        /// <summary>
        /// Factory method for spawning a projectile
        /// </summary>
        /// <param name="projectileType">The type of projectile to create</param>
        /// <param name="position">The position of the projectile in the game world</param>
        /// <returns>The game object id of the projectile</returns>
        public Powerup CreatePowerup(PowerupType powerupType, Vector2 position)
        {
            Powerup powerup;
            uint id = NextID();

            switch (powerupType)
            {
                case PowerupType.Fireball:
                    powerup = new FireballPowerup(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The powerup type " + Enum.GetName(typeof(ProjectileType), powerupType) + " is not supported");
            }

            QueueGameObjectForCreation(powerup);
            return powerup;
        }


        /// <summary>
        /// Factory method for spawning a projectile
        /// </summary>
        /// <param name="projectileType">The type of projectile to create</param>
        /// <param name="position">The position of the projectile in the game world</param>
        /// <returns>The game object id of the projectile</returns>
        public Projectile CreateProjectile(ProjectileType projectileType, Vector2 position)
        {
            Projectile projectile;
            uint id = NextID();

            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    projectile = new Bullet(id, content, position);
                    break;

                case ProjectileType.Fireball:
                    projectile = new Fireball(id, content, position);
                    break;

                default:
                    throw new NotImplementedException("The projectile type " + Enum.GetName(typeof(ProjectileType), projectileType) + " is not supported");
            }

            QueueGameObjectForCreation(projectile);
            return projectile;
        }


        /// <summary>
        /// Factory method for spawning enemies.
        /// </summary>
        /// <param name="enemyType">The type of enemy to spawn</param>
        /// <param name="position">The location to spawn the enemy</param>
        /// <returns></returns>
        public Enemy CreateEnemy(EnemyType enemyType, Vector2 position)
        {
            Enemy enemy;
            uint id = NextID();

            switch (enemyType)
            {
                case EnemyType.Dart:
                    enemy = new Dart(id, content, position);
                    break;
                
                default:
                    throw new NotImplementedException("The enemy type " + Enum.GetName(typeof(EnemyType), enemyType) + " is not supported");
            }

            QueueGameObjectForCreation(enemy);
            return enemy;
        }

        #endregion

        #region Axis List Helper Methods

        /// <summary>
        /// Sorts the axis using insertion sort - provided the list is
        /// already nearly sorted, this should happen very quickly 
        /// </summary>
        /// <param name="axis">The axis to sort</param>
        private void UpdateAxisLists()
        {
            HashSet<CollisionPair> overlaps = new HashSet<CollisionPair>();
            int i, j;

            // Sort the horizontal axis
            for (i = 1; i < horizontalAxis.Count; i++)
            {
                Bound bound = horizontalAxis[i];
                j = i - 1;

                // if our bound needs to be moved left... lower in the list
                while ((j >= 0) && horizontalAxis[j].CompareTo(bound) > 0)
                {
                    // What are we passing, and what are we passing it with?
                    if(horizontalAxis[j].Type == BoundType.Min && bound.Type == BoundType.Max)
                    {
                        // when a Max bound passes a min, we remove it from 
                        // the collision set
                        collisions.Remove(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                        horizontalOverlaps.Remove(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    } 
                    else if(horizontalAxis[j].Type == BoundType.Max && bound.Type == BoundType.Min)  
                    {
                        // when a Min bound passes a Max, we add it to the 
                        // potential collision set
                        horizontalOverlaps.Add(new CollisionPair(horizontalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }

                    // Shift the elment at j up the list by one index
                    horizontalAxis[j + 1] = horizontalAxis[j];
                    j--;
                }
                horizontalAxis[j + 1] = bound;
            }

            // Sort the vertical axis
            for (i = 1; i < verticalAxis.Count; i++)
            {
                Bound bound = verticalAxis[i];
                j = i - 1;

                // if our bound needs to be moved left... lower in the list
                while ((j >= 0) && verticalAxis[j].CompareTo(bound) > 0)
                {
                    // What are we passing, and what are we passing it with?
                    if (verticalAxis[j].Type == BoundType.Min && bound.Type == BoundType.Max)
                    {
                        // when a Max bound passes a min, we remove it from 
                        // the collision set
                        collisions.Remove(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                        verticalOverlaps.Remove(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }
                    else if (verticalAxis[j].Type == BoundType.Max && bound.Type == BoundType.Min)
                    {
                        // when a Min bound passes a Max, we add it to the 
                        // potential collision set
                        verticalOverlaps.Add(new CollisionPair(verticalAxis[j].Box.GameObjectID, bound.Box.GameObjectID));
                    }

                    // Shift the elment at j up the list by one index
                    verticalAxis[j + 1] = verticalAxis[j];
                    j--;
                }
                verticalAxis[j + 1] = bound;
            }

            // Check the potential overlaps for intersection
            foreach (CollisionPair pair in verticalOverlaps)
            {
                    GameObject A = GetObject(pair.A);
                    GameObject B = GetObject(pair.B);
                    if (A.Bounds.Intersects(B.Bounds))
                    {
                        collisions.Add(pair);
                    }
            }
        }


        /// <summary>
        /// Inserts a new bound into an axis list.  The list is assumed to be
        /// sorted, so the method uses binary insertion for speed.
        /// </summary>
        /// <param name="axis">The axis list to insert the Bound into</param>
        /// <param name="bound">The Bound to insert</param>
        /// <returns>The index where the bound was inserted</returns>
        private int InsertBoundIntoAxis(List<Bound> axis, Bound bound)
        {
            // Use binary search for fast O(log(n)) indentification
            // of appropriate index for our bound
            int index = axis.BinarySearch(bound);
            if (index < 0)
            {
                // If the index returned by binary search is negative,
                // then our current bound value does not exist within the
                // axis (most common case).  The bitwise compliement (~) of
                // that index value indicates the index of the first element 
                // in the axis list *larger* than our bound, and therefore
                // the appropriate place for our item
                index = ~index;
                axis.Insert(index, bound);
            }
            else
            {
                // If the index returned by binary search is positive, then
                // we have another bound with the *exact same value*.  We'll
                // go ahead and insert at that position.
                axis.Insert(index, bound);
            }

            return index;
        }

        
        /// <summary>
        /// Helper method that adds a GameObject to the GameObjectManager
        /// </summary>
        /// <param name="gameObject">The Game Object to add</param>
        private void AddGameObject(GameObject gameObject)
        {
            uint id = gameObject.ID;

            // Store the game object in our list of all game objects
            gameObjects.Add(id, gameObject);

            // Create the game object's bounding box
            BoundingBox box = new BoundingBox(id, gameObject.Bounds);
            boundingBoxes.Add(id, box);

            // Store the game object's bounds in our horizontal axis list
            int leftIndex = InsertBoundIntoAxis(horizontalAxis, box.Left);
            int rightIndex = InsertBoundIntoAxis(horizontalAxis, box.Right);

            // Grab any game object ids for game objects whose bounds fall
            // between the min and max bounds of our new game object
            for (int i = leftIndex + 1; i < rightIndex; i++)
            {
                horizontalOverlaps.Add(new CollisionPair(id, horizontalAxis[i].Box.GameObjectID));
            }

            // Store the new game object's bounds in our vertical axis list
            int topIndex = InsertBoundIntoAxis(verticalAxis, box.Top);
            int bottomIndex = InsertBoundIntoAxis(verticalAxis, box.Bottom);

            // Grab any game object ids for game objects whose bounds fall
            // between the min and max of our new game object
            for (int i = topIndex + 1; i < bottomIndex; i++)
            {
                verticalOverlaps.Add(new CollisionPair(id, verticalAxis[i].Box.GameObjectID));
            }
        }


        /// <summary>
        /// Updates the position of a GameObject within the axis
        /// lists
        /// </summary>
        /// <param name="gameObjectID">The ID of the game object to update</param>
        private void UpdateGameObject(uint gameObjectID)
        {
            // Grab our bounding box
            BoundingBox box = boundingBoxes[gameObjectID];

            // Grab our game object
            GameObject go = gameObjects[gameObjectID];

            // Apply the changes to bounding box
            box.Left.Value = go.Bounds.Left;
            box.Right.Value = go.Bounds.Right;
            box.Top.Value = go.Bounds.Top;
            box.Bottom.Value = go.Bounds.Bottom;
        }

        /// <summary>
        /// Removes a Game object from the axis
        /// </summary>
        /// <param name="gameObject">The game object to remove</param>
        private void RemoveGameObject(GameObject gameObject)
        {
            uint id = gameObject.ID;

            // Remove the game object from our list of all game objects
            gameObjects.Remove(id);

            // Remove the game object from our collection of collisions
            int i = collisions.Count - 1;
            while (i >= 0)
            {
                CollisionPair pair = collisions.ElementAt(i);
                if (pair.A == id || pair.B == id)
                    collisions.Remove(pair);
                i--;
            }

            // Remove the game object from our collection of overlaps
            i = horizontalOverlaps.Count - 1;
            while (i >= 0)
            {
                CollisionPair pair = horizontalOverlaps.ElementAt(i);
                if (pair.A == id || pair.B == id)
                    horizontalOverlaps.Remove(pair);
                i--;
            }
            i = verticalOverlaps.Count - 1;
            while (i >= 0)
            {
                CollisionPair pair = verticalOverlaps.ElementAt(i);
                if (pair.A == id || pair.B == id)
                    verticalOverlaps.Remove(pair);
                i--;
            }

            // Grab the game object's bounding box
            BoundingBox box = boundingBoxes[id];

            // Remove the game objects' bounds from our horizontal axis lists
            horizontalAxis.Remove(box.Left);
            horizontalAxis.Remove(box.Right);
            verticalAxis.Remove(box.Top);
            verticalAxis.Remove(box.Bottom);

            // Remove the game object's bounding box
            boundingBoxes.Remove(id);
        }

        #endregion

    }
}
<<<<<<< HEAD
=======
>>>>>>> b5617c4156435a47d8ca773e55de9e922c9604c5:ScrollingShooter/ScrollingShooter/GameObjects/GameObjectManager.cs
>>>>>>> updating everything
