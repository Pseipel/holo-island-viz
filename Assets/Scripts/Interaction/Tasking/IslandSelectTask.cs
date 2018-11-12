﻿using HoloIslandVis.Automaton;
using HoloIslandVis.Component.UI;
using HoloIslandVis.Interaction;
using HoloIslandVis.Utility;
using HoloIslandVis.Visualization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandSelectTask : DiscreteInteractionTask
{
    public override void Perform(InputEventArgs eventArgs, Command command)
    {
        GameObject[] highlights = GameObject.FindGameObjectsWithTag("Highlight");
        foreach(GameObject highlight in highlights)
        {
            if (highlight.GetComponent<MeshRenderer>().enabled)
                highlight.GetComponent<MeshRenderer>().enabled = false;
        }

        GameObject currentFocus = RuntimeCache.Instance.CurrentFocus;
        GameObject infoPanel = UserInterface.Instance.Panel;
        Island island = currentFocus.GetComponent<Island>();
        Transform[] transforms = currentFocus.transform.GetComponentsInChildren<Transform>(true);
        foreach(Transform trans in transforms)
        {
            if (trans.gameObject.name == "Highlight")
            {
                trans.gameObject.GetComponent<MeshRenderer>().enabled = true;
                infoPanel.SetActive(true);
                infoPanel.transform.Find("Canvas").Find("Title").GetComponent<Text>().text = currentFocus.name;
                infoPanel.transform.Find("Canvas").Find("Maintext").GetComponent<Text>().text =
                    "This Bundle contains:\n"
                    + island.Regions.Count + " packages\n"
                    + island.CartographicIsland.Bundle.CompilationUnitCount + " compilation units\n"
                    + island.CartographicIsland.Bundle.ExportedPackages.Count + " exports\n"
                    + island.CartographicIsland.Bundle.ImportedPackages.Count + " imports\n"
                    + island.CartographicIsland.Bundle.ServiceComponents.Count + " services\n";
            }
        }
    }
}
