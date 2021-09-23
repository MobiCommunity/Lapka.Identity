using System;
using System.Collections.Generic;

namespace Lapka.Identity.Application.Dto
{
    public class ShelterViewsDto
    {
        public Guid Id { get; set; }
        public int ActualMonthViewsCount { get; set; }
        public IEnumerable<ViewHistoryDto> PreviousMonthsViews { get; set; }
    }
}