// See LICENSE file in the root directory
//

using UnityEngine;

namespace LGK.PathFinding.Example
{
    public class PathFindingManager : MonoBehaviour
    {
#pragma warning disable 0649 // Disable because this will be injected
        [SerializeField] Grid m_Grid;
#pragma warning restore 0649

        IPathFinder<Node> m_PathFinder;
        InputState m_InputState;
        Node m_StartNode;
        Node m_TargetNode;
        Path<Node> m_Path = new Path<Node>(10);

        void Start()
        {
            m_PathFinder = new PathFinder<Node>(m_Grid.m_RowSize, m_Grid.m_ColumnSize, m_Grid.Nodes);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_InputState == InputState.StartPoint)
                {
                    m_StartNode = m_Grid.NodeFromWorldPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    m_InputState = InputState.TargetPoint;

                    ShowNode(m_StartNode);
                }
                else if (m_InputState == InputState.TargetPoint)
                {
                    m_TargetNode = m_Grid.NodeFromWorldPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    m_InputState = InputState.Reset;

                    ShowNode(m_TargetNode);

                    m_PathFinder.Find(m_StartNode.NodePosition, m_TargetNode.NodePosition, m_Path);

                    DrawPath(m_Path);
                }
                else
                {
                    Reset();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                var node = m_Grid.NodeFromWorldPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                node.Walkable = !node.Walkable;

                Reset();
            }
        }

        void Reset()
        {
            m_InputState = InputState.StartPoint;

            HideNode(m_StartNode);

            HideNode(m_TargetNode);

            HidePath(m_Path);
        }

        void ShowNode(Node node)
        {
            if (node == null)
                return;

            node.IsHighlight = true;
        }

        void HideNode(Node node)
        {
            if (node == null)
                return;

            node.IsHighlight = false;
        }

        void DrawPath(Path<Node> path)
        {
            if (!path.IsReady)
                return;

            path.Reset();
            while (path.IsValid)
            {
                path.Current.IsHighlight = true;

                path.MoveNext();
            }
        }

        void HidePath(Path<Node> path)
        {
            if (!path.IsReady)
                return;

            path.Reset();
            while (path.IsValid)
            {
                path.Current.IsHighlight = false;

                path.MoveNext();
            }
        }

        enum InputState
        {
            StartPoint,
            TargetPoint,
            Reset
        }
    }
}
