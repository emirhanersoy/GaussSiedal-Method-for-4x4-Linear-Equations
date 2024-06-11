using System;

    public static class GaussSeidelIteration
    {
        // EMIRHAN ERSOY
        // 22118080001
        public static (double[], int, double) Solve(double[,] A, double[] b, double tolerance, int maxIterations)
        {
            // Check matrix and vector dimensions
            if (A.GetLength(0) != A.GetLength(1) || A.GetLength(0) != b.Length)
            {
                throw new ArgumentException("Matrix and vector dimensions must be compatible.");
            }

            int n = A.GetLength(0);

            // Extract diagonal elements
            double[] diagonal = new double[n];
            for (int i = 0; i < n; i++)
            {
                diagonal[i] = A[i, i];
            }

            // Convert b to a column vector
            double[] bVector = b;

            // Calculate inverse of diagonal elements (used later)
            double[] invDiagonal = new double[n];
            for (int i = 0; i < n; i++)
            {
                if (diagonal[i] == 0) // Check for zero on diagonal (may cause issues)
                {
                    throw new ArgumentException("Matrix is not diagonally dominant (zero diagonal element).");
                }
                invDiagonal[i] = 1.0 / diagonal[i];
            }

            // Initialize solution vector with zeros
            double[] x = new double[n];
            double[] xOld = new double[n];

            double error = double.PositiveInfinity;
            int iterations = 0;

            while (error > tolerance && iterations < maxIterations)
            {
                iterations++;

                Array.Copy(x, xOld, n);

                // Update solution vector using newly calculated values
                for (int i = 0; i < n; i++)
                {
                    double sum = 0.0;
                    for (int j = 0; j < i; j++) // Use already updated values for x
                    {
                        sum += A[i, j] * x[j];
                    }
                    for (int j = i + 1; j < n; j++) // Use original values for x
                    {
                        sum += A[i, j] * xOld[j];
                    }
                    x[i] = invDiagonal[i] * (bVector[i] - sum);
                }

                // Calculate error
                error = 0.0;
                for (int i = 0; i < n; i++)
                {
                    error = Math.Max(error, Math.Abs(x[i] - xOld[i]));
                }
            }

            if (error > tolerance)
            {
                Console.WriteLine("Warning: Convergence tolerance was not met at termination.");
            }

            return (x, iterations, error);
        }

        public static void Main(string[] args)
        {
            // Define the matrix A
            double[,] A = new double[,] {
            { 2, 0, 1, 1},
            { 1, 3, 1, -1 },
            { 0, -1, 4, 1 },
            {0, 1, 1, -5 }
        };

            // Define the result vector b
            double[] b = { 1, 3, 6, 16 };

            // Set tolerance and maximum iterations
            double tolerance = 1e-6;
            int maxIterations = 100; // 18 iteration

            // Solve the system using Jacobi iteration
            var (solution, iterations, error) = Solve(A, b, tolerance, maxIterations);

            // Print the solution
            Console.WriteLine("Solution:");
            for (int i = 0; i < solution.Length; i++)
            {
                Console.WriteLine($"x[{i + 1}] = {solution[i]}");
            }

            Console.WriteLine($"\nIterations: {iterations}");
            Console.WriteLine($"Error: {error}");
            Console.ReadKey();
        }
    }
