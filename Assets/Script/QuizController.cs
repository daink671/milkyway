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

    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetQuizDataFromWeb());
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timerText.text = time.ToString();
    }

    

    IEnumerator GetQuizDataFromWeb()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://daink671.github.io/milkyway/JsonData-main/TextMatchGame/Questions.json");
        
        www.SetRequestHeader("Access-Control-Allow-Credentials", "true");
        www.SetRequestHeader("Access-Control-Allow-Headers", "Accept, Content-Type, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
        www.SetRequestHeader("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS");
        www.SetRequestHeader("Access-Control-Allow-Origin", "*");

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            www.Dispose();
        }
        else
        {
            var jsonData = (www.downloadHandler).text;
            questionDatas = JsonUtility.FromJson<QuestionsData>(jsonData);
            yield return new WaitForEndOfFrame();
            ShowQuestions();
        }
        
    }


    void ShowQuestions()
    {
        Init();

        var data = questionDatas.Space[currentQuestionNumber];
        questionText.text = data.Question;
        for (int i = 0; i < answer.Length; i++)
        {
            if (i < data.Select.Length)
            {
                answer[i].gameObject.SetActive(true);
                answer[i].GetComponent<Text>().text = (i + 1).ToString() +  ". " + data.Select[i];
            }
            else
            {
                answer[i].gameObject.SetActive(false);
            }
        }
    }


    public void OnAnswerButtonClick(int idx)
    {
        for (int i = 0; i < answer.Length; i++)
        {

            if (i == idx)
            {
                answer[i].GetComponent<Text>().color = Color.red;
                selectAnswer = answer[i].GetComponent<Text>().text.Substring(3);
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
        if (questionDatas.Space[currentQuestionNumber].Answer.ToUpper().Contains(selectAnswer.ToUpper()))
        {
            point += 20;
            Debug.Log("correct");
        }
        else
        {
            Debug.Log("wrong");
        }

        currentQuestionNumber++;

        if (currentQuestionNumber >= questionDatas.Space.Length)
        {
            Debug.Log("your point " + point.ToString());
            SceneChange(8);
            return;
        }
        
            ShowQuestions();
     


    }


    private void Init()
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