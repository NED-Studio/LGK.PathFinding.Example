// See LICENSE file in the root directory
//
using UnityEngine;

namespace LGK.PathFinding.Example
{
    public class Grid : MonoBehaviour
    {
        public bool displayGridGizmos;

        public LayerMask m_UnwalkableMask;
        public Vector2 m_WorldSize;
        public float m_NodeRadius;

        public byte m_RowSize, m_ColumnSize;

        Node[] m_Nodes;
        float m_NodeDiameter;

        public Node[] Nodes { get { return m_Nodes; } }

        public Node this[int row, int column]
        {
            get { return m_Nodes[row * column + column]; }
        }

        void OnValidate()
        {
            Setup();
        }

        void Awake()
        {
            Setup();
        }

        void Setup()
        {
            m_NodeDiameter = m_NodeRadius * 2;
            m_RowSize = (byte)Mathf.RoundToInt(m_WorldSize.x / m_NodeDiameter);
            m_ColumnSize = (byte)Mathf.RoundToInt(m_WorldSize.y / m_NodeDiameter);

            CreateGrid();
        }

        void CreateGrid()
        {
            Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * m_WorldSize.x / 2 - Vector2.up * m_WorldSize.y / 2;

            m_Nodes = new Node[m_RowSize * m_ColumnSize];
            for (byte x = 0; x < m_RowSize; x++)
            {
                for (byte y = 0; y < m_ColumnSize; y++)
                {
                    Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * m_NodeDiameter + m_NodeRadius) + Vector2.up * (y * m_NodeDiameter + m_NodeRadius);
                    bool walkable = (Physics2D.OverlapCircle(worldPoint, m_NodeRadius, m_UnwalkableMask) == null); // if no collider2D is returned by overlap circle, then this node is walkable

                    m_Nodes[x * m_ColumnSize + y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }


        public Node NodeFromWorldPoint(Vector2 worldPosition)
        {
            float percentX = (worldPosition.x + m_WorldSize.x / 2) / m_WorldSize.x;
            float percentY = (worldPosition.y + m_WorldSize.y / 2) / m_WorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((m_RowSize - 1) * percentX);
            int y = Mathf.RoundToInt((m_ColumnSize - 1) * percentY);
            return m_Nodes[x * m_ColumnSize + y];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector2(m_WorldSize.x, m_WorldSize.y));
            if (m_Nodes != null && displayGridGizmos)
            {
                for (ushort i = 0; i < m_Nodes.Length; i++)
                {
                    var n = m_Nodes[i];

                    Gizmos.color = n.IsHighlight ? Color.blue : (n.Walkable ? Color.white : Color.red);

                    Gizmos.DrawCube(n.WorldPosition, Vector3.one * (m_NodeDiameter - .1f));
                }
            }
        }

    }
}
