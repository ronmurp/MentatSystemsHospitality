using Msh.Common.Data;
using Msh.Common.Models.Captcha;

namespace Msh.Common.Services;

/// <summary>
/// A place to record various states that other records can be recorded in.
/// </summary>
public interface ICaptchaConfigRepoService : IBaseRepository<CaptchaConfig>
{
	
}