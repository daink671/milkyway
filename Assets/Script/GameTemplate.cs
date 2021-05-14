using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameTemplate : MonoBehaviour
{
    public Text UsernameField;
   
    public Button Game;
    public Button Game2;
    public Button Game3;

    private string email = PlayerInfo.email;
    // Start is called before the first frame update
    void Start()
    {
        GetNextPlanet();
        Game3.onClick.AddListener(() =>
        {
            PlayerInfo.constellation = -1;
            Debug.Log(PlayerInfo.constellation);
            StartCoroutine(RefreshUser());
            SceneManager.LoadScene(2);
            

        });
    }
    private void Awake()
    {
        Game3.gameObject.SetActive(false);
    }
    private void GetNextPlanet()
    {
        int planet = PlayerInfo.constellation;
        var u = (PlanetsEnum)planet-1;
        if (planet > 7)
        {
            UsernameField.text = "You're awesome! You've completed your journey! Get ready to comeback to Earth! Would you like to start over again?";
            Game.gameObject.SetActive(false);
            Game2.gameObject.SetActive(false);
            Game3.gameObject.SetActive(true);
            //g2.GetComponent<Button>().gameObject.SetActive(false);

        } else
        {
            UsernameField.text = u < 0 ? "Welcome! Are you ready to start your journey?\n" +
           "Let's go to " + (PlanetsEnum)(planet) + "!" : "The last planet you visited was " + u + ".\n Would you like to embark on a new journey to " + (PlanetsEnum)(planet) + "?";

        }

    }
    public IEnumerator RefreshUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", PlayerInfo.email);
        string url = "http://ec2-34-253-2-208.eu-west-1.compute.amazonaws.com/queries/updateHistory.php";
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
       
        {

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
                Debug.Log("Success");

                
                }
            


}


}
