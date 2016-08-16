﻿using EvoVI.PluginContracts;
using System;

namespace ShipSystemControl
{
    public class ShipSystemControl : IPlugin
    {
        #region GUID (readonly)
        readonly Guid GUID = new Guid("Ship System Control");
        #endregion


        #region Plugin Info
        public string Description
        {
            get
            {
                return "Enables the VI to control basic ship functions like switching weapons and intiating intiating jumps.";
            }
        }

        public Guid Id
        {
            get { return GUID; }
        }

        public string Name
        {
            get { return "Ship System Control"; }
        }

        public string Version
        {
            get { return "0.1"; }
        }
        #endregion


        #region Interface Functions
        public void Initialize()
        {
            // TODO: Build dialog tree
        }

        public void OnDialogAction(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e, EvoVI.classes.dialog.DialogNode originNode)
        {

        }

        public void OnGameDataUpdate()
        {

        }
        #endregion
    }
}
