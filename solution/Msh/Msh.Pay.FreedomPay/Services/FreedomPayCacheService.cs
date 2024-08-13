﻿using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;
using Msh.Common.Services;
using Msh.Pay.FreedomPay.Models;
using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services;

/// <summary>
/// Memory cache of FreedomPay configuration
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public class FreedomPayCacheService(IMemoryCache memoryCache, IConfigRepository configRepository)
    : DataCacheService(memoryCache, configRepository), IFreedomPayCacheService
{

    /// <summary>
    /// Get the bank of common error messages
    /// </summary>
    /// <returns></returns>
    public async Task<List<FpErrorCodeBank>> GetFpErrorCodesBank() => await base.GetData<List<FpErrorCodeBank>>(ConstFp.FpErrorCodeBank);

    /// <summary>
    /// Get the list of error codes
    /// </summary>
    /// <returns></returns>
    public async Task<List<FpErrorCode>> GetFpErrorCodes()
    {
        var bankList = await GetFpErrorCodesBank();

        var errorCodes = await base.GetData<List<FpErrorCode>>(ConstFp.FpErrorCode);

        // Update error codes with any bank messages
        foreach (var errorCode in errorCodes)
        {
            if (!string.IsNullOrEmpty(errorCode.Use))
            {
                var bank = bankList.FirstOrDefault(b => b.Code == errorCode.Use);
                if (bank != null)
                {
                    errorCode.Message = bank.Message;
                }
            }
        }

        return errorCodes;
    }


    /// <summary>
    /// Clear the bank, so the cache is reloaded when next used
    /// </summary>
    public void ReloadBank() => base.Reload(ConstFp.FpErrorCodeBank);

    /// <summary>
    /// Clear the error codes cache, so the cache is reloaded when next used
    /// </summary>
    public void ReloadErrorCodes() => base.Reload(ConstFp.FpErrorCode);
}