using System;
using UnityEngine;
using Viguar.EditorTooling.GUITools.OverrideLabels;

[Serializable]
public class ConfigRadioTransmitter
{
    public TransmitterData[] _cRadioTransmitters; //The amount of transmitters! NOT (!) channels. An Airliner usually has 2 or 3. (I think). 
}

[Serializable]
public class TransmitterData
{
    [LabelOverride("Default Transmitter Frequency")]
    public int _cDefaultFrequency;    
}
