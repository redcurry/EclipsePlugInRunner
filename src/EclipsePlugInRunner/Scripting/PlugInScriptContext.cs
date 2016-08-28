using System.Collections.Generic;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.Scripting
{
    internal class PlugInScriptContext
    {
        public PlugInScriptContext(
            User user,
            Patient patient,
            Image image,
            StructureSet structureSet,
            PlanSetup planSetup,
            IEnumerable<PlanSetup> planSetupsInScope,
            IEnumerable<PlanSum> planSumsInScope)
        {
            User = user;
            Patient = patient;
            Image = image;
            StructureSet = structureSet;
            PlanSetup = planSetup;
            PlanSetupsInScope = planSetupsInScope;
            PlanSumsInScope = planSumsInScope;
        }

        public User User { get; set; }
        public Patient Patient { get; set; }
        public Image Image { get; set; }
        public StructureSet StructureSet { get; set; }
        public PlanSetup PlanSetup { get; set; }
        public IEnumerable<PlanSetup> PlanSetupsInScope { get; set; }
        public IEnumerable<PlanSum> PlanSumsInScope { get; set; }
    }
}