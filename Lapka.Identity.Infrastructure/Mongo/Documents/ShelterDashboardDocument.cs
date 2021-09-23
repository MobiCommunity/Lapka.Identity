using System;
using System.Collections.Generic;
using Convey.Types;

namespace Lapka.Identity.Infrastructure.Mongo.Documents
{
    public class ShelterDashboardDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public int ActualMonthViewsCount { get; set; }
        public List<ViewHistoryDocument> PreviousMonthsViews { get; set; }
    }
}