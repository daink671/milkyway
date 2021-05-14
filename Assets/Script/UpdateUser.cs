using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateUser : MonoBehaviour
{

    private Button SubmitButton;
    private InputField UsernameField;
    private InputField PasswordField;
    private Text Message;

    // Start is called before the first frame update
    void Start()
    {

        SubmitButton.onClick.AddListener(() =>
        {
            StartCoroutine(UpdatePlayer(PasswordField.text, UsernameField.text));

        });

    }

    //ask pedro for if blank situation;


    public IEnumerator UpdatePlayer(string password, string username)
    {

        WWWForm form = new WWWForm();
        form.AddField("email", PlayerInfo.email);
        form.AddField("loginPass", password);
        form.AddField("userName", username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/updateUser.php", form))

        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                PlayerInfo.username = username;
                Debug.Log("Deu certo!");
                Debug.Log(PlayerInfo.username);

            }

        }
    }
}

    
