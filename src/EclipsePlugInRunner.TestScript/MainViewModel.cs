using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.TestScript
{
    public class MainViewModel
    {
        public MainViewModel(PlanSetup planSetup)
        {
            PlanSetup = planSetup;
        }

        public PlanSetup PlanSetup { get; set; }
    }
}
