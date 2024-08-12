namespace Msh.Common.Exceptions;

/// <summary>
/// An exception thrown when an expected Config does not exist in the db
/// </summary>
/// <param name="message"></param>
public class NullConfigException(string message) : Exception(message);