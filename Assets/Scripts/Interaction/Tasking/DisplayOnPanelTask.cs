using HoloIslandVis.Automaton;
using HoloIslandVis.Component.UI;
using HoloIslandVis.Input;
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
using static HoloIslandVis.Input.RasaResponse;
using UnityEngine.UI;

public class DisplayOnPanelTask : DiscreteInteractionTask
{
    string currentIntentName = "info"; 
    
    public override void Perform(InputEventArgs eventArgs, Command command)
    {
        Debug.Log("DisplayOnPanelTask.Perform is triggered");

        SpeechInputEventArgs speechInputEventArgs = (SpeechInputEventArgs) eventArgs;
        Neo4JConnector neo4JConnector = (new GameObject("APIHelperObject")).AddComponent<Neo4JConnector>();
        //Neo4JConnector neo4JConnector = new Neo4JConnector();
        this.currentIntentName = speechInputEventArgs.intentName;
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            neo4JConnector.QueryDB(speechInputEventArgs.rasaResponse, DisplayOnPanel);
        });

        //List <Neo4JResponse> neo4jResponse = neo4JConnector.GetAPIResponse(speechInputEventArgs.rasaResponse);

    }

    private void DisplayOnPanel(List<Neo4JResponse> neo4jResponse)
    {
        Debug.Log("displayOnPanel");
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
        GameObject infoPanel = UserInterface.Instance.Panel;
        infoPanel.SetActive(true);

        infoPanel.transform.Find("Canvas").Find("Title").GetComponent<Text>().text = this.currentIntentName;
        string panelMainText = "";
        //TODOS:
        // 1. Add elements to Panel>canvas>maintext
        // 2. add script to each element with "name" value (for finding the element later)
        // 3. Make these elements clickable (i.e. extend the oneTabCommand)
        // 4. trigger findElementFunction through click
        // 5. make Panel scrollable (resultlists can be very long)

        foreach (Neo4JResponse resp in neo4jResponse)
        {
            GameObject resultObject = new GameObject("result");
            resultObject.AddComponent<Text>().text = resp.Description;
            resultObject.AddComponent<Interactable>().name = resp.TargetName;
            resultObject.transform.SetParent(infoPanel.transform.Find("Canvas").Find("Maintext"));
            
                //resp.TargetName
                //panelMainText += resp.Description;
            }
            //infoPanel.transform.Find("Canvas").Find("Maintext").GetComponent<Text>().text = panelMainText;
        });
    }


}
