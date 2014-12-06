using System;
using System.Linq;

namespace Algo3
{ 
    internal class Program
    {
        private static void Main(string[] args)
        {
            var n = 5;

            var A = new[,]
            {
                {100, 99 , 1 , 1, 1},
                {1  , 100, 1 , 1, 1},
                {1  , 1  , 99, 1, 1},
                {1  , 100, 1 , 1, 1},
                {100, 99 , 1 , 1, 1},
            };


            var B = new int[n, n];

            for (var j = 0; j < n; j++)
            {
                B[0, j] = A[0, j];
            }

            // Code not in test, reseting the second row to prevent index out of range
            for (var j = 1; j < n - 1; j++)
            {
                B[1, j] = A[1, j] + new[] {B[0, j], B[0, j - 1], B[0, j + 1]}.Max();
            }
            
            B[1, 0] = A[1, 0] + new[] { B[0, 0], B[0, 1] }.Max();
            B[1, n - 1] = A[1, n - 1] + new[] { B[0, n - 1], B[0, n - 2] }.Max();
            // End of code not in test

            for (var i = 2; i < n; i++)
            {
                for (var j = 1; j < n - 1; j++)
                {
                    B[i, j] = A[i, j] + new[] {B[i - 1, j], A[i - 1, j - 1] + B[i - 2, j - 1], A[i - 1, j + 1] + B[i - 2, j + 1]}.Max();
                }

                B[i, 0] = A[i, 0] + new[] {B[i - 1, 0], A[i - 1, 1] + B[i - 2, 1]}.Max();
                B[i, n - 1] = A[i, n -1] + new[] {B[i - 1, n - 1], A[i - 1, n - 2] + B[i - 2, n - 2]}.Max();
            }

            var max = int.MinValue;

            // Fix error where ran from n to n
            for (int j = 0; j < n; j++)
            {
                if (B[n - 1,j] > max)
                {
                    max = B[n - 1, j];
                }
            }

            Console.WriteLine(max);
            Console.ReadKey();
        }
    }
}