﻿using HoloIslandVis.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HoloIslandVis.Component.UI
{
    public class UserInterface : SingletonComponent<UserInterface>
    {
        public GameObject ContentSurface {
            get { return RuntimeCache.Instance.ContentSurface; }
            private set { }

        }
        public GameObject ParsingProgressText { get; private set; }
        public GameObject ScanInstructionText { get; private set; }
        public GameObject ScanProgressBar { get; private set; }
        public GameObject Panel { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            ParsingProgressText = GameObject.Find("ParsingProgressText");
            ScanInstructionText = GameObject.Find("ScanInstructionText");
            ScanProgressBar = GameObject.Find("ScanProgressBar");
            Panel = GameObject.Find("Panel");

            ScanInstructionText.SetActive(false);
            ScanProgressBar.SetActive(false);
            Panel.SetActive(false);

        }

        private void Testmethod()
        {
            Debug.Log("Button pressed");
        }
    }
}
