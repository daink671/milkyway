using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class EndController : MonoBehaviour
{
    [SerializeField] Text clearTimeText;
    [SerializeField] Text pointText;

    public Text Message;
    private string email = PlayerInfo.email;
    private string s;

    private Ranks[] ranks = new Ranks[11];

    private int Failure;
    private int IsCompleted;
    private int next;
    int playerPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        ranks[0] = new Ranks("Spaceflight Participant", 0);
        ranks[1] = new Ranks("Operator", 150);
        ranks[2] = new Ranks("Second Pilot", 350);
        ranks[3] = new Ranks("First Pilot", 600);
        ranks[4] = new Ranks("Orbital Module Astronaut", 900);
        ranks[5] = new Ranks("Flight Engineer", 1250);
        ranks[6] = new Ranks("Mission Specialist", 1650);
        ranks[7] = new Ranks("Major General", 2100);
        ranks[8] = new Ranks("Second Commander", 2600);
        ranks[9] = new Ranks("Spacecraft Commander", 3150);
        ranks[10] = new Ranks("General of the Space Force", 3750);
        int point = NextPlanet.point;
        int time = (int)(NextPlanet.time);
        Debug.Log(point);
        Debug.Log(time);
        EndGame(point,time);
    }










    public void EndGame(int points, int time)

    {
        Debug.Log(ranks[3].RankName);
        Debug.Log(PlayerInfo.points);
        int sum;
        if (points < 50)
        {
            IsCompleted = 0;
            if (PlayerInfo.failure < 3)
            {
                PlayerInfo.failure = PlayerInfo.failure + 1;
            }

            playerPoints = PlayerInfo.points;
            checkRank(playerPoints);
            StartCoroutine(UpdatePlayerHistory());

            s = "No one said it would be easy! But at least you tried.\n " +
                "Perhaps studying a little bit more you could be promoted to a " + ranks[next].RankName + "!";


            SceneManager.LoadScene(15);


        }
        else
        {
            IsCompleted = 1;
            PlayerInfo.failure = 0;
            Debug.Log(time);
            float t = (time * 2);
            Debug.Log(t);
            float flo = ((points / t)*100);
            sum = (int)flo;
            Debug.Log(sum);
            playerPoints = PlayerInfo.points;
            PlayerInfo.points = (playerPoints + sum);
            playerPoints += sum;
           string check = checkRank(playerPoints);

            PlayerInfo.constellation = PlayerInfo.constellation + 1;
            StartCoroutine(UpdatePlayerHistory());
            

            s = "As expected of you! Congratulations! As I thought, your future is bright in the space force!\n" +
                "And, you got " + sum + " at once!  Unbelieveble!";

            SceneManager.LoadScene(14);
        }
       
        Message.text = s;
    }

    public string checkRank(int playerpoints)
    {
        for (int i = 0; i < ranks.Length - 1; i++)
        {
            if (ranks[i].RankName == PlayerInfo.title)
            {
                next = i + 1;
            }
        }
        if (playerpoints >= ranks[next].Points)
        {
            PlayerInfo.title = ranks[next].RankName;
            s = s + "Congratulations on becoming a " + PlayerInfo.title + "! It's a well-deserved promotion!";
        }
        else
        {
            s = s + "It's just " + (ranks[next].Points - playerpoints) + "points after all! You can do it!";
        }
        return s;
    }

    void Success(int points)
    {

    }

    public IEnumerator UpdatePlayerHistory()
    {
        WWWForm form = new WWWForm();
        form.AddField("fkConstid", PlayerInfo.constellation);
        form.AddField("fkEmail", PlayerInfo.email);
        form.AddField("failed", PlayerInfo.failure);
        form.AddField("complete", IsCompleted);
        form.AddField("title", PlayerInfo.title);
        form.AddField("points", PlayerInfo.points);


        string url = "http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/updateQuest.php";
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
                Debug.Log("Success");


            }

        }

    }

}