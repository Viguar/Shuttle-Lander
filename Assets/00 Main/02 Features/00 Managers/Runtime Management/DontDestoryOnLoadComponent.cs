using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOnLoadComponent : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
