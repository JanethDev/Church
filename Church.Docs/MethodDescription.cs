using System;

namespace Church.Docs
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MethodDescription : Attribute
    {
        public string Description { get; set; }

        public MethodDescription(string FormatString, params string[] Argument)
        {
            Description = Argument != null ? string.Format(FormatString, Argument) : FormatString;
        }
    }
}
