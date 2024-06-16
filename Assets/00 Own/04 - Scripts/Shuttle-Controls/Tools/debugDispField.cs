using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Viguar.Aircraft
{
    public class debugDispField : MonoBehaviour
    {
        private TMP_Text textDisplayField;
        public aircraftDebugText.AircraftInfoField.InfoProperty propertyDisplay;

        private void Start()
        {
            textDisplayField = GetComponent<TMP_Text>();
        }

        public void displayDebugValue(string itext, Color color)
        {
            textDisplayField.text = itext;
            textDisplayField.color = color;
        }
    }
}
