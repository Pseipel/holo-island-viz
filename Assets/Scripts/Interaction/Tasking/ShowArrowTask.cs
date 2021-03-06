﻿using HoloIslandVis.Automaton;
using HoloIslandVis.Input;
using HoloIslandVis.Interaction;
using HoloIslandVis.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloIslandVis.Interaction.Tasking
{
    public class ShowArrowTask : DiscreteInteractionTask
    {
        public override void Perform(InputEventArgs eventArgs, Command command)
        {
            GameObject currentFocus = RuntimeCache.Instance.CurrentFocus;

            if(currentFocus != null)
            {
                DependencyDock dependencyDock = currentFocus.GetComponent<DependencyDock>();

                if(dependencyDock != null)
                {
                    if (!dependencyDock.expanded)
                        dependencyDock.showAllDependencies();
                    else
                        dependencyDock.hideAllDependencies();
                }
            }

            Debug.Log("Showing/Hiding dependencies.");
        }
    }
}