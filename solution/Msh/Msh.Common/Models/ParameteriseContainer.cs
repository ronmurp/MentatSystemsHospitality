namespace Msh.Common.Models;

public abstract class ParameteriseContainer<T>
{

    /// <summary>
    /// Replaces parameter in a message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>The message with parameter key replaced by value </returns>
    public string Parameterise(string message, string key, string value)
        => message.Contains(key) ? message.Replace(key, value) : message;

    /// <summary>
    /// Replaces parameters in a message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="keyValues"></param>
    /// <returns>The message with parameter keys replaced by values </returns>
    public string Parameterise(string message, KeyValuePair<string, string>[] keyValues)
        => keyValues.Aggregate(message, (current, kv) => current.Replace(kv.Key, kv.Value));
}