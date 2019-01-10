using System.Collections;
using System.Collections.Generic;
using HoloIslandVis.Automaton;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using static HoloIslandVis.Input.RasaResponse;

namespace HoloIslandVis.Input
{
    public class SpeechInputEventArgs : InputEventArgs
    {
 
        public string rasaResponse;
        public string intentName;
        public KeywordType kt;

        public SpeechInputEventArgs(string rasaResponse, string intentName, KeywordType kt)
        {   
            this.rasaResponse = rasaResponse;
            this.intentName = intentName;
            this.kt = kt;
        }
    }
}