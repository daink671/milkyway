using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public float clearTime;
    //private float clearTime; - encapsulation
    public int point;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GetSaveData();
    }

    void GetSaveData()
    {
        if (PlayerPrefs.HasKey("clearTime"))
        {
            clearTime = PlayerPrefs.GetFloat("clearTime");
        }
        if (PlayerPrefs.HasKey("Point"))
        {
            point = PlayerPrefs.GetInt("Point");
        }
    }


    public void SetData()
    {
        PlayerPrefs.SetFloat("clearTime", clearTime);
        PlayerPrefs.SetInt("Point", point);
    }
}
