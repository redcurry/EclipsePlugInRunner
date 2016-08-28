namespace EclipsePlugInRunner.Data
{
    internal class PlanningItem
    {
        public string Id { get; set; }
        public string CourseId { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as PlanningItem;

            if (other != null)
            {
                return Id == other.Id && CourseId == other.CourseId;
            }

            return false;
        }
    }
}