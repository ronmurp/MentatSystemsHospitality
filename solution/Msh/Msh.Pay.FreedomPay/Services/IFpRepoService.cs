﻿using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services;

public interface IFpRepoService
{
	Task<FpConfig> GetFpConfig();
	Task SaveFpConfig(FpConfig config);

	//FpErrorCodeBank
	Task<List<FpErrorCodeBank>> GetFpErrorCodeBank();
	Task SaveFpErrorCodeBank(List<FpErrorCodeBank> list);
}