using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    [SerializeField] Text clearTimeText;
    [SerializeField] Text pointText;
    // Start is called before the first frame update
    void Start()
    {
        clearTimeText.text = ((int)GameManager.Instance.clearTime).ToString() + " Sec";
        pointText.text = GameManager.Instance.point.ToString() + " Point";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
