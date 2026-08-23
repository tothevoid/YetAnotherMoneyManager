using System;

namespace MoneyManager.Application.Attributes.Scheduler
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ScheduledJobAttribute : Attribute
    {
        public string TaskName { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public string Category { get; }

        public string DefaultCronExpression { get; }

        public ScheduledJobAttribute(
            string taskName,
            string displayName,
            string description,
            string category,
            string defaultCronExpression = "0 0 * * *")
        {
            TaskName = taskName;
            DisplayName = displayName;
            Description = description;
            Category = category;
            DefaultCronExpression = defaultCronExpression;
        }
    }
}
