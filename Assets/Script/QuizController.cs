using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class QuizController : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private QuestionsData questionDatas;
    [SerializeField] private int currentQuestionNumber = 0;
    [SerializeField] Text questionText;
    [SerializeField] Button[] answer;
    [SerializeField] GameObject endUI;

    int point = 0;
    string selectAnswer;
    int selectAnswerInt;
    private string email = PlayerInfo.email;
    public int countFailure;
    public static Dictionary<string, string> QList;
    
    int constellation;
    public string[] keySet;
    private float time = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
//QorA(1, 1);
//QorA(1, 1);
        //Debug.Log(QList.Count);
    }

    void Start()
    {
        
        Debug.Log(QList.Count);
        Keys(1);
        //GetQuestionText();

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timerText.text = ((int)time).ToString() + " Sec";
    }



   
    public void Keys(int option)
    {
        //question option 1, answer option 0
        int i = 0;

        keySet = new string[QList.Count];
        foreach (var kvalue in QList)
        {
            keySet[i] = kvalue.Key;

            i++;

        }
        Debug.Log("Aqui" + keySet[0]);

    }


   


    //IEnumerator GetQuizDataFromWeb()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get("https://daink671.github.io/milkyway/JsonData-main/TextMatchGame/Questions.json");

    //    www.SetRequestHeader("Access-Control-Allow-Credentials", "true");
    //    www.SetRequestHeader("Access-Control-Allow-Headers", "Accept, Content-Type, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
    //    www.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");
    //    www.SetRequestHeader("Access-Control-Allow-Origin", "*");

    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        www.Dispose();
    //    }
    //    else
    //    {
    //        var jsonData = (www.downloadHandler).text;
    //        questionDatas = JsonUtility.FromJson<QuestionsData>(jsonData);
    //        yield return new WaitForEndOfFrame();
    //        ShowQuestions();
    //    }

    //}



    void ShowQuestions(int constellation)
    {
        Init();//set all answers to black color for next questions
        this.constellation = constellation;
        //var data = questionDatas.Space[currentQuestionNumber];
        //questionText.text = data.Question;

        GetQuestionText();
        Debug.Log(questionText.text);
        //for (int i = 0; i < answer.Length; i++)
        //{
        //    if (i < data.Select.Length)
        //    {
        //        answer[i].gameObject.SetActive(true);
        //        answer[i].GetComponent<Text>().text = (i + 1).ToString() +  ". " + data.Select[i];
        //    }
        //    else
        //    {
        //        answer[i].gameObject.SetActive(false);
        //    }
        //}
    }   


    private void GetQuestionText()
    {
        Debug.Log(currentQuestionNumber);
        Debug.Log(keySet[0]);
        string value = null;
        string QText = QList.TryGetValue(keySet[currentQuestionNumber], out value) ? value : "";
        Debug.Log(QText);
        questionText.text = QText;
    }



    private void GetAnswerList(string Question)
    {
       // Ans = quizModeScript.getAnswers(int.Parse(Question));
        
        
    }
    public void OnAnswerButtonClick(int idx)
    {
        for (int i = 0; i < answer.Length; i++)
        {

            if (i == idx)
            {
                answer[i].GetComponent<Text>().color = Color.red;
                selectAnswer = answer[i].GetComponent<Text>().text.Substring(3);
                selectAnswerInt = i + 1;
                Debug.Log(selectAnswer);
            }
            else
            {
                answer[i].GetComponent<Text>().color = Color.black;
            }
            //answer[i].GetComponent<Text>().color = (i == idx) ? Color.red : Color.black;


        }
    }

    public void OnSubmitButtonClick()
    {
        //if answer is string
        if (questionDatas.Space[currentQuestionNumber].Answer.ToUpper().Contains(selectAnswer.ToUpper()))
        {
            Debug.Log(questionDatas.Space[currentQuestionNumber].Answer);
            point += 20;
            Debug.Log("correct");
        }
        //if answer is int
        else if (int.Parse(questionDatas.Space[currentQuestionNumber].Answer).Equals(selectAnswerInt))
        {
            Debug.Log(questionDatas.Space[currentQuestionNumber].Answer);
            point += 20;
            Debug.Log("correct");
        }
        else 
        { 
            Debug.Log("wrong");
        }

        currentQuestionNumber++;

        //this is for just showing current point
        if (currentQuestionNumber >= questionDatas.Space.Length)
        {
            Debug.Log("your point " + point.ToString());
            if (point >= 20) {
                SceneChange(8);
            } else {
                SceneChange(10);
            }
            
            return;
        }
        
        ShowQuestions(constellation);
     


    }


   


private void Init()//make sure to show all possible answers as black color for next questions
    {
        selectAnswer = string.Empty;

        foreach (var answer in answer)
        {
            answer.GetComponent<Text>().color = Color.black;
        }
    }


    public void SceneChange(int idx)
    {
        GameManager.Instance.clearTime = time;
        GameManager.Instance.point = point;
        GameManager.Instance.SetData();
        SceneManager.LoadScene(idx);
    }
}

[System.Serializable]
public class QuestionsData
{
    public SpaceData[] Space;
}



[System.Serializable]
public class SpaceData
{
    public int No;
    public string Question;
    public string Answer;
    public string[] Select;

}