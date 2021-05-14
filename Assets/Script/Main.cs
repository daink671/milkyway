using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


//main
public class Main : MonoBehaviour
{
    public static Main Instance;
    
    public Connection Connection;
    
    void Start()
    {
        Instance = this;
        Connection = GetComponent<Connection>();
    }

    
}


    

