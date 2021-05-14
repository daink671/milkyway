using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Main02 : MonoBehaviour
{
    
    public Text Username; 
    
    // Start is called before the first frame update
    void Start()
    {
       
       Username.text = "Welcome, "+ PlayerInfo.title + "\n " + SetName();
        if (PlayerInfo.constellation != -1)
        {
            StartCoroutine(GetPlanet());
            Debug.Log(PlayerInfo.constellation);
        }
        else
        {
            PlayerInfo.constellation = 0;
            Debug.Log(PlayerInfo.constellation);
        }
        
        
    }
    private string SetName()
    {
        string setS = PlayerInfo.username;
        return setS;
    }


    //to get the last completed planet, will return -1 if there is no previous planet
    public IEnumerator GetPlanet()
    {
        WWWForm form = new WWWForm();
        form.AddField("fkEmail", PlayerInfo.email);
        string url = "http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/lastConst.php";
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))

        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string[] info = www.downloadHandler.text.Split('\t');
                int planet = int.Parse(info[0]);
                
                PlayerInfo.constellation = planet + 1;
                Debug.Log(PlayerInfo.constellation);

            }
        }
    }
        

    }
