using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


//class to load and display the learning text


public class Learning : MonoBehaviour
{
   // [SerializeField] private InfoData infoDatas; //array that will store the database info
    [SerializeField] private int currentQuestionNumber = 0; //int to iterate between the scenes
    [SerializeField] Text questionText; 
    //[SerializeField] Button[] answer;
    [SerializeField] GameObject endUI;
    [SerializeField] private string[] info;
    //[SerializeField] private string[] ArrayLearning;
    
    int constellation = PlayerInfo.constellation;

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(GetLearningText());
    }


    //get the text from the database
    public IEnumerator GetLearningText()
    {
        WWWForm form = new WWWForm();
        form.AddField("fkConstid", constellation);

        using (UnityWebRequest www = UnityWebRequest.Post("http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/learning.php", form))

        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                info = www.downloadHandler.text.Split('@');
                yield return new WaitForEndOfFrame();
                ShowQuestions();//after fetching the information, to start displaying it
            }

        }
    }
   
    //to display the text part
    void ShowQuestions()
    {
        questionText.text = info[currentQuestionNumber];  
    }

    //to change the text when the next button is clicked
    public void OnSubmitButtonClick()
    {
          currentQuestionNumber++;

       //to change screens in each part of the text
        if (currentQuestionNumber >= info.Length)
        {
            SceneChange(4);
            return;
        }
           ShowQuestions();
            }

    //method to change scenes
    public void SceneChange(int idx)
    {
        SceneManager.LoadScene(idx);
    }
}

//to build the array object that will store the information that will come from the database
[System.Serializable]
public class InfoData
{
    public SpaceInfoData[] Space;
}

[System.Serializable]
public class SpaceInfoData
{
    public int No;
    public string Question;

}