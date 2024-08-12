namespace Msh.Common.Models.Configuration;

/// <summary>
/// The type of secret determines where the secret is stored.
/// </summary>
public enum ConfigSecretSource
{
    InConfig, // The secret is stored in the config itself. OK for test secrets.
    InEnvVar, // In an environment variable,
    InAzureSecret
}