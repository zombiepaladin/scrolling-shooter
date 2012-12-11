//Twin Jet Flight Pattern Class:
//Coders: Nicholas Boen
//Date: 9/16/2012
//Time: 6:32 P.M.
//
//Handles the series of nodes that constitutes
//as a flight pattern for the boss

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace ScrollingShooter
{
    class BossFlightPattern
    {
        /// <summary>
        /// The proximity the boss must be to register as 'close enough'
        /// to the node
        /// </summary>
        private static int MaxProximity = 30;

        /// <summary>
        /// The list of nodes for the path
        /// </summary>
        private LinkedList<Vector2> nodeList;

        /// <summary>
        /// The current node that we're at
        /// </summary>
        private LinkedListNode<Vector2> currentNode;

        /// <summary>
        /// Creates a shiny new flight pattern class
        /// </summary>
        /// <param name="firstNode">The first node in the list</param>
        public BossFlightPattern(Vector2 firstNode)
        {
            nodeList = new LinkedList<Vector2>();
            nodeList.AddFirst(firstNode);
            currentNode = nodeList.First;
        }

        /// <summary>
        /// Creates a shiny new flight pattern class
        /// </summary>
        /// <param name="nodeCollection">An entire collection of nodes</param>
        public BossFlightPattern(List<Vector2> nodeCollection)
        {
            nodeList = new LinkedList<Vector2>(nodeCollection);
            currentNode = nodeList.First;
        }

        /// <summary>
        /// Adds a new node to the end of the list
        /// </summary>
        /// <param name="newNode">Node to add</param>
        public void Add(Vector2 newNode)
        {
            nodeList.AddLast(newNode);
        }

        /// <summary>
        /// Go to the Next Node, or cycle to first node if at the end
        /// </summary>
        public void NextNode()
        {
            if (currentNode.Next == null)
                currentNode = nodeList.First;
            else
                currentNode = currentNode.Next;
        }

        /// <summary>
        /// Gets the vector2 at the current node
        /// </summary>
        /// <returns></returns>
        public Vector2 GetVector()
        {
            return currentNode.Value;
        }

        /// <summary>
        /// Gets the Vecto2 indicating the direction to the next node
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 GetDirectionToNode(Vector2 position)
        {
            Vector2 directionToNode = currentNode.Value - position;
            directionToNode.Normalize();

            return directionToNode;
        }

        /// <summary>
        /// Checks the proximity of the passed in position to the current node
        /// </summary>
        /// <param name="position">The position to check</param>
        /// <returns></returns>
        public bool CheckProximity(Vector2 position)
        {
            Vector2 nodePosition = this.GetVector();

            Vector2 vectorToNode = nodePosition - position;

            if (vectorToNode.LengthSquared() < (MaxProximity * MaxProximity))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Finds the nearest node to position and sets the current node to it
        /// </summary>
        /// <param name="position">The position to check against</param>
        public void SetToNearestNode(Vector2 position)
        {
            Vector2 closestNodeVector = nodeList.First.Value;
            currentNode = nodeList.First;

            int currentMagnitude = 0;
            int smallestMagnitude = (int)(position - currentNode.Value).LengthSquared();

            while (currentNode != null)
            {
                currentMagnitude = (int)(position - currentNode.Value).LengthSquared();

                if (currentMagnitude <= smallestMagnitude)
                {
                    closestNodeVector = currentNode.Value;
                    smallestMagnitude = currentMagnitude;
                }

                currentNode = currentNode.Next;
            }

            currentNode = nodeList.FindLast(closestNodeVector);
            
        }

    }
}
