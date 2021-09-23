using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class ShelterViewsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public int ActualMonthViewsCount { get; set; }
        public IEnumerable<ViewHistoryDocument> PreviousMonthsViews { get; set; }
    }
}