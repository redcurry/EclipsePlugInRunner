using System;
using System.Collections.Generic;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.Helpers
{
    internal static class EsapiExtensions
    {
        public static IEnumerable<Tuple<Course, PlanningItem>> GetPlanningItems(this Patient patient)
        {
            var planningItems = new List<Tuple<Course, PlanningItem>>();

            if (patient.Courses != null)
            {
                foreach (var course in patient.Courses)
                {
                    if (course.PlanSetups != null)
                    {
                        planningItems.AddRange(course.PlanSetups
                            .Select(p => new Tuple<Course, PlanningItem>(course, p)));
                    }

                    if (course.PlanSums != null)
                    {
                        planningItems.AddRange(course.PlanSums
                            .Select(p => new Tuple<Course, PlanningItem>(course, p)));
                    }
                }
            }

            return planningItems;
        }
    }
}
