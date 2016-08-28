using EclipsePlugInRunner.ViewModels;

namespace EclipsePlugInRunner.Scripting
{
    public class ScriptRunner
    {
        public static void Run(object script)
        {
            var mainViewModel = new MainViewModel(script);
            var mainWindow = new Views.MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
