namespace Msh.Common.Models.Captcha;

/// <summary>
/// Config data for Google reCAPTCHA
/// </summary>
public class CaptchaConfig
{
	/// <summary>
	/// The label or name of the particular setting (local host, test, live)
	/// </summary>
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// The key used in the client javascript
	/// </summary>
	public string ClientKey { get; set; } = string.Empty;

	/// <summary>
	/// The secret key used in the server code, in CaptchaController
	/// </summary>
	public string SecretKey { get; set; } = string.Empty;

	/// <summary>
	/// The lowest score that will be accepted
	/// </summary>
	public double ScoreLimit { get; set; } = 0.5;

	/// <summary>
	/// The main script url that the service builds into a script tag, with the render parameter
	/// </summary>
	public string ScriptUrl { get; set; } = string.Empty;

	/// <summary>
	/// The server side call to verify the captcha and return a score
	/// </summary>
	public string VerifyUrl { get; set; } = string.Empty;

}