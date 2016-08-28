using System.Collections.Generic;
using System.Windows;
using EclipsePlugInRunner.TestScript;
using VMS.TPS.Common.Model.API;

namespace VMS.TPS
{
    public class Script
    {
        public void Execute(ScriptContext scriptContext, Window mainWindow)
        {
            Run(scriptContext.CurrentUser,
                scriptContext.Patient,
                scriptContext.Image,
                scriptContext.StructureSet,
                scriptContext.PlanSetup,
                scriptContext.PlansInScope,
                scriptContext.PlanSumsInScope,
                mainWindow);
        }

        // This method must be present in any plug-in script that wants to use
        // EclipsePlugInRunner (it couldn't be named Execute because it confuses Eclipse)
        public void Run(
            User user,
            Patient patient,
            Image image,
            StructureSet structureSet,
            PlanSetup planSetup,
            IEnumerable<PlanSetup> planSetupsInScope,
            IEnumerable<PlanSum> planSumsInScope,
            Window mainWindow)
        {
            var mainViewModel = new MainViewModel(planSetup);
            var mainView = new MainView(mainViewModel);
            mainWindow.Content = mainView;
        }
    }
}
