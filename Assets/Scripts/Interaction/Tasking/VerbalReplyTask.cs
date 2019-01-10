using HoloIslandVis.Automaton;
using HoloIslandVis.Input;
using HoloIslandVis.Interaction;
using HoloIslandVis.Utility;
using HoloToolkit.Unity;
using UnityEngine;

public class VerbalReplyTask : DiscreteInteractionTask
{
    private TextToSpeech tts;

    public override void Perform(InputEventArgs eventArgs, Command command)
    {
        SpeechInputEventArgs speechInputEventArgs = (SpeechInputEventArgs)eventArgs;
        Debug.Log("I said: " + speechInputEventArgs.rasaResponse);
        if (tts == null)
            tts = RuntimeCache.Instance.ContentSurface.AddComponent<TextToSpeech>();
        tts.StartSpeaking(speechInputEventArgs.rasaResponse);
    }
}
