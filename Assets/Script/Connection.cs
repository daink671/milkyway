using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//web
public class Connection : MonoBehaviour
{



    private string email = PlayerInfo.email;
    public int countFailure;
    public static Dictionary<string, string> QList;
    public Dictionary<string, string> AList;
    public string[] keys;


    // Start is called before the first frame update
    
    void Start()
    { 
        
    }
    private void Awake()
    {
        
    }



    private IEnumerator GetFields2(int constellation, string url, string field)
    {
        WWWForm form = new WWWForm();
        form.AddField(field, constellation);
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

                QList = new Dictionary<string, string>();

                for (int i = 0; i < info.Length - 1; i++)
                {

                    string[] items = info[i].Split('?');
                    QList.Add(items[1].Trim(), items[0] + '?');


                }
            }

        }

    }
        



    




   
   



   



   

   
    public IEnumerator GetUsers()
    {
        WWWForm form = new WWWForm();
        form.AddField("fkConstid", 0);
        string url = "http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/getQuestions.php";
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        // using (UnityWebRequest www = UnityWebRequest.Post("http://34.253.2.208/mycode.php", form))
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string[] info = www.downloadHandler.text.Split('\t');

                QList = new Dictionary<string, string>();

                for (int i = 0; i < info.Length - 1; i++)
                {

                    string[] items = info[i].Split('?');
                    QList.Add(items[1].Trim(), items[0] + '?');
                    Debug.Log(items[1]);
                }
            }
           
        }
    
    }
    

           
}

