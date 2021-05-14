using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class RegisterInput : MonoBehaviour
{
    public InputField UsernameInputField;
    public InputField PasswordInputField;
    public InputField EmailInputField;
    public Button Submit;
    public Text ErrorMessage;
    // Start is called before the first frame update
    void Start()
    {
        Submit.onClick.AddListener(() =>
        {
            StartCoroutine(Register(EmailInputField.text, UsernameInputField.text, PasswordInputField.text));

        });
    }


    bool IsValidEmail(string email)
    {
        try
        {

            if (email.Contains("."))
            {

                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            }
        catch
        {
            return false;
        }
        return true;
    }

    // Update is called once per frame
    IEnumerator Register(string email, string username, string password)
    {
        string error;
        if (!IsValidEmail(email))
        {
            ErrorMessage.text = "Please type a valid email";
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("userEmail", email);
            form.AddField("userName", username);
            form.AddField("userPass", password);

            using (UnityWebRequest www = UnityWebRequest.Post("http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/registerPlayer.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    error = www.downloadHandler.text;
                    Debug.Log(error);
                    if (error.Contains("Error") || error.Contains("This email") || error.StartsWith("All"))
                    {
                        ErrorMessage.text = error;
                    }
                    else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    }

                }
            }
        }

    }
}
