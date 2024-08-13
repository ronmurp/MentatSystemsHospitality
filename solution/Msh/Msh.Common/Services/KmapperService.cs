using Msh.Common.Models;

namespace Msh.Common.Services;

/// <summary>
/// A service that maps inputs of type T to outputs of type U
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public class KmMapperService<T, U>(List<T> inputKeys, List<KmMap<U>> mapList, U defaultOutput)
    where T : IComparable<T>
{
    public List<T> InputKeys { get; } = inputKeys;
    public int InputSize { get; } = inputKeys.Count;

    public U DefaultOutput { get; } = defaultOutput;
    public List<KmMap<U>> MapList { get; } = mapList;

    /// <summary>
    /// Takes the input set of type T
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public U GetOutput(List<T> inputs)
    {
        if (inputs.Count == 0)
        {
            return DefaultOutput;
        }

        var inputMap = new bool[InputSize];

        // Create a boolean map that sets a flag true if any corresponding input is present,
        // but false otherwise. 
        for (var i = 0; i < InputSize; i++)
        {
            inputMap[i] = inputs.Any(x => x.Equals(InputKeys[i]));
        }

        // If there's no match, this is the output
        U finalOutput = DefaultOutput;

        // A conceptual Map matrix exists, organised as columns and rows

        // Check each column of the Map matrix
        foreach (var m in MapList)
        {
            var mk = m.Map;
            var ok = true; // Assume we're going to find a Map that matches the input

            // Check each map in turn - i.e. each row of the Map column from a conceptual map matrix
            for (var row = 0; row < inputMap.Length; row++)
            {
                if (mk[row] == "X") continue; // Ignore case - That's a match whatever the input for this row.

                if (!inputMap[row] && mk[row] == "0") continue; // Input matches Map for this minterm
                if (inputMap[row] && mk[row] == "1") continue;  // Input matches Map for this minterm

                // These are where the row match fails, so it breaks out of this column onto the next column
                if (inputMap[row] && mk[row] == "0")
                {
                    ok = false;
                    break;
                }
                if (!inputMap[row] && mk[row] == "1")
                {
                    ok = false;
                    break;
                }
            }

            if (ok)
            {
                // If we come out of a column check with ok true,
                // the column is a match. Therefore, the output is this column map's Output.
                finalOutput = m.Output;

                // And we're done, so break out of further column tests
                break;
            }
        }

        // This the output for the given input. It might be the DefaultOutput if no columns matched the input.
        return finalOutput;
    }

}