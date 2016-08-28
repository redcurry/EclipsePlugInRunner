﻿using System.Windows;
using EclipsePlugInRunner.Scripting;
using VMS.TPS;

namespace EclipsePlugInRunner.TestRunner
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            ScriptRunner.Run(new Script());
        }
    }
}
