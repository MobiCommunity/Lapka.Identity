namespace Lapka.Identity.Infrastructure.Elastic.Options
{
    public class ElasticSearchOptions
    {
        public string Url { get; set; }
        public ElasticAliases Aliases { get; set; }
    }
}