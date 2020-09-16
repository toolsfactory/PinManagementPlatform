﻿using System;
using System.Threading.Tasks;

namespace PinPlatform.Common.Interfaces
{
    public interface IPinDataStore
    {
        Task<(uint FailedAttemptsCount, DateTime LastFailedAttempt)> GetFailedVerificationsInfoAsync(DataModels.RequestorInfo requestor, uint? pinType);
        Task UpdateFailedVerificationsInfoAsync(DataModels.RequestorInfo requestor, uint? pinType, uint failedAtempts, DateTime lastFailed);
        Task DeleteFailedAttemptsInfoAsync(DataModels.RequestorInfo requestor, uint? pinType);
        Task<byte[]?> GetPinHashAsync(DataModels.RequestorInfo requestor, uint? pinType);
        Task SetPinHashAsync(DataModels.RequestorInfo requestor, uint? pinType, byte[] hash);
    }
}