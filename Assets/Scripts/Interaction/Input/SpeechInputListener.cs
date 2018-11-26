using HoloIslandVis;
using HoloIslandVis.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using System;
using HoloToolkit.Unity;
using System.Net;
using UnityEngine.Networking;
using System.Threading.Tasks;
using HoloIslandVis.Automaton;
using UnityEngine.Events;
using System.Text;
using System.IO;

namespace HoloIslandVis.Interaction.Input
{
    public class SpeechInputListener : SingletonComponent<SpeechInputListener>
    {
        public delegate void CustomSpeechInputHandler(SpeechInputEventArgs eventArgs);
        public event CustomSpeechInputHandler SpeechResponse = delegate { };

        private readonly string baseURL = "http://localhost:5005/";

        private DictationRecognizer m_DictationRecognizer;
        private TextToSpeech tts;
        private bool listening = false;
        string currentRasaResponse;
        string currentKeyWord;

        protected override void Awake()
        {
            base.Awake();
            tts = gameObject.AddComponent<TextToSpeech>();
            InitDictationRecognizer();
        }

        private void InitDictationRecognizer()
        {

            m_DictationRecognizer = new DictationRecognizer();

            m_DictationRecognizer.DictationResult += (text, confidence) =>
            {

                if (!listening)
                {
                    //words to activate the listening
                    if (text.Equals("hello") || text.Equals("test") || text.Equals("wilson") || text.Equals("island voice"))
                    {
                        listening = true;
                        Debug.Log("I am listening");
                        tts.StartSpeaking("I am listening");
                    }
                    else
                    if (text.StartsWith("hello") || text.StartsWith("test") || text.StartsWith("wilson") || text.Equals("island voice"))
                    {
                        int i = text.IndexOf(" ") + 1;
                        string str = text.Substring(i);
                        GetAPIResponse(str);
                        SpeechInputEventArgs eventArgs = new SpeechInputEventArgs(currentRasaResponse, currentKeyWord);
                        UnityMainThreadDispatcher.Instance.Enqueue(response => SpeechResponse(response), eventArgs);
                    }
                }
                else
                {
                    GetAPIResponse(text);
                    SpeechInputEventArgs eventArgs = new SpeechInputEventArgs(currentRasaResponse, currentKeyWord);
                    UnityMainThreadDispatcher.Instance.Enqueue(response => SpeechResponse(response), eventArgs);
                    listening = false;
                }
            };
            m_DictationRecognizer.DictationComplete += (DictationCompletionCause cause) =>
            {
                if(listening)
                    tts.StartSpeaking("I did not understand");
                m_DictationRecognizer.Start();
            };
            
            m_DictationRecognizer.Start();

        }

        private void GetAPIResponse(string voiceCommand)
        {
            WebRequest request = WebRequest.Create(
            baseURL + "webhooks/rest/webhook");
            request.ContentType = "application/json";
            request.Method = "POST";
            string postData = "{\"sender\":\"default\",\"message\":\"" + voiceCommand + "\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();

            //TODO: checked Status 
            //Debug.Log(((HttpWebResponse)response).StatusDescription);

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Debug.Log(responseFromServer);
            string textResponse = responseFromServer;
            reader.Close();
            dataStream.Close();
            response.Close();

            JSONObject jobj = JSONObject.Create(textResponse);

            currentRasaResponse = jobj[0].GetField("text").ToString().Replace("\"", "");
            if (currentRasaResponse.ToLower().StartsWith("match"))
            {
                //TODO: prototyping
                //currentRasaResponse = "MATCH (n:Units) RETURN n LIMIT 5";
                currentKeyWord = "find";
            }
            else
            {
                currentKeyWord = "utter";
            }
        }

    }

}
