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
            ScriptRunner.Run(new Script());
        }

        // Use an ESAPI object (Patient) so that ESAPI is referenced by this assembly
        // (the dummy method is public so that it's not optimized away in a Release build);
        // if ESAPI is not referenced, an exception is thrown by ESAPI
        public void FixEsapiBug(Patient patient) { }
    }
}
