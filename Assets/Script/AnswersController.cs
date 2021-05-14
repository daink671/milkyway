using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswersController : MonoBehaviour
{
    public bool isCorrect = false;
    public Teste04 quizMan;
    public void Answer(int idx)
    {
        quizMan.OnAnswerButtonClick(idx);
        if (isCorrect)
        {
            Debug.Log("Certo");
            quizMan.Correct();
        }
        else
        {
            Debug.Log("Wrong");
            quizMan.Correct();
        }
    }
}