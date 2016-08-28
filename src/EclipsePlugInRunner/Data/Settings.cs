using System.Collections.Generic;

namespace EclipsePlugInRunner.Data
{
    internal class Settings
    {
        public Settings()
        {
            RecentPatientContexts = new List<PatientContext>();
        }

        public bool ShouldExitAfterScriptEnds { get; set; }
        public List<PatientContext> RecentPatientContexts { get; set; }
    }
}