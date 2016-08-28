using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.Scripting
{
    internal class ScriptProxy
    {
        private readonly object _scriptInstance;
        private readonly MethodInfo _scriptRunMethod;

        public ScriptProxy(object scriptInstance)
        {
            _scriptInstance = scriptInstance;
            _scriptRunMethod = GetScriptRunMethod();
        }

        public void RunWithNewWindow(PlugInScriptContext scriptContext)
        {
            var scriptWindow = new Window();
            InvokeScriptRun(scriptContext, scriptWindow);
            scriptWindow.ShowDialog();
        }

        private MethodInfo GetScriptRunMethod()
        {
            return _scriptInstance.GetType().GetMethod("Run", new[]
            {
                typeof(User),
                typeof(Patient),
                typeof(Image),
                typeof(StructureSet),
                typeof(PlanSetup),
                typeof(IEnumerable<PlanSetup>),
                typeof(IEnumerable<PlanSum>),
                typeof(Window)
            });
        }

        private void InvokeScriptRun(PlugInScriptContext scriptContext, Window scriptWindow)
        {
            _scriptRunMethod.Invoke(_scriptInstance, new object[]
            {
                scriptContext.User,
                scriptContext.Patient,
                scriptContext.Image,
                scriptContext.StructureSet,
                scriptContext.PlanSetup,
                scriptContext.PlanSetupsInScope,
                scriptContext.PlanSumsInScope,
                scriptWindow
            });
        }
    }
}