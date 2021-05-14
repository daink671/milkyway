using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


//class that will populate and display the questions/ answers


public class NextPlanet : MonoBehaviour
{
    public List<QuestionsWithAnswers> QnA; // list with the questions and answers
    public List<QuestionsWithAnswers> qwA; //list with the questions and no answers

    [SerializeField] private Text timerText;
  
    [SerializeField] private int currentQuestionNumber = 0;
    [SerializeField] Text questionText;
    [SerializeField] Button[] answer;
    [SerializeField] GameObject endUI;

    int constellation = PlayerInfo.constellation;
    public static int point = 0;
    string selectAnswer;
    int getCorrect;

    public static float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerInfo.constellation > 7)
        {
            SceneChange(4);
            return;
        }

    }

    // Update is called once per frame
   
    void Update()
    {
        time += Time.deltaTime;
        timerText.text = ((int)time).ToString() + " Sec"; //to keep the clock running
    }

    private void Awake()
    {
        //to instatiate the lists that are going to fetch the questions and answers from the database
        QnA = new List<QuestionsWithAnswers>();
        qwA = new List<QuestionsWithAnswers>();
        //start fetching
        StartCoroutine(GetQuestionsList(constellation));



    }

    //get the answer's list( int constellation = questionID)
    //To instantiate in every question
    private IEnumerator GetAnswersList(int constellation)
    {

        QuestionsWithAnswers withAnswers = qwA[currentQuestionNumber]; // list that contains the questions
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
                //loop to fetch and separate all the answers
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
        //to only start showing the questions when the list is fully populated
        if (QnA.Count > 7)
        {
            currentQuestionNumber = 0;
            ShowQuestions();
        }
    }


    //get the question's list( int constellation = constellation)
    //To fetch the question's list

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
                //array to loop the the questions, separate the fields and and in the first list
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
        //to create the list with answers
        CreateList();


    }
    //to start the answers's list
    void CreateList()
    {
        for (int i = 0; i < qwA.Count; i++)
        {
            int ind = int.Parse(qwA[i].Question.QuestionID);
            currentQuestionNumber = i;
            StartCoroutine(GetAnswersList(ind));
        }
        Debug.Log(QnA.Count);

    }

    //after the list was populated, to display the questions
    void ShowQuestions()
    {
        Init();//set all answers to black color for next questions

        
        var data = QnA[currentQuestionNumber];
        questionText.text = data.Question.QuestionText;
        for (int i = 0; i < answer.Length; i++)
        {
            if (i < data.Answers.Length) //populating each question on the screen
            {
                answer[i].gameObject.SetActive(true);
                answer[i].GetComponent<Text>().text = (i + 1).ToString() + ". " + data.Answers[i];
            }
            else
            {
                answer[i].gameObject.SetActive(false);
            }
        }
    }

//when the user selects an answer
    public void OnAnswerButtonClick(int idx)
    {
        for (int i = 0; i < answer.Length; i++)
        {

            if (i == idx)
            {
                answer[i].GetComponent<Text>().fontStyle = FontStyle.Bold;
                selectAnswer = answer[i].GetComponent<Text>().text.Substring(3);
           
           
            }
            else
            {
                answer[i].GetComponent<Text>().fontStyle = FontStyle.Normal;
            }
            //answer[i].GetComponent<Text>().color = (i == idx) ? Color.red : Color.black;


        }
    }

    //when the user clicks on next button
    public void OnSubmitButtonClick()
    {
        // check if the answer is correct
        if (QnA[currentQuestionNumber].CorrectAnswer.ToUpper().Contains(selectAnswer.ToUpper()))
        {
            Debug.Log(selectAnswer);
            Debug.Log(QnA[currentQuestionNumber].CorrectAnswer);
            point += 20;
            
        }
      

        currentQuestionNumber++;

        //to check if the quiz is ended or not
        if (currentQuestionNumber >= QnA.Count)
        {

            if(point > 50)
        {
                SceneChange(8);
            }
            if (point < 50)
            {
                SceneChange(9);
            } //if it is over
            return;
                
        }

        ShowQuestions(); //if it is not



    }

     



    private void Init()//make sure to show all possible answers as black color for next questions
    {
        selectAnswer = string.Empty;

        foreach (var answer in answer)
        {
            answer.GetComponent<Text>().fontStyle = FontStyle.Normal;
        }
    }

    //to manage the scene changes the the user clicks on next or ends the quiz
    public void SceneChange(int idx)
    {
        GameManager.Instance.clearTime = time;
        GameManager.Instance.point = point;
        GameManager.Instance.SetData();
        SceneManager.LoadScene(idx);
    }
}

