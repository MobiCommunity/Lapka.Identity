namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class ViewHistoryDocument
    {
        public int MonthOfTheYear { get; set; }
        public int Year { get; set; }
        public int Views { get; set; }
    }
}