﻿using EvoVI.PluginContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EvoVI.classes.dialog;
using EvoVI.engine;

namespace Smalltalk
{
    public class Smalltalk : IPlugin
    {
        #region GUID (readonly)
        readonly Guid GUID = Guid.NewGuid();
        #endregion


        #region Plugin Info
        public string Description
        {
            get
            {
                return "This Plugin enables the VI to engage in smalltalk with the player.\n" +
                    "The conversations themselves don't affect the ship's operations and serve primarily to build the " +
                    "relationship between the player and the VI";
            }
        }

        public Guid Id
        {
            get { return GUID; }
        }

        public string Name
        {
            get { return "Smalltalk"; }
        }

        public string Version
        {
            get { return "0.1"; }
        }
        #endregion


        #region Interface Functions
        public void Initialize()
        {
            DialogTreeReader.BuildDialogTree(
                null,
                new DialogTreeBranch(
                    new DialogPlayer("{Hey|Hi}[ what's up?];Hello[ there]."),
                    new DialogTreeBranch(

                        new DialogVI("Hi", DialogBase.DialogImportance.NORMAL),                            
                        new DialogTreeBranch(
                                    
                            new DialogPlayer("Are you okay?"),                             
                            new DialogTreeBranch(
                                new DialogVI("Just a little bummed out. I want to be finished soon.")
                            )
                        ),
                            
                        new DialogTreeBranch(
                            new DialogPlayer("Whats new?;Any news?"),
                            new DialogTreeBranch(
                                new DialogVI("Nothing new at the western front.")
                            )
                        ),
                    
                        new DialogTreeBranch(
                            new DialogPlayer("Anyone there?", DialogBase.DialogImportance.NORMAL)
                        ),

                        new DialogTreeBranch(
                            new DialogVI("I can't right now", DialogBase.DialogImportance.NORMAL)
                        )
                    )
                ),

                new DialogTreeBranch(
                    new DialogPlayer("Start test"),
                    new DialogTreeBranch(
                        new DialogCommand("Smalltalk", DialogBase.DialogImportance.NORMAL, "Trigger Smalltalk")
                    )
                ),

                new DialogTreeBranch(
                    new DialogPlayer("Start test number two"),
                    new DialogTreeBranch(
                        new DialogCommand("Smalltalk", DialogBase.DialogImportance.NORMAL)
                    )
                )
            );
        }

        public void OnDialogAction(EvoVI.classes.dialog.DialogBase originNode)
        {
            SpeechEngine.Say("Oh my! My small-talk subroutine has been triggered.", false);
        }

        public void OnGameDataUpdate()
        {

        }
        #endregion
    }
}
