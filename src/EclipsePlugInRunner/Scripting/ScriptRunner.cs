using EclipsePlugInRunner.ViewModels;

namespace EclipsePlugInRunner.Scripting
{
    public class ScriptRunner
    {
        public static void Run(object script)
        {
            var mainViewModel = new MainViewModel(script);
            CreateAndShowWindow(mainViewModel);
        }

        public static void Run(object script, string username, string password)
        {
            var mainViewModel = new MainViewModel(script, username, password);
            CreateAndShowWindow(mainViewModel);
        }

        private static void CreateAndShowWindow(MainViewModel vm)
        {
            var mainWindow = new Views.MainWindow(vm);
            mainWindow.Show();
        }
    }
}
