using HoloIslandVis.Visualization;
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
            this.Description = dbEntry.GetField("row").ToString();
            this.TargetName = dbEntry.GetField("row").GetField("row")[0].GetField("name").ToString();
        }

        public string TargetName
        {
            get; set;
        }
        public string Description
        {
            get; private set;
        }
    }

}