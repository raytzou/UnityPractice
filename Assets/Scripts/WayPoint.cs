using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<GameObject> neighborList = new();
    public bool link = false;    
    public int floor = 0;

    private void OnDrawGizmos()
    {
        if(neighborList != null && neighborList.Count > 0)
        {
            Gizmos.color = Color.blue;

            foreach(var node in  neighborList)
            {
                Gizmos.DrawLine(transform.position, node.transform.position);
            }
        }
    }
}
