/*
//Matlab�y��
 function A*(start,goal)
     closedset := the empty set                                        //�B�z�L�᪺�`�I
     openset := set containing the initial node                        //�N�n�Q���⪺�`�I���X�A��l�u�]�tstart
     came_from := empty map
     g_score[start] := 0                                               //g(n)
     h_score[start] := heuristic_estimate_of_distance(start, goal)     //�q�L���p��� ���ph(start)
     f_score[start] := h_score[start]                                  //f(n)=h(n)+g(n)�A�ѩ�g(n)=0�A�ҥH�ٲ�
     while openset is not empty                                        //��N�Q���⪺�`�I�s�b�ɡA����`��
         x := the node in openset having the lowest f_score[] value    //�b�N�Q���p�����X�����f(x)�̤p���`�I
         if x = goal                                                   //�Yx�����I�A����
             return reconstruct_path(came_from,goal)                   //��^��x���̨θ��|
         remove x from openset                                         //�Nx�`�I�q�N�Q���⪺�`�I���R��
         add x to closedset                                            //�Nx�`�I���J�w�g�Q���⪺�`�I
         for each y in neighbor_nodes(x)                               //�`���M���Px�۾F�`�I
             if y in closedset                                         //�Yy�w�Q���ȡA���L
                 continue
             tentative_g_score := g_score[x] + dist_between(x,y)       //�q�_�I��`�Iy���Z��

             if y not in openset                                       //�Yy���O�N�Q���⪺�`�I
                 tentative_is_better := true                           //�ȮɧP�_����n
             elseif tentative_g_score < g_score[y]                     //�p�G�_�I��y���Z���p��y����ڶZ��
                 tentative_is_better := true                           //�ȮɧP�_����n
             else
                 tentative_is_better := false                          //�_�h�P�_����t
             if tentative_is_better = true                             //�p�G�P�_����n
                 came_from[y] := x                                     //�Ny�]��x���l�`�I
                 g_score[y] := tentative_g_score                       //��sy����I���Z��
                 h_score[y] := heuristic_estimate_of_distance(y, goal) //���py����I���Z��
                 f_score[y] := g_score[y] + h_score[y]
                 add y to openset                                      //�Ny���J�N�Q���⪺�`�I��
     return failure
 
 function reconstruct_path(came_from,current_node)
     if came_from[current_node] is set
         p = reconstruct_path(came_from,came_from[current_node])
         return (p + current_node)
     else
         return current_node
 */

//#define DEBUG_NODE_INFO
#define DEBUG_SNODE_ENODE
#define DEBUG_PATHING_INFO
//#define DEBUG_PRINT_ALL_NEXTNODE
//#define DEBUG_THE_ADD_NEXTNODE
//#define DEBUG_LOOP_LISTS_INFO


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class AStarImplement : MonoBehaviour
{
    public LayerMask Ground;
    public LayerMask Wall;

    enum NodeStatus
    {
        None = -1, // have not checked yet, or the node just drop in black hole :<
        Open,
        Close
    }

    class PathNode
    {
        public GameObject waypoint; // the waypoint in game
        public List<PathNode> neighborList; // transfering GameObject to PathNode is mayhem
        public int floor = 0; // not implement yet,
        public bool link = false; // not implement yet
        public float gScore = 0f; // g(current.node) = g(current.node.parent) + cost(current.node.parent, current.node)
                                  // The cost from Start to Current node
        public float hScore = 0f; // h(current.node) = | current.node.x - goal.node.x | + | current.node.y - goal.node.y |
                                  // The cost from Current node to End node (estimate)
        public float fScore = 0f; // fScore = gScore + hScore
                                  // Combination of gScore and hScore
        public Vector3 position = Vector3.zero; // position of the node

        public PathNode ParentNode { get; set; } // A cute instance here but not field,
                                                 // cuz I'm not sure the ParentNode of the CurrentNode will be changed
                                                 // if NextNode has the same neighbor as CurrentNode?

        public NodeStatus nodeStatus; // calculate later after run algorithm
    }

    private List<PathNode> _nodeList;
    private List<PathNode> _openList = new(); // for implementing A* only, set private
    private List<PathNode> _closeList = new();
    private List<Vector3> _pathList = new(); // store every paths that node has been checked, clear everytime in BuildPath()
    private GameObject[] _waypointsArray;
    private Color _originWPColor;
    private Color _originNPCColor;

    private bool _aStar = false;

    private void Start()
    {
        _nodeList = new();
        _waypointsArray = GameObject.FindGameObjectsWithTag("WayPoint");
        _originWPColor = _waypointsArray[0].GetComponent<Renderer>().material.color;
        _originNPCColor = gameObject.GetComponent<Renderer>().material.color;

        foreach (GameObject waypoint in _waypointsArray) // every waypoints on scene
        {
            //List<PathNode> neighbors = new();
            PathNode node = new()
            {
                waypoint = waypoint,
                neighborList = new(),
                nodeStatus = NodeStatus.None,
                position = waypoint.transform.position
            };

            // cannot handle neighbors here, because every PathNode have not been initialized yet
            /*foreach (var neighbor in waypoint.GetComponent<WayPoint>().neighborList) // every neighbors in single waypoint
            {
                //Debug.LogError(waypoint.name + " " + neighbor.name);
                neighbors.Add(node)
            }*/

            _nodeList.Add(node);
        }

        InitNeighborListInsideNode(); // Now I do initialize neighbor list in every PathNode here.

#if DEBUG_NODE_INFO
        LogNodeAndNeighbors();
#endif
    }

#if DEBUG_NODE_INFO
    /// <summary>
    /// Print all nodes and their neighbors.
    /// </summary>
    private void LogNodeAndNeighbors()
    {
        foreach (var theNode in _nodeList)
        {
            foreach(var neighbor in theNode.neighborList)
            {
                Debug.Log(theNode.node.name + " " + theNode.nodeStatus + " " + neighbor.node.name);
            }
        }
    }
#endif

    /// <summary>
    /// May God mercy the pathetic 3D nested loop.
    /// </summary>
    private void InitNeighborListInsideNode()
    {
        foreach(var theNode in _nodeList)
        {
            foreach(var neighbor in theNode.waypoint.GetComponent<WayPoint>().neighborList)
            {
                foreach(var n in _nodeList)
                {
                    if (theNode == n || theNode.neighborList.Contains(n)) continue;
                    if (n.waypoint == neighbor)
                    {
                        theNode.neighborList.Add(n);
                        break;
                    }
                }
            }
        }

        Debug.Log("NeighborList in PathNode initialized");
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // seems it detects two or more times?
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool mouseHit = Physics.Raycast(ray, out RaycastHit hitInfo, 9999, Ground);

            #region reset colors and aStar boolean
            foreach (var wp in _waypointsArray)
                wp.GetComponent<Renderer>().material.color = _originWPColor;
            _aStar = false;
            gameObject.GetComponent<Renderer>().material.color = _originNPCColor;
            #endregion

            if (mouseHit)
            {
                Debug.Log("mouse hit: " + hitInfo.point);
                _aStar = AStar(transform.position, hitInfo.point);
                Debug.Log("aStar success? " + _aStar);
                //Debug.DrawLine(hitInfo.point, Vector3.up * 1000, Color.red, 1000);
                var tmpPos = hitInfo.point;
                tmpPos.y += 1;
                var effect = GameObject.Find("ShockWave");
                effect.transform.position = tmpPos;
                effect.transform.Find("Particle System").GetComponent<ParticleSystem>().Play();
            }
        }
    }

    private bool AStar(Vector3 npcPos, Vector3 mouseHitPos)
    {
        Debug.Log("AStar start");
        PathNode startNode = GetNearestNode(npcPos);
        PathNode endNode = GetNearestNode(mouseHitPos);

#if DEBUG_SNODE_ENODE
        Debug.Log("start node: " + startNode.waypoint.name + ", end node: " + endNode.waypoint.name);
        Debug.Log("start node pos: " + startNode.position);
        Debug.Log("end node pos: " + endNode.position);
#endif

        if (startNode is null || endNode is null)
        {
            Debug.LogError("AStar ERROR: cannot find the \"Start\" or \"End\" node, they probably drop in the black hole :<");
            return false;
        }
        else if(startNode.waypoint == endNode.waypoint)
        {
            Debug.Log("Start node is the same as end node.");
            var pos = startNode.position;

            BuildPath(npcPos, mouseHitPos, startNode, endNode); // first possible path, start = end
            return true;
        }

        /*
            Open : priority queue for next node, the node will be calculated its new gScore
            Closed : list of next node had been checked, and gScore was bad :<
            None: the node has not been checked yet, claculating its scores first time.
         */
        _openList = new();
        _closeList = new();
        ResetAllNodesInfo(); // clear in every round for new stats
        _openList.Add(startNode);

        while(_openList.Count > 0)
        {
            PathNode currentNode = PopBestNode();
            _closeList.Add(currentNode);

            if (currentNode == null)
            {
                Debug.LogError("Cannot find the best node, are every scores in node set?");
                return false;
            }
            else if(currentNode.waypoint == endNode.waypoint)
            {
#if DEBUG_PATHING_INFO
                Debug.Log("Current node is the same as end node, or the pathing is end.");
#endif

                BuildPath(npcPos, mouseHitPos, startNode, endNode); // second possible path
                return true;
            }

            for (int i = 0; i < currentNode.neighborList.Count; i++) // Breadth-First Search algorithm, checking every nodes near by current node.
            {
                PathNode nextNode = currentNode.neighborList[i];
                Vector3 currentToNext;

#if DEBUG_PATHING_INFO
                Debug.Log("currentNode: " + currentNode.waypoint.name + " nextNode: " + nextNode.waypoint.name + " status: " + nextNode.nodeStatus);
#endif

                if (nextNode.nodeStatus == NodeStatus.Close) // back route
                {
#if DEBUG_PATHING_INFO
                    Debug.Log($"nextNode ( {nextNode.waypoint.name} ) was closed. Skip.");
#endif
                    continue;
                }
                else if (nextNode.nodeStatus == NodeStatus.Open) // bad next node, cost more than prediction
                {
                    #region Calculating NewGScore
                    currentToNext = currentNode.position - nextNode.position;
                    float newGScore = currentNode.gScore + currentToNext.magnitude;

                    if (newGScore < nextNode.gScore) 
                    {
                        nextNode.gScore = newGScore;
                        nextNode.fScore = nextNode.gScore + nextNode.hScore;
                        nextNode.ParentNode = currentNode;
                    }
                    #endregion
#if DEBUG_PATHING_INFO
                    Debug.Log("bad next node :(");
                    Debug.Log("nextnode: " + nextNode.waypoint.name);
#endif

                    continue;
                }
                
#if DEBUG_PRINT_ALL_NEXTNODE
                Debug.Log("cur node: " + currentNode.node.name + ", next node: " + nextNode.node.name);
#endif

                #region Calculating Unchecked NextNode (status = none)
                currentToNext = currentNode.position - nextNode.position;
                nextNode.gScore = currentNode.gScore + currentToNext.magnitude;
                //currentToNext = endNode.node.transform.position - nextNode.node.transform.position;
                var hScorePredict = endNode.position - nextNode.position;
                nextNode.hScore = hScorePredict.magnitude;
                nextNode.fScore = nextNode.gScore + nextNode.hScore;
                #endregion

                nextNode.ParentNode = currentNode;
                _openList.Add(nextNode);
                nextNode.nodeStatus = NodeStatus.Open;

#if DEBUG_THE_ADD_NEXTNODE
                Debug.Log("Next Node: " + nextNode.node.name);
                Debug.Log("NextNode pos: " + nextNode.position);
#endif
            }

            currentNode.nodeStatus = NodeStatus.Close;
            _closeList.Add(currentNode);

#if DEBUG_LOOP_LISTS_INFO
            Debug.Log($"open({_openList.Count}): ");
            foreach (var node in _openList) Debug.Log(node.node.name);
            Debug.Log($"close({_closeList.Count}): ");
            foreach (var node in _closeList) Debug.Log(node.node.name);
#endif
        }

        Debug.Log("Path (node) count: " + _pathList.Count);
        return false;
    }

    /// <summary>
    /// Reset everything in node, for calculating new scores.
    /// </summary>
    private void ResetAllNodesInfo()
    {
        foreach(var theNode in _nodeList)
        {
            //theNode.ParentNode = null;
            theNode.fScore = 0.0f;
            theNode.gScore = 0.0f;
            theNode.hScore = 0.0f;
            theNode.nodeStatus = NodeStatus.None;
        }
    }

    /// <summary>
    /// Give the position to find nearest node.
    /// </summary>
    /// <param name="thePos">position</param>
    /// <returns>PathNode</returns>
    private PathNode GetNearestNode(Vector3 thePos)
    {
        PathNode node = null;

        float minDistance = int.MaxValue;

        foreach (var theNode in _nodeList) // Linear search, should do something for optimizing. What if the map is huge or nodes are many?
        {
            var nodePos = theNode.waypoint.transform.position;
            if (Physics.Linecast(thePos, nodePos, Wall)) continue; // from the position to node, if hit wall, then skip

            Vector3 vecNodeToPos = theNode.waypoint.transform.position - thePos;
            var distance = vecNodeToPos.magnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                node = theNode;
            }
        }

        return node;
    }

    /// <summary>
    /// Find the lowest fScore node in Open List
    /// </summary>
    /// <returns>The lowest fScore node.</returns>
    private PathNode PopBestNode()
    {
        PathNode bestNode = null;
        float minFScore = float.MaxValue;

        foreach (var theNode in _openList)
        {
            if(theNode.fScore < minFScore)
            {
                minFScore = theNode.fScore;
                bestNode = theNode;
            }
        }

        _openList.Remove(bestNode);

        return bestNode;
    }

    /// <summary>
    /// Build path and store the path in list. It's only used in each node while checking path by A*.
    /// </summary>
    private void BuildPath(Vector3 startPos, Vector3 endPos, PathNode startNode, PathNode endNode)
    {
        _pathList = new() { startPos }; // add startPostion first

        if (startNode.waypoint == endNode.waypoint)
            _pathList.Add(startPos);
        else
        {
            PathNode parentNode = endNode.ParentNode;

            while(parentNode != null) // fetch all parent nodes from current endNode
            {
                _pathList.Insert(1, parentNode.waypoint.transform.position);
                parentNode = parentNode.ParentNode; // set as next parent node
            }
        }

        _pathList.Add(endPos); // after finishing add all parent nodes, add end position
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (_aStar)
        {
            for (int i = 0; i < _pathList.Count - 1; i++)
            {
                var sPos = _pathList[i];
                sPos.y += 1; // can't see the line if don't set height lol
                var ePos = _pathList[i + 1];
                ePos.y += 1; 

                Gizmos.DrawLine(sPos, ePos);
                
                foreach (var wp in _waypointsArray)
                {
                    if(wp.transform.position == _pathList[i])
                    {
                        wp.GetComponent<Renderer>().material.color = Color.yellow;
                        break;
                    }
                }
            }

            gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
