using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Viguar.Aircraft;
using UnityEngine.UI;

public class DebugAlertImageSwapper : MonoBehaviour
{
    private AircraftBaseProcessor _configBaseProcessor;
    [SerializeField] private string _ConditionVariable;
    [SerializeField] private string _ConditionValue;
    [Space(10)]
    private Sprite _StateOffImage;
    [SerializeField] private Sprite _StateOnImage;
    private Image _ImageComponent;

    private void Start()
    {
        _configBaseProcessor = GetComponentInParent<AircraftBaseProcessor>();
        _ImageComponent = GetComponent<Image>();
        _StateOffImage = _ImageComponent.sprite;
    }

    private void Update()
    {
        SwapOnCondition();
    }

    private void SwapOnCondition()
    {
        _configBaseProcessor.DefineDebugDictionaryString();
        _configBaseProcessor.DebugStringDict.TryGetValue(_ConditionVariable, out string VariableString);
        if(VariableString == _ConditionValue)
        {
            _ImageComponent.sprite = _StateOnImage;
        }
        else
        {
            _ImageComponent.sprite = _StateOffImage;
        }
    }
    
}
