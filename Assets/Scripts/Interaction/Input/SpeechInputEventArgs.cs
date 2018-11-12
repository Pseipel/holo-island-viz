using System.Collections;
using System.Collections.Generic;
using HoloIslandVis.Interaction.Input;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using static HoloIslandVis.Interaction.Input.RasaResponse;

namespace HoloIslandVis.Interaction.Input
{
    public class SpeechInputEventArgs : InputEventArgs
    {
 
        public string rasaResponse;
        public string keyWord;


        //public List<Entity> entities;
        //public double intentionConfidence;

        public SpeechInputEventArgs(string rasaResponse, string keyWord)
        {   
            this.rasaResponse = rasaResponse;
            this.keyWord = keyWord;
            //this.entities = response.Entities;
            //this.intentionConfidence = response.IntentConfidence;
        }
    }
}