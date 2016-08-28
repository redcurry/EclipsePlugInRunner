using System;
using EclipsePlugInRunner.Helpers;
using GalaSoft.MvvmLight;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.ViewModels
{
    internal class PlanningItemViewModel : ViewModelBase
    {
        public PlanningItemViewModel(PlanningItem planningItem)
        {
            PlanningItem = planningItem;
        }

        public PlanningItem PlanningItem { get; private set; }

        public string Id
        {
            get { return PlanningItem.Id; }
        }

        public string CourseId
        {
            get { return PlanningItem.GetCourse().Id; }
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