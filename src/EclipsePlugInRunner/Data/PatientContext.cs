using System.Collections.Generic;

namespace EclipsePlugInRunner.Data
{
    internal class PatientContext
    {
        public PatientContext()
        {
            PlanningItemsInScope = new List<PlanningItem>();
        }

        public string PatientId { get; set; }
        public string PlanSetupId { get; set; }
        public List<PlanningItem> PlanningItemsInScope { get; set; }
    }
}
