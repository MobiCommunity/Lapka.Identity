using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lapka.Identity.Core.ValueObjects
{
    public class ShelterViews
    {
        private ICollection<ViewHistory> _viewHistories = new Collection<ViewHistory>();
        public Guid Id { get; }
        public int ViewsCount { get; private set; }

        public IEnumerable<ViewHistory> PreviousMonthsViews
        {
            get => _viewHistories;
            private set => _viewHistories = new List<ViewHistory>(value);
        }
        
        public ShelterViews(Guid id, int viewsCount, IEnumerable<ViewHistory> history)
        {
            Id = id;
            ViewsCount = viewsCount;
            PreviousMonthsViews = history;
        }

        public void IncreaseViewsCount()
        {
            ViewsCount++;
        }
    }
}