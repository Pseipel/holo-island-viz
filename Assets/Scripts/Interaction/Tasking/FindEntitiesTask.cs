﻿using HoloIslandVis.Automaton;
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

namespace HoloIslandVis.Interaction.Tasking
{
    public class FindEntitiesTask : DiscreteInteractionTask
    {
        //public List<Island> Islands;
        private TextToSpeech tts;
        private readonly float speed = 0.2f;

        //public Island FoundObject
        //{
        //    get; private set;
        //}

        public override void Perform(InputEventArgs eventArgs, Command command)
        {

            GameObject currentFocus = RuntimeCache.Instance.CurrentFocus;

            if (tts == null)
                tts = RuntimeCache.Instance.ContentSurface.AddComponent<TextToSpeech>();

            //if (Islands == null)
            //    Islands = RuntimeCache.Instance.Islands;

            //if (Islands == null)
            //    Islands = RuntimeCache.Instance.Islands;

            //FoundObject = null;
            SpeechInputEventArgs siea = (SpeechInputEventArgs)eventArgs;

            this.navigateToEntity(RuntimeCache.Instance.FindGameObjectByName(currentFocus.name));

        }


        //private void findBuilding(string buildingName)
        //{
        //    FoundObject = Islands.Find(island => island.Regions.Find(region => region.Buildings.Find(building => building.CompilationUnit.Name.ToLower().Contains(buildingName))));
        //    Debug.Log("found a Building: " + FoundObject.name + "[" + FoundObject.tag + "]");
        //}

        //private void findIsland(string islandName)
        //{
        //    Debug.Log("Looking for Bundle/Island");
        //    FoundObject = Islands.Find(island => island.CartographicIsland.Bundle.Name.ToLower().Contains(islandName));
        //    if (FoundObject != null)
        //    {
        //        Vector3 source = FoundObject.transform.position;
        //        Vector3 visualizationContainerPosition = RuntimeCache.Instance.VisualizationContainer.transform.position;
        //        Vector3 vectorToIsland = source - RuntimeCache.Instance.ContentSurface.transform.position;
        //        Vector3 target = visualizationContainerPosition - vectorToIsland;

        //        new Task(() =>
        //        {
        //            int loopcount = 0;
        //            bool dispatched = false;
        //            while (Vector3.Distance(target, visualizationContainerPosition) > 0.1f)
        //            {
        //                loopcount++;
        //                if (dispatched)
        //                {
        //                    Task.Delay(50);
        //                    continue;
        //                }

        //                dispatched = true;
        //                UnityMainThreadDispatcher.Instance.Enqueue(() =>
        //                {
        //                    UserInterface.Instance.ParsingProgressText.GetComponent<TextMesh>().text = "Loopcount: " + loopcount;
        //                    source = FoundObject.transform.position;
        //                    visualizationContainerPosition = RuntimeCache.Instance.VisualizationContainer.transform.position;
        //                    vectorToIsland = source - RuntimeCache.Instance.ContentSurface.transform.position;
        //                    target = visualizationContainerPosition - vectorToIsland;
        //                    RuntimeCache.Instance.VisualizationContainer.transform.position = Vector3.Lerp(RuntimeCache.Instance.VisualizationContainer.transform.position, new Vector3(target.x, RuntimeCache.Instance.VisualizationContainer.transform.position.y, target.z), speed);
        //                    dispatched = false;
        //                });
        //            }
        //        }).Start();
        //    }
        //    else
        //        tts.StartSpeaking("Nothing found for this name");
        //}

        //private void findRegion(string regionName)
        //{
        //    FoundObject = Islands.Find(island => island.Regions.Find(region => region.Package.Name.ToLower().Contains(regionName)));
        //    Debug.Log("found a Region: " + FoundObject.name + "[" + FoundObject.tag + "]");
        //}

        private void navigateToEntity(GameObject targetObject)
        {
            Vector3 source = targetObject.transform.position;
            Vector3 visualizationContainerPosition = RuntimeCache.Instance.VisualizationContainer.transform.position;
            Vector3 vectorToIsland = source - RuntimeCache.Instance.ContentSurface.transform.position;
            Vector3 target = visualizationContainerPosition - vectorToIsland;

            new Task(() =>
            {
                int loopcount = 0;
                bool dispatched = false;
                while (Vector3.Distance(target, visualizationContainerPosition) > 0.1f)
                {
                    loopcount++;
                    if (dispatched)
                    {
                        Task.Delay(50);
                        continue;
                    }

                    dispatched = true;
                    UnityMainThreadDispatcher.Instance.Enqueue(() =>
                    {
                        UserInterface.Instance.ParsingProgressText.GetComponent<TextMesh>().text = "Loopcount: " + loopcount;
                        source = targetObject.transform.position;
                        visualizationContainerPosition = RuntimeCache.Instance.VisualizationContainer.transform.position;
                        vectorToIsland = source - RuntimeCache.Instance.ContentSurface.transform.position;
                        target = visualizationContainerPosition - vectorToIsland;
                        RuntimeCache.Instance.VisualizationContainer.transform.position = Vector3.Lerp(RuntimeCache.Instance.VisualizationContainer.transform.position, new Vector3(target.x, RuntimeCache.Instance.VisualizationContainer.transform.position.y, target.z), speed);
                        dispatched = false;
                    });
                }
            }).Start();
        }
    }
}