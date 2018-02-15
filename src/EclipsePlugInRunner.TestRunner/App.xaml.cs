using System.Windows;
using EclipsePlugInRunner.Scripting;
using VMS.TPS;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.TestRunner
{
    public partial class App : System.Windows.Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Patient patient = null;  // Dummy reference to workaround ESAPI bug
            ScriptRunner.Run(new Script());
        }
    }
}
