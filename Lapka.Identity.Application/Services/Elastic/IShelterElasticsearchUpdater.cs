using System;
using System.Threading.Tasks;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services.Elastic
{
    public interface IShelterElasticsearchUpdater
    {
        Task InsertAndUpdateDataAsync(Shelter shelter);
        Task DeleteDataAsync(Guid shelterId);
    }
}