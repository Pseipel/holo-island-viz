using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloIslandVis.Utility;

namespace HoloIslandVis.Input
{
    public class InputHandler : Singleton<InputHandler>
    {
        private SpeechType _activeSpeech;
        private GestureType _activeGesture;

        public void InvokeSpeechInputEvent(Action<SpeechInputEventArgs> action, SpeechInputEventArgs eventArgs)
        {
            // TODO: Implement callback for when API call is finished.
            _activeSpeech = SpeechType.None;
            eventArgs.ActiveGesture = _activeGesture;

            if(ValidateInputEvent(eventArgs))
                UnityMainThreadDispatcher.Instance.Enqueue(action, eventArgs);
        }

        public void InvokeGestureInputEvent(Action<GestureInputEventArgs> action, GestureInputEventArgs eventArgs)
        {
            _activeGesture = eventArgs.GestureType;

            if(ValidateInputEvent(eventArgs))
                UnityMainThreadDispatcher.Instance.Enqueue(action, eventArgs);
        }

        private bool ValidateInputEvent(GestureInputEventArgs eventArgs)
        {
            if (_activeSpeech != SpeechType.None)
                return false; 

            return true;
        }

        private bool ValidateInputEvent(SpeechInputEventArgs eventArgs)
        {
            return true;
        }
    }
}
