namespace church.backend.Utilities
{
    public class NullValues
    {
        public int nullInt(string data) { try { return int.Parse(data); } catch { return 0; } }
        public bool nullBool(string data)
        {
            return data.ToLower() == "true" ? true : false;
        }
        public bool nullBoolNumber(string data)
        {
            return data.ToLower() == "1" ? true : false;
        }
        public DateTime nullDate(string data) { try { return DateTime.Parse(data); } catch { return new DateTime(); } }
        public double nullDouble(string data)
        {
            try { return double.Parse(data); }
            catch { return 0.0; }
        }
    }
}
