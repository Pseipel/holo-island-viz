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

namespace HoloIslandVis.Utility
{
    public class Neo4JConnector : SingletonComponent<Neo4JConnector>
    {
        string baseURL = "http://localhost:7474/db/data/transaction/commit";

        public List<Neo4JResponse> QueryDB(string query)
        {
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
                UserInterface.Instance.ParsingProgressText.GetComponent<TextMesh>().text = "Connector is started");
            Debug.Log("Connector is started");

            return Connect(query); ;
        }

        private List<Neo4JResponse> Connect(string query)
        {
            List<Neo4JResponse> entries = new List<Neo4JResponse>();
            WebRequest request = WebRequest.Create(
            baseURL);
            request.ContentType = "application/json";
            request.Method = "POST";
            string postData = "{\"statements\": " +
                "[{\"statement\": " +
                "\"" + query + "\"," +
                "\"includeStats\": true}]}";
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

            JSONObject apiResponse = JSONObject.Create(textResponse).GetField("results")[0].GetField("data");
            Debug.Log("apiResponse: " + apiResponse);
            foreach (JSONObject dbEntry in apiResponse)
            {
                entries.Add(new Neo4JResponse(dbEntry));
            }
            return entries;
        }
    }


}