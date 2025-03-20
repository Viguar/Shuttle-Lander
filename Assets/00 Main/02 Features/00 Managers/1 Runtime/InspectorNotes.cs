using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorNotes : MonoBehaviour
{
    [TextArea(5, 100)] //Min Lines, Max Lines
    public string Notes;
}
