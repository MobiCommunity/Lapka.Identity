﻿using System;
using System.Threading.Tasks;
using Lapka.Identity.Core.ValueObjects;

namespace Lapka.Identity.Application.Services
{
    public interface IShelterOwnerApplicationRepository
    {
        Task<ShelterOwnerApplication> GetAsync(Guid id);
        Task<ShelterOwnerApplication> GetAsync(Guid userId, Guid shelterId);
        Task AddApplicationAsync(ShelterOwnerApplication application);
        Task UpdateAsync(ShelterOwnerApplication application);
    }
}