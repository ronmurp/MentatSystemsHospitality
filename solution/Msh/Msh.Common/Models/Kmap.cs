namespace Msh.Common.Models;

/// <summary>
/// Each KmMap maps in input to Output of type U, based on the multi-value Map list.
/// </summary>
/// <typeparam name="U">string or enum, typically. Could be an object</typeparam>
public class KmMap<U>
{
    /// <summary>
    /// The string of map values acts as a key into a dictionary. The key string
    /// is transposed into the Map list, which if the Map satisfies an input, the Output
    /// becomes the output used.
    /// </summary>
    /// <remarks>
    /// Something like ...
    /// "1,0,X,1,0"
    /// </remarks>
    public string Key { get; set; }

    /// <summary>
    /// Contains the values of the Key
    /// </summary>
    public List<string> Map { get; set; }

    /// <summary>
    /// The output emitted if the inputs satisfy this Map
    /// </summary>
    public U Output { get; set; }

}