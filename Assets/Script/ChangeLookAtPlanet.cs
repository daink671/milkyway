using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLookAtPlanet : MonoBehaviour
{
    public GameObject target;
    void OnMouseDown()
    {
        LookAtPlanet.target = target;
        Camera.main.fieldOfView = Mathf.Clamp(60 * target.transform.localScale.x, 1, 100);
    }
}
