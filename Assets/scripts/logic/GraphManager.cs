﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GraphManager : MonoBehaviour
{
    public GameObject ConnectionPrefab;

    public static GraphManager Instance
    {
        get
        {

            if (m_Instance == null)
            {
                GameObject obj = GameObject.Find("GraphManager");
                if (obj != null) m_Instance = obj.GetComponent<GraphManager>(); 
            }
            return m_Instance;
        }
    }
    private static GraphManager m_Instance;

    private Node[] m_Nodes;
    private Dictionary<Node, List<Connection>> m_Connections;

    public List<Connection> GetConnections(Node n1)
    {
        return m_Connections[n1];
    }


    // Called from the editor to set up a connection 
    public void CreateConnection(GameObject connObj, Node n1, Node n2, ConnectionType type)
    {
        connObj.transform.SetParent(this.transform);
        Connection c = connObj.GetComponent<Connection>();
        c.Set(n1, n2, type);
        c.gameObject.name = "Connection " + n1.gameObject.name + " to " + n2.gameObject.name;
    }


    void Awake()
    {
        m_Connections = new Dictionary<Node, List<Connection>>();

        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        m_Nodes = new Node[nodes.Length];

        for (int i = 0; i < nodes.Length; ++i)
        {
            m_Nodes[i] = nodes[i].GetComponent<Node>();
            if ( m_Nodes[i] == null )
            {
                Debug.LogError("Error: Node " + nodes[i].name + " has a node tag but no Node component!");
            } else
            {
                m_Connections[m_Nodes[i]] = new List<Connection>();
            }
        }

        GameObject[] conns = GameObject.FindGameObjectsWithTag("Connection");
        int connCount = 0;
        for (int i = 0; i < conns.Length; ++i)
        {
            Connection c = conns[i].GetComponent<Connection>();
            if ( c != null )
            {
                if ( c.m_Node1 == null || c.m_Node2 == null)
                {
                    Debug.LogError("Error: Connection " + conns[i].name + " has invalid nodes!");
                } else
                {
                    ++connCount;
                    m_Connections[c.m_Node1].Add(c);
                    m_Connections[c.m_Node2].Add(c);
                }
            } else
            {
                Debug.LogError("Error: Connection " + conns[i].name + " has a connection tag but no Connection component!");
            }
        }

        Debug.Log(m_Nodes.Length + " nodes and " + connCount + " connections initialized");
    }

    void Update()
    {

    }
}