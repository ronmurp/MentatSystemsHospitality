namespace Msh.Common.Models.Configuration;

/// <summary>
/// Holds a loaded secret, and the source of the secret
/// </summary>
public class ConfigSecret
{
    /// <summary>
    /// Where is the secret stored?
    /// </summary>
    public ConfigSecretSource SecretSource { get; set; } = ConfigSecretSource.InConfig;

    /// <summary>
    /// For InEnvVar, what is the target? Ignored for other sources
    /// </summary>
    public EnvironmentVariableTarget Target { get; set; } = EnvironmentVariableTarget.Machine;

    /// <summary>
    /// The name of the location where the secret is held
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The secret itself
    /// </summary>
    public string Secret { get; set; } = string.Empty;

}