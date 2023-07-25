using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveWayPoint : MonoBehaviour
{

    public class SaveWayPointNode
    {
        public Vector3 pos;
        public List<Vector3> neibors;
    }

    // Use this for initialization
    void Start()
    {
        string waypointTextPath = Directory.GetCurrentDirectory() + "Assets\\waypoints\\waypoint.txt";
        
        if(!File.Exists(waypointTextPath))
        {
            SaveWaypointName();
            //SaveByPosition();
            Debug.Log("Waypoints text file has been created.");
        }
    }

    void SaveWaypointName()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoint");

        StreamWriter sw = new StreamWriter("Assets/waypoints/waypoint.txt", false);
        string s = "";
        for (int i = 0; i < gos.Length; i++)
        {
            s = "";
            s += gos[i].name.Trim();
            s += "$";
            WayPoint wp = gos[i].GetComponent<WayPoint>();
            s += wp.floor;
            s += "$";
            s += wp.link;
            s += "$";
            s += wp.neighborList.Count;
            s += "$";
            for (int j = 0; j < wp.neighborList.Count; j++)
            {
                s += wp.neighborList[j].name;
                s += "$";
            }

            sw.WriteLine(s);
        }
        sw.Close();
    }

    void SaveWaypointPosition()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoint");

        StreamWriter sw = new StreamWriter("Assets/waypoints/waypoint.txt", false);
        sw.WriteLine(gos.Length);
        // FileStream fs = new FileStream("Assets/abc.txt", FileMode.Create);
        string s = "";
        for (int i = 0; i < gos.Length; i++)
        {
            s = "";
            s += gos[i].name.Trim();
            s += "$";
            s += gos[i].transform.position[0];
            s += "$";
            s += gos[i].transform.position[1];
            s += "$";
            s += gos[i].transform.position[2];
            s += "$";
            WayPoint wp = gos[i].GetComponent<WayPoint>();
            s += wp.floor;
            s += "$";
            s += wp.link;
            s += "$";
            s += wp.neighborList.Count;
            s += "$";
            for (int j = 0; j < wp.neighborList.Count; j++)
            {
                for (int k = 0; k < gos.Length; k++)
                {
                    if (wp.neighborList[j] == gos[k])
                    {
                        s += k.ToString();
                        s += " ";
                    }
                }
            }

            sw.WriteLine(s);
        }
        sw.Close();
    }

}
