using System;

namespace Church.Data
{
    public class dtoIntentions
    {
        public int TotalCount { get; set; }
        public Int64 ROWNUM { get; set; }
        public int IntentionID { get; set; }
        public string Name { get; set; }
        public string MentionPerson { get; set; }
        public string Hour { get; set; }
        public string IntentionType { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime IntentionDate { get; set; }
    }
}
