using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour
{

    public GameObject[] spaceShips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0)) { 
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
                if (hit.transform.gameObject == spaceShips[0])
                {
                    spaceShips[0].GetComponent<Animator>().enabled = true;
                    spaceShips[1].GetComponent<Animator>().enabled = false;

                }
                else {
                    spaceShips[0].GetComponent<Animator>().enabled = false;
                    spaceShips[1].GetComponent<Animator>().enabled = true;
                }
            }
        
        }
        
    }
}
