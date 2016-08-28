namespace EclipsePlugInRunner
{
    public class ScriptRunner
    {
        public static void Run(object script)
        {
            var mainViewModel = new MainViewModel(script);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
