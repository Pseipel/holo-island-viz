using HoloIslandVis.Visualization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using UnityEngine;
using HoloIslandVis.Component.UI;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine.Networking;

namespace HoloIslandVis.Utility
{
    public class Neo4JConnector : MonoBehaviour
    {
        string baseURL = "http://localhost:7474/db/data/transaction/commit";
        List<Neo4JResponse> entries = new List<Neo4JResponse>();

        public void QueryDB(string query, Action<List<Neo4JResponse>> callback)
        {
            Debug.Log("queryDB is running");
            StartCoroutine(GetAPIResponse(query, callback));
        }

        //public List<Neo4JResponse> Connect(string query)
        //{
        //    GetAPIResponse(query);
        //    Debug.Log("entries.Count: " + entries.Count);
        //    return entries;

        //WebRequest request = WebRequest.Create(
        //baseURL);
        //request.ContentType = "application/json";
        //request.Method = "POST";
        //string postData = "{\"statements\": " +
        //    "[{\"statement\": " +
        //    "\"" + query + "\"," +
        //    "\"includeStats\": true}]}";
        //Debug.Log(postData);
        //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //request.ContentLength = byteArray.Length;
        //Stream dataStream = request.GetRequestStream();
        //dataStream.Write(byteArray, 0, byteArray.Length);
        //dataStream.Close();
        //WebResponse response = request.GetResponse();

        ////TODO: checked Status 
        ////Debug.Log(((HttpWebResponse)response).StatusDescription);

        //dataStream = response.GetResponseStream();
        //StreamReader reader = new StreamReader(dataStream);
        //string responseFromServer = reader.ReadToEnd();
        //string textResponse = responseFromServer;
        //reader.Close();
        //dataStream.Close();
        //response.Close();

        //JSONObject apiResponse = JSONObject.Create(textResponse).GetField("results")[0].GetField("data");
        //Debug.Log("apiResponse: " + apiResponse);
        //foreach (JSONObject dbEntry in apiResponse)
        //{
        //    entries.Add(new Neo4JResponse(dbEntry));
        //}
        //}


        public IEnumerator GetAPIResponse(string query, Action<List<Neo4JResponse>> callback)
        {
            entries = new List<Neo4JResponse>();

            Debug.Log("GetAPIRespons is triggered in neo4jconnector");
            string postData = "{\"statements\": " +
                "[{\"statement\": " +
                "\"" + query + "\"}]}";

            Debug.Log("postData: " + postData);

            using (UnityWebRequest www = UnityWebRequest.Put(
            baseURL, Encoding.UTF8.GetBytes(postData))
            )
            {
                www.method = "POST";
                www.SetRequestHeader("accept", "application/json; charset=UTF-8");
                www.SetRequestHeader("content-type", "application/json; charset=UTF-8");

                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    callback(entries);
                }
                else
                {
                    Debug.Log("received data");
                    JSONObject apiResponse = JSONObject.Create(www.downloadHandler.text).GetField("results")[0].GetField("data");
                    foreach (JSONObject dbEntry in apiResponse)
                    {
                        entries.Add(new Neo4JResponse(dbEntry));
                    }
                    callback(entries);
                }
            }
        }
    }


}