using System;



public static class ArrayExtension
{
    /// <summary>
    /// Swaps the two elements at the given indices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="indexA"></param>
    /// <param name="indexB"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Swap<T>(this T[] array, int indexA, int indexB)
    {
        if (indexA < 0 || indexA >= array.Length || indexB < 0 || indexB >= array.Length)
            throw new ArgumentOutOfRangeException("The index argument is outside the given arrays index range.");

        T temp = array[indexA];
        array[indexA] = array[indexB];
        array[indexB] = temp;
    }
}