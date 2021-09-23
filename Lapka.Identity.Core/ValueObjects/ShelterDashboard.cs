using System;
using System.Collections.Generic;

namespace Lapka.Identity.Core.ValueObjects
{
    public class ShelterDashboard
    {
        public Guid Id { get; }
        public int ViewsCount { get; }

        public ShelterDashboard(Guid id, int viewsCount)
        {
            Id = id;
            ViewsCount = viewsCount;
        }
    }
}