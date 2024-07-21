using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Viguar.Aircraft
{
    public class SegmentDisplay : MonoBehaviour
    {
        private enum SegmentDisplayLengths { _2, _3, _4, _5, }
        [SerializeField] SegmentDisplayLengths SegmentDisplayLength;
        
        private TMP_Text SegmentField;

        private void Start()
        {
            SegmentField = GetComponentInChildren<TMP_Text>();  
        }

        public void DisplayText(float textValue)
        {           
            switch (SegmentDisplayLength)
            {               
                case SegmentDisplayLengths._2:
                    SegmentField.text = int.Parse(Mathf.RoundToInt(textValue).ToString().Length > 2 ? Mathf.RoundToInt(textValue).ToString().Substring(0, 2) : Mathf.RoundToInt(textValue).ToString()).ToString("D" + 2);
                    break;
                case SegmentDisplayLengths._3:
                    SegmentField.text = int.Parse(Mathf.RoundToInt(textValue).ToString().Length > 3 ? Mathf.RoundToInt(textValue).ToString().Substring(0, 3) : Mathf.RoundToInt(textValue).ToString()).ToString("D" + 3);
                    break;
                case SegmentDisplayLengths._4:
                    SegmentField.text =  int.Parse(Mathf.RoundToInt(textValue).ToString().Length > 4 ? Mathf.RoundToInt(textValue).ToString().Substring(0, 4) : Mathf.RoundToInt(textValue).ToString()).ToString("D" + 4);
                    break;
                case SegmentDisplayLengths._5:
                    SegmentField.text = int.Parse(Mathf.RoundToInt(textValue).ToString().Length > 5 ? Mathf.RoundToInt(textValue).ToString().Substring(0, 5) : Mathf.RoundToInt(textValue).ToString()).ToString("D" + 5);
                    break;
            }
        }
    }
}
