namespace Cars.Data.PostgreSQL.Data
{
    public class Car
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string VIN { get; set; }
        public string creationReason { get; set; }


    }
}