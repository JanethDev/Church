namespace Church.Data
{
    public enum Result
    {
        OK,
        ERROR,
        BAD_REQUEST,
        NOT_FOUND,
        INVALID_PASSWORD,
        EXCEPTION,
        BLOCKED_USER,
        ALREADY_ASSIGNED,
        NOT_ASSIGNED,
        DISABLED_AND_UNASSIGNED,
        NOT_REGISTERED,
        ALREADY_EXISTS
    }
    public class Response
    {
        public Result Result { get; set; }
        public string Data { get; set; }
        public string error { get; set; }
        public decimal Decimal { get; set; }
    }
}
