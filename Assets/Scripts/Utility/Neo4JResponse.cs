﻿using HoloIslandVis.Visualization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloIslandVis.Utility
{
    public class Neo4JResponse: Interactable
    {
        public Neo4JResponse(JSONObject dbEntry)
        {
            this.Description = dbEntry.GetField("columns").ToString();
            this.Description = dbEntry.GetField("row").ToString();
        }

        public string ID
        {
            get; private set;
        }
        public string Description
        {
            get; private set;
        }
    }

}