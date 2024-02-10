using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    // Extension method to shuffle a list using the Fisher-Yates algorithm
    public static void Shuffle<T>(this List<T> list)
    {
        int n = list.Count;

        // Iterate through the list in reverse order
        while (n > 1)
        {
            n--;

            // Generate a random index within the remaining unshuffled elements
            int k = Random.Range(0, n + 1);

            // Swap the elements at indices k and n
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
