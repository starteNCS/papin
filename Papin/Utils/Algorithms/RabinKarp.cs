namespace Papin.Utils.Algorithms;

/// <summary>
/// Implementation of Rabin Karp Algorithm
/// Is used to find a sequence of objects in a list
/// </summary>
public static class RabinKarp
{
    /// <summary>
    /// Uses the Rabin Karp Algorithm to search for a sequence in the provided list
    /// </summary>
    /// <param name="list">List to search the sequence in</param>
    /// <param name="sequence">Sequence to search for</param>
    /// <param name="index">The found index or -1</param>
    /// <returns>Index of the first element when found, otherwise -1</returns>
    public static bool FindSequence(this List<byte> list, byte[] sequence, out int index)
    {
        int sequenceHash = 0, currentWindowHash = 0;
        const int alphabetLength = 127;
        const int q = 101;
        int h = (int) Math.Pow(alphabetLength, sequence.Length - 1) % q;

        // calculate the first hashes
        for (int i = 0; i < sequence.Length; i++)
        {
            sequenceHash = (alphabetLength * sequenceHash + sequence[i]) % q;
            currentWindowHash = (alphabetLength * currentWindowHash + list[i]) % q;
        }
        
        for (int i = 0; i <= list.Count - sequence.Length; i++)
        {
            if (currentWindowHash == sequenceHash)
            {
                // check the values naively when the hashes match, to make sure they are exactly the same
                int j; 
                for (j = 0; j < sequence.Length; j++)
                {
                    if (list[j + i] != sequence[j])
                    {
                        break;
                    }
                }

                if (j == sequence.Length)
                {
                    index = i;
                    return true;
                }
            }
            
            // Roll the hash for the next window
            if (i < list.Count - sequence.Length)
            {
                currentWindowHash = (alphabetLength * (currentWindowHash - list[i] * h) + list[i + sequence.Length]) % q;
                if (currentWindowHash < 0)
                {
                    currentWindowHash += q;
                }
            }
        }

        index = -1;
        return false;
    }
}