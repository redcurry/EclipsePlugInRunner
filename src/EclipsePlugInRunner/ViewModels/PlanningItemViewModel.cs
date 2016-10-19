using System;
using GalaSoft.MvvmLight;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.ViewModels
{
    internal class PlanningItemViewModel : ViewModelBase
    {
        public PlanningItemViewModel(Course course, PlanningItem planningItem)
        {
            Course = course;
            PlanningItem = planningItem;
        }

        public Course Course { get; private set; }
        public PlanningItem PlanningItem { get; private set; }

        public string Id
        {
            get { return PlanningItem.Id; }
        }

        public string CourseId
        {
            get { return Course.Id; }
        }

        public DateTime? CreationDateTime
        {
            get { return PlanningItem.CreationDateTime; }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { Set(ref _isChecked, value); }
        }
    }
}