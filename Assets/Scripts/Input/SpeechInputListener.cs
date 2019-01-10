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

namespace HoloIslandVis.Input
{
    public class SpeechInputListener : SingletonComponent<SpeechInputListener>
    {
        public delegate void CustomSpeechInputHandler(SpeechInputEventArgs eventArgs);
        public event CustomSpeechInputHandler SpeechResponse = delegate { };

        private readonly string baseURL = "http://localhost:5005/";

        private DictationRecognizer m_DictationRecognizer;
        private TextToSpeech tts;
        private bool listening = false;
        //string currentRasaResponse;
        //KeywordType kt;

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
                    if (text.Equals("hello") || text.Equals("test") || text.Equals("wilson") || text.Equals("rick"))
                    {
                        listening = true;
                        Debug.Log("I am listening");
                        tts.StartSpeaking("I am listening");
                    }
                    else
                    if (text.StartsWith("hello") || text.StartsWith("test") || text.StartsWith("wilson") || text.Equals("rick"))
                    {
                        int i = text.IndexOf(" ") + 1;
                        string str = text.Substring(i);
                        StartCoroutine("GetAPIResponse", text);
                        //UnityMainThreadDispatcher.Instance.Enqueue(
                        //    r => SpeechResponse(r), GetAPIResponse(text));
                        listening = false;
                    }
                }
                else
                {

                    StartCoroutine("GetAPIResponse", text);
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

        //private void GetAPIResponse(string voiceCommand)
        //{
        //    Debug.Log("GetAPIResponse");

        //    WebRequest request = WebRequest.Create(
        //    baseURL + "webhooks/rest/webhook");
        //    request.ContentType = "application/json";
        //    request.Method = "POST";
        //    string postData = "{\"sender\":\"default\",\"message\":\"" + voiceCommand + "\"}";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    request.ContentLength = byteArray.Length;
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse response = request.GetResponse();

        //    //TODO: checked Status 
        //    //Debug.Log(((HttpWebResponse)response).StatusDescription);

        //    dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string responseFromServer = reader.ReadToEnd();
        //    string textResponse = responseFromServer;
        //    reader.Close();
        //    dataStream.Close();
        //    response.Close();

        //    JSONObject jobj = JSONObject.Create(textResponse);
        //    string textFieldValue = jobj[0].GetField("text").ToString();
        //    currentRasaResponse = textFieldValue.Substring(1, textFieldValue.Length - 2);
        //    Debug.Log("currentRasaResponse: " + currentRasaResponse);

        //    if (currentRasaResponse.ToLower().StartsWith("match"))
        //    {
        //        currentKeyWord = "find";
        //    }
        //    else
        //    {
        //        currentKeyWord = "utter";
        //    }
        //    UnityMainThreadDispatcher.Instance.Enqueue(
        //        r => SpeechResponse(r),
        //        new SpeechInputEventArgs(currentRasaResponse, currentKeyWord));

        //}

        IEnumerator GetAPIResponse(string voiceCommand)
        {
            Debug.Log("GetAPIRespons is triggered");
            string postData = "{\"message\":\"count all compilation units\"}";

            using (UnityWebRequest www = UnityWebRequest.Put(
            baseURL + "webhooks/rest/webhook", Encoding.UTF8.GetBytes(postData))
            )
            {
                www.method = "POST";
                www.SetRequestHeader("accept", "application/json; charset=UTF-8");
                www.SetRequestHeader("content-type", "application/json; charset=UTF-8");

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("running donwloadhelper");
                    JSONObject jobj = JSONObject.Create(www.downloadHandler.text);
                    string textFieldValue = jobj[0].GetField("text").ToString();
                    string currentRasaResponse = textFieldValue.Substring(1, textFieldValue.Length - 2);
                    string intentName = jobj[1].GetField("text").ToString().Substring(1, jobj[1].GetField("text").ToString().Length - 2);

                    Debug.Log("currentRasaResponse: " + currentRasaResponse);
                    KeywordType kt = KeywordType.Invariant;
                    if (currentRasaResponse.ToLower().StartsWith("match"))
                    {
                        kt = KeywordType.Find;
                    }
                    else
                    {
                        kt = KeywordType.Utter;
                    }

                    UnityMainThreadDispatcher.Instance.Enqueue(
                        r => SpeechResponse(r),
                        new SpeechInputEventArgs(currentRasaResponse, intentName, kt));

                }
            }
        }

    }

}
