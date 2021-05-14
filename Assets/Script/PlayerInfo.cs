using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class PlayerInfo : MonoBehaviour
{
    public static string username;
    public static string email;
    public static string title;
    public static int points;
    public static int constellation;
    public static int failure;
    public  bool LoggedIn { get { return email != null; } }
    public  void LogOut()
    {
        email = null;
    }
    public PlayerInfo()
    {
        points = 0;
   
    }
    
}

