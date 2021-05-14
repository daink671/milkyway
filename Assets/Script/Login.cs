using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using System;
//login
public class Login : MonoBehaviour
{

    public InputField EmailInputField;
   public InputField PasswordInputField;
    public Text ErrorMessage;
   public Button SubmitButton;
    
    // Start is called before the first frame update
    void Start()

    {

        SubmitButton.onClick.AddListener(() =>
        {
          StartCoroutine(LogIn(EmailInputField.text, PasswordInputField.text));

         });

  

         //StartCoroutine(LogIn("julianav@terra.com.br", "12345"));
    }
    public string Empty(string s)
    {
        s = "";
        return s;
    }

    public IEnumerator LogIn(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", email);
        form.AddField("loginPass", password);
        
        using (UnityWebRequest www = UnityWebRequest.Post("http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/login.php", form))
        {
            
           
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                //Debug.Log(www.error);
                
            }
            else
            {
                string text = www.downloadHandler.text;
        
              if(text.Contains("Wrong Credentials") || text.Contains("User does not exist"))
                {
                    ErrorMessage.text = "Username or Password not found.";
                    
                } else
                {
                    string[] info = www.downloadHandler.text.Split('\t');
                    PlayerInfo.email = email;
                    PlayerInfo.username = info[0].Split('>')[1]; ;
                    PlayerInfo.title = info[1];
                 
              
                    PlayerInfo.points = int.Parse(info[2].Split('<')[0]);
                   
                    SceneManager.LoadScene(2);
                }
                   
                    
                
                
            }
        }
        


    }




}
