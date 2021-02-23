using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class UpdateGPSText : MonoBehaviour
    {

        public Text coordinates;

        // Update is called once per frame
        private void Update()
        {
            coordinates.text = "Lat : " + GPS.instance.latitude.ToString() + " Lon : " + GPS.instance.longitude.ToString();
        }
    }
