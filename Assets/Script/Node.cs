// See LICENSE file in the root directory
//
using UnityEngine;

namespace LGK.PathFinding.Example
{
    public class Node : INode
    {
        private bool m_Walkable;
        private readonly Vector2 m_WorldPosition;

        public NodePosition m_NodePosition;

        public bool IsHighlight;

        public Node(bool walkable, Vector2 worldPosition, byte row, byte column)
        {
            m_Walkable = walkable;
            m_WorldPosition = worldPosition;
            m_NodePosition.Row = row;
            m_NodePosition.Column = column;
        }

        public bool Walkable
        {
            get { return this.m_Walkable; }
            set { this.m_Walkable = value; }
        }

        public Vector2 WorldPosition
        {
            get { return m_WorldPosition; }
        }

        public NodePosition NodePosition
        {
            get { return m_NodePosition; }
        }
    }
}
