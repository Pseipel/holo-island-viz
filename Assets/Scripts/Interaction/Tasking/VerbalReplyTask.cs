using HoloIslandVis.Automaton;
using HoloIslandVis.Component.UI;
using HoloIslandVis.Interaction.Input;
using HoloIslandVis.Interaction;
using HoloIslandVis.Utility;
using HoloIslandVis.Visualization;
using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static HoloIslandVis.Interaction.Input.RasaResponse;

public class VerbalReplyTask : DiscreteInteractionTask
{
    private TextToSpeech tts;

    public override void Perform(InputEventArgs eventArgs, Command command)
    {
        SpeechInputEventArgs speechInputEventArgs = (SpeechInputEventArgs)eventArgs;
        Debug.Log("I said: " +speechInputEventArgs.rasaResponse);
        if (tts == null)
            tts = RuntimeCache.Instance.ContentSurface.AddComponent<TextToSpeech>();
        tts.StartSpeaking(speechInputEventArgs.rasaResponse);
    }
}
