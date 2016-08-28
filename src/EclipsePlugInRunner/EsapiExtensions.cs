using System.Collections.Generic;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner
{
    internal static class EsapiExtensions
    {
        public static IEnumerable<PlanningItem> GetPlanningItems(this Patient patient)
        {
            var planningItems = new List<PlanningItem>();

            if (patient.Courses != null)
            {
                foreach (var course in patient.Courses)
                {
                    if (course.PlanSetups != null)
                    {
                        planningItems.AddRange(course.PlanSetups);
                    }

                    if (course.PlanSums != null)
                    {
                        planningItems.AddRange(course.PlanSums);
                    }
                }
            }

            return planningItems;
        }

        public static Course GetCourse(this PlanningItem planningItem)
        {
            return planningItem is PlanSetup
                ? ((PlanSetup)planningItem).Course
                : ((PlanSum)planningItem).Course;
        }
    }
}
