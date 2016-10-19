using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.TestScript
{
    public class MainViewModel
    {
        private readonly PlanSetup _planSetup;

        public MainViewModel(PlanSetup planSetup)
        {
            _planSetup = planSetup;
        }

        public string PlanSetupId
        {
            get { return _planSetup != null ? _planSetup.Id : "(No active plan.)"; }
        }
    }
}
