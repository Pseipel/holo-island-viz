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
using UnityEngine.UI;

public class DisplayOnPanelTask : DiscreteInteractionTask
{

    
    public override void Perform(InputEventArgs eventArgs, Command command)
    {

        Debug.Log("DisplayOnPanelTask.Perform is triggered");
        SpeechInputEventArgs speechInputEventArgs = (SpeechInputEventArgs) eventArgs;

        Neo4JConnector neo4JConnector = new Neo4JConnector();
        
        List<Neo4JResponse> neo4jResponse = neo4JConnector.QueryDB(speechInputEventArgs.rasaResponse);
        Debug.Log("neo4jResponse.Count: " + neo4jResponse.Count);

        GameObject infoPanel = UserInterface.Instance.Panel;
        infoPanel.SetActive(true);

        infoPanel.transform.Find("Canvas").Find("Title").GetComponent<Text>().text = "";
        foreach (Neo4JResponse resp in neo4jResponse)
        {
            infoPanel.transform.Find("Canvas").Find("Maintext").GetComponent<Text>().text = resp.Description;
        }

    }
    
}
