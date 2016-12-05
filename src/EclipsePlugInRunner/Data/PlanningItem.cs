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

        public static bool operator ==(PlanningItem p1, PlanningItem p2)
        {
            // Handle case where p1 is null (p2 must be null to be equal)
            return ReferenceEquals(p1, null) ? ReferenceEquals(p2, null) : p1.Equals(p2);
        }

        public static bool operator !=(PlanningItem p1, PlanningItem p2)
        {
            return !(p1 == p2);
        }

        // Do not bother with calculating a hashcode,
        // but overriding it stops a compiler warning
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}