namespace Church.Business
{
    public static class QueryHelper
    {      
        public class Column
        {
            public string Table { get; set; }
            public string Name { get; set; }
            public string As { get; set; }
            public Column() { }
            public Column(string _Table, string _Name, string _As = "")
            {
                Table = _Table;
                Name = _Name;
                As = _As;
            }
        }

        public static class JoinType
        {
            public static string JOIN { get { return "JOIN"; } }
            public static string LEFT_JOIN { get { return "LEFT JOIN"; } }
            public static string RIGHT_JOIN { get { return "RIGHT JOIN"; } }
            public static string FULL_OUTER_JOIN { get { return "FULL OUTER JOIN"; } }
        }

        public class Condition
        {
            public string Table { get; set; }
            public string Column { get; set; }
            public object Value { get; set; }

            public Condition(string _Table, string _Column, object _Value)
            {
                Table = _Table;
                Column = _Column;
                Value = _Value;
            }
            public Condition() { }
        }
    }
}
