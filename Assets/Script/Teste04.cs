using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Teste04 : MonoBehaviour
{
    [SerializeField] private Text timerText;
    
    public List<QuestionsWithAnswers> QnA;
    public List<QuestionsWithAnswers> qwA;

    string[] info;

    [SerializeField] Text QuestionString;
    //[SerializeField] Button[] options;
    public GameObject[] options;
    public int currentQuestion;
    public int totalQuestions = 0;
    public int points;
    public int selectAnswerInt;
    int constellation = PlayerInfo.constellation;

    public string selectAnswer;
    
    //public Text QuestionString;
    public Text FinalScore;

    private float time = 0f;


    
    public GameObject QuizPanel;
    //public GameObject FinalPanel;

    // Start is called before the first frame update
   
    
    void Start()
    {

        // FinalPanel.SetActive(false);
       
        //CreateList();


    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timerText.text = ((int)time).ToString() + "  Sec";
    }
    private void Awake()
    {
        QnA = new List<QuestionsWithAnswers>();
        qwA = new List<QuestionsWithAnswers>();
        StartCoroutine(GetQuestionsList(constellation));



    }

    

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameOver()
    {
       // QuizPanel.SetActive(false);
      //  FinalPanel.SetActive(true);
       // FinalScore.text = points + "/" + totalQuestions;
    }
   
    public void Correct()
    {
        points += 1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }
    
    public void Wrong()
    {
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }

    void SetAnswersList()
    {
        GetCorrect();
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswersController>().isCorrect = false;
            options[i].transform.GetComponent<Text>().text = QnA[currentQuestion].Answers[i];
        
        if(QnA[currentQuestion].correct == i+1)
            {
                options[i].GetComponent<AnswersController>().isCorrect = true;

            }

        }
    
         
    }

    public void GetCorrect()
    {
       for (int i = 0; i< options.Length; i++)
        {
          if (options[i].GetComponent<Text>().text == (QnA[currentQuestion].CorrectAnswer))
            {
                QnA[currentQuestion].correct = i;
            }
        }
    }

    public void OnAnswerButtonClick(int idx)
    {
        for (int i = 0; i < options.Length; i++)
        {
            Debug.Log("Aqi" + idx);
            if (i == idx)
            {
                options[i].GetComponent<Text>().color = Color.red;
                selectAnswer = options[i].GetComponent<Text>().text;
                selectAnswerInt = i + 1;
                Debug.Log(selectAnswer);
            }
            else
            {
                options[i].GetComponent<Text>().color = Color.black;
            }
            options[i].GetComponent<Text>().color = (i == idx) ? Color.red : Color.black;


        }
    }
    void CreateList()
    {
        for (int i = 0; i < qwA.Count; i++)
        {
            int ind = int.Parse(qwA[i].Question.QuestionID);
            currentQuestion = i;
            StartCoroutine(GetAnswersList(ind));
        }
        Debug.Log(QnA.Count);
        
    }

    void generateQuestion()
    {

        if (QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            int ans = int.Parse(QnA[currentQuestion].Question.QuestionID);
            QuestionString.text = QnA[currentQuestion].Question.QuestionText;
            SetAnswersList();
            }
            else
            {
                GameOver();
                Debug.Log("End.");
            }


        
          
           
       
       
    
    }


    private IEnumerator GetAnswersList(int constellation)
    {
        
        QuestionsWithAnswers withAnswers = qwA[currentQuestion];
        string[] array;
        WWWForm form = new WWWForm();
        form.AddField("fkQuestionid", constellation);
        using (UnityWebRequest www = UnityWebRequest.Post("http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/getAnswers.php", form))

        {
            array = new string[www.downloadHandler.text.Split('\t').Length];
            yield return www.SendWebRequest();
            array = www.downloadHandler.text.Split('\t');

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {

                withAnswers.Answers = new string[5];
                for (int i = 0; i < 5; i++)

                {
                    
                    string[] teste = array[i].Split('.');
                    withAnswers.Answers[i] = teste[0];
                    
                    if (teste[1] == " 1")
                    {
                        withAnswers.CorrectAnswer = teste[0];

                    }
         

                }
               
               
               
                QnA.Add(withAnswers);


            }


           
        }

        if (QnA.Count > 7)
        {
            currentQuestion = 0;
            generateQuestion();
            totalQuestions = QnA.Count;
        }


    }
        private IEnumerator GetQuestionsList(int constellation)
    {
;
        string[] array;
        WWWForm form = new WWWForm();
        form.AddField("fkConstid", constellation);
        using (UnityWebRequest www = UnityWebRequest.Post("http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/getQuestions.php", form))

        {
             array = new string[www.downloadHandler.text.Split('\t').Length];
            yield return www.SendWebRequest();
            array = www.downloadHandler.text.Split('\t');
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {

                for (int i = 0; i < array.Length - 1; i++)
                {
                    QuestionsWithAnswers QAns = new QuestionsWithAnswers();
                    string[] items = array[i].Split('?');
                    Question question = new Question();
                    question.QuestionID = items[1].Trim();
                    question.QuestionText = items[0] + '?';
                    QAns.Question = question;

                    qwA.Add(QAns);
                 
                }


            }
           
        }

        CreateList();


    }








}
