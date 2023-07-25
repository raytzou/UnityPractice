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
//#define DEBUG_SNODE_ENODE
//#define DEBUG_CURNODE
//#define DEBUG_PRINT_ALL_NEXTNODE
//#define DEBUG_NEXTNODE
//#define DEBUG_LOOP_PRINT


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarImplement : MonoBehaviour
{

    public LayerMask Ground;
    public LayerMask Wall;

    enum NodeStatus
    {
        None = -1, // lost in black hole
        Open,
        Close
    }

    class PathNode
    {
        public GameObject node;
        public List<PathNode> neighborList; // transfer from waypoint
        public int floor = 0; // not implement yet,
        public bool link = false; // not implement yet
        public float gScore = 0f; // g(current.node) = g(current.node.parent) + cost(current.node.parent, current.node)
                                  // The cost from Start to Current node
        public float hScore = 0f; // h(current.node) = | current.node.x - goal.node.x | + | current.node.y - goal.node.y |
                                  // The cost from Current node to End node
        public float fScore = 0f; // fScore = gScore + hScore
                                  // Combination of gScore and hScore
        

        //public Vector3 Position { get; set; } // I use GameObject node.transform.position, the Instance doesn't really need
        public PathNode ParentNode { get; set; }

        public NodeStatus nodeStatus; // calculate later after run algorithm
    }

    private List<PathNode> _nodeList;
    private List<PathNode> _openList = new(); // for implement A*, set private
    private List<PathNode> _closeList = new();
    private List<Vector3> _pathList = new(); // store every paths that node has been checked, clear everytime in BuildPath()
    //private List<List<Vector3>> _myTestPathList = new(); // two-dimension list for pathList, {start, end}

    private void Start()
    {
        _nodeList = new();
        GameObject[] waypointsArray = GameObject.FindGameObjectsWithTag("WayPoint");

        foreach (GameObject waypoint in waypointsArray) // every waypoints on scene
        {
            List<PathNode> neighbors = new();
            foreach (var neighbor in waypoint.GetComponent<WayPoint>().neighborList) // every neighbors in single waypoint
            {
                //Debug.LogError(waypoint.name + " " + neighbor.name);
                neighbors.Add(new PathNode { node = neighbor,  });
            }

            _nodeList.Add(new PathNode
            {
                node = waypoint,
                neighborList = neighbors,
            });
        }


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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // seems it detects two or more times?
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var bMouseHit = Physics.Raycast(ray, out RaycastHit hitInfo, 9999, Ground);
            
            if (bMouseHit)
            {
                Debug.Log("mouse hit: " + hitInfo.point);
                bool aStar = AStar(transform.position, hitInfo.point);

                Debug.LogError("aStar: " + aStar);
            }
        }
    }

    private bool AStar(Vector3 npcPos, Vector3 mouseHitPos)
    {
        PathNode startNode = GetNearestNode(npcPos);
        PathNode endNode = GetNearestNode(mouseHitPos);

#if DEBUG_SNODE_ENODE
        Debug.Log("start node: " + startNode.node.name + ", end node: " + endNode.node.name);
        Debug.Log("start node pos: " + startNode.node.transform.position);
        Debug.Log("end node pos: " + endNode.node.transform.position);
#endif

        // _pathList = new(); // why don't clear in the beginning?

        if (startNode is null || endNode is null)
        {
            Debug.LogError("AStar ERROR: cannot find the \"Start\" or \"End\" node, they probably drop in the black hole :<");
            return false;
        }
        else if(startNode.node == endNode.node)
        {
            Debug.Log("Start node is the same as end node.");
            var pos = startNode.node.transform.position;

            BuildPath(npcPos, mouseHitPos, startNode, endNode); // first possible path, start = end
            return true;
        }

        /*
            Open : priority queue for next node
            Closed : list of next node has had been checked
         */
        _openList = new();
        _closeList = new();
        ResetAllNodesInfo(); // clear in every round for new stats

        PathNode currentNode = startNode;

        _openList.Add(currentNode);

        while(_openList.Count > 0)
        {
            currentNode = GetBestNode();

#if DEBUG_CURNODE
            Debug.Log("currentNode: " + currentNode.node.name);
            Debug.Log("currentNode pos: " + currentNode.node.transform.position);
#endif

            if (currentNode == null)
            {
                Debug.LogError("Cannot find the best node, are every scores in node set?");
                return false;
            }
            else if(currentNode.node == endNode.node)
            {
                Debug.Log("Current node is the same as end node.");

                BuildPath(npcPos, mouseHitPos, startNode, endNode); // second possible path
                return true;
            }


            /*
              remove x from openset                                         //�Nx�`�I�q�N�Q���⪺�`�I���R��
              add x to closedset                                            //�Nx�`�I���J�w�g�Q���⪺�`�I
             */
            _closeList.Add(currentNode);
            _openList.Remove(currentNode);
            currentNode.nodeStatus = NodeStatus.Close;

            for (int i = 0; i < currentNode.neighborList.Count; i++) // Breadth-First Search algorithm, checking every nodes near by current node.
            {
                /*
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
                 */

                PathNode nextNode = currentNode.neighborList[i];

                if (nextNode.nodeStatus == NodeStatus.Close) // back route
                {
                    Debug.Log("cur node: " + currentNode.node.name + $", next node ({nextNode.node.name}) was closed.");
                    continue;
                }

                Vector3 currentToNext = nextNode.node.transform.position - currentNode.node.transform.position;
                var distance = currentToNext.magnitude;
                var tentativeGScore = currentNode.gScore + distance;

                Debug.LogError(nextNode.gScore + " " + tentativeGScore);
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                /// gScore of nextNode always zero
                if (nextNode.nodeStatus == NodeStatus.Open 
                    && tentativeGScore > nextNode.gScore
                    ) // bad next node, cost more than prediction
                {
                    /*#region CalculatingCurrentGScore
                    float currentGScore = 0f;

                    currentToNext = currentNode.node.transform.position - nextNode.node.transform.position;
                    currentGScore = currentNode.gScore + currentToNext.magnitude;

                    if (currentGScore < nextNode.gScore)
                    {
                        nextNode.gScore = currentGScore;
                        nextNode.fScore = nextNode.gScore + nextNode.hScore;
                        nextNode.ParentNode = currentNode;
                    }
                    #endregion*/
                    Debug.LogError("BAD NEXT NODE");
                    Debug.LogError("nextnode: " + nextNode.node.name + ", gScore: " + nextNode.gScore);
                    continue; 
                }

#if DEBUG_PRINT_ALL_NEXTNODE
                Debug.Log("cur node: " + currentNode.node.name + ", next node: " +  nextNode.node.name);
#endif

                /*
                 if tentative_is_better = true                             //�p�G�P�_����n
                    came_from[y] := x                                     //�Ny�]��x���l�`�I
                    g_score[y] := tentative_g_score                       //��sy����I���Z��
                    h_score[y] := heuristic_estimate_of_distance(y, goal) //���py����I���Z��
                    f_score[y] := g_score[y] + h_score[y]
                    add y to openset                                      //�Ny���J�N�Q���⪺�`�I��
                 */
                #region CalculatingScoresOfNextNode
                nextNode.gScore = tentativeGScore;
                currentToNext = endNode.node.transform.position - nextNode.node.transform.position;
                nextNode.hScore = currentToNext.magnitude;
                nextNode.fScore = nextNode.gScore + nextNode.hScore;
                #endregion

                nextNode.ParentNode = currentNode;
                _openList.Add(nextNode);
                nextNode.nodeStatus = NodeStatus.Open;

#if DEBUG_NEXTNODE
                Debug.Log("Next Node: " + nextNode.node.name);
                Debug.Log("NextNode pos: " + nextNode.node.transform.position);
#endif
            }

#if DEBUG_LOOP_PRINT
            Debug.Log($"open({_openList.Count}): ");
            foreach (var node in _openList) Debug.Log(node.node.name);
            Debug.Log($"close({_closeList.Count}): ");
            foreach (var node in _closeList) Debug.Log(node.node.name);
#endif
        }

        if(_pathList.Count > 0) return true; else return false;
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

#if DEBUG_NODE_INFO
        LogNodeAndNeighbors();
#endif
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

        foreach (var theNode in _nodeList)
        {
            //Debug.Log(theNode.node.name);
            var nodePos = theNode.node.transform.position;
            if (Physics.Linecast(thePos, nodePos, Wall)) continue; // from the position to node, if hit wall, then skip

            Vector3 vecNodeToPos = theNode.node.transform.position - thePos;
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
    private PathNode GetBestNode()
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
        // _myTestPathList.Add(new List<Vector3> { startPos, endPos }); // the line will be argued by Kevin lol
                                                                        // this will be much easier, but it can't add middle parent nodes (from start to end), and it always do single one line moving
        _pathList = new() { startPos }; // add startPostion first

        if (startNode.node == endNode.node)
            _pathList.Add(startPos);
        else
        {
            PathNode parentNode = endNode.ParentNode;

            while(parentNode != null) // fetch all parent nodes from current endNode
            {
                _pathList.Insert(1, parentNode.node.transform.position);
                parentNode = parentNode.ParentNode; // set as next parent node
            }
        }

        _pathList.Add(endPos); // after finishing add all parent nodes, add end position
    }

    private void OnDrawGizmos()
    {
        
    }
}
