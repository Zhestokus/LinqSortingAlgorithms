///*************************************************************************
// *  Compilation:  javac StdRandom.java
// *  Execution:    java StdRandom
// *  Dependencies: StdOut.java
// *
// *  A library of static methods to generate pseudo-random numbers from
// *  different distributions (bernoulli, uniform, gaussian, discrete,
// *  and exponential). Also includes a method for shuffling an array.
// *
// *
// *  %  java StdRandom 5
// *  seed = 1316600602069
// *  59 16.81826  true 8.83954  0 
// *  32 91.32098  true 9.11026  0 
// *  35 10.11874  true 8.95396  3 
// *  92 32.88401  true 8.87089  0 
// *  72 92.55791  true 9.46241  0 
// *
// *  % java StdRandom 5
// *  seed = 1316600616575
// *  96 60.17070  true 8.72821  0 
// *  79 32.01607  true 8.58159  0 
// *  81 59.49065  true 9.10423  1 
// *  96 51.65818  true 9.02102  0 
// *  99 17.55771  true 8.99762  0 
// *
// *  % java StdRandom 5 1316600616575
// *  seed = 1316600616575
// *  96 60.17070  true 8.72821  0 
// *  79 32.01607  true 8.58159  0 
// *  81 59.49065  true 9.10423  1 
// *  96 51.65818  true 9.02102  0 
// *  99 17.55771  true 8.99762  0 
// *
// *
// *  Remark
// *  ------
// *    - Relies on randomness of nextDouble() method in java.util.Random
// *      to generate pseudorandom numbers in [0, 1).
// *
// *    - This library allows you to set and get the pseudorandom number seed.
// *
// *    - See http://www.honeylocust.com/RngPack/ for an industrial
// *      strength random number generator in Java.
// *
// *************************************************************************/


///**
// *  <i>Standard random</i>. This class provides methods for generating
// *  random number from various distributions.
// *  <p>
// *  For additional documentation, see <a href="http://introcs.cs.princeton.edu/22library">Section 2.2</a> of
// *  <i>Introduction to Programming in Java: An Interdisciplinary Approach</i> by Robert Sedgewick and Kevin Wayne.
// *
// *  @author Robert Sedgewick
// *  @author Kevin Wayne
// */

//using System;

//namespace SortingConsoleApp
//{
//    public static class StdRandom
//    {
//        private static int _seed;        // pseudo-random number generator seed
//        private static Random _random;    // pseudo-random number generator

//        // static initializer
//        static StdRandom()
//        {
//            // this is how the seed was set in Java 1.4
//            _seed = (int)TimeSpan.FromTicks(DateTime.Now.Ticks).TotalMilliseconds;
//            _random = new Random(_seed);
//        }

//        /**
//         * Sets the seed of the psedurandom number generator.
//         */
//        public static void SetSeed(int seed)
//        {
//            _seed = seed;
//            _random = new Random(seed);
//        }

//        /**
//         * Returns the seed of the psedurandom number generator.
//         */
//        public static long getSeed()
//        {
//            return _seed;
//        }

//        /**
//         * Return real number uniformly in [0, 1).
//         */
//        public static double Uniform()
//        {
//            return _random.NextDouble();
//        }

//        /**
//         * Returns an integer uniformly between 0 (inclusive) and N (exclusive).
//         * @throws IllegalArgumentException if <tt>N <= 0</tt>
//         */
//        public static int Uniform(int N)
//        {
//            if (N <= 0)
//                throw new ArgumentException("Parameter N must be positive");

//            return _random.Next(N);
//        }

//        ///////////////////////////////////////////////////////////////////////////
//        //  STATIC METHODS BELOW RELY ON JAVA.UTIL.RANDOM ONLY INDIRECTLY VIA
//        //  THE STATIC METHODS ABOVE.
//        ///////////////////////////////////////////////////////////////////////////

//        /**
//         * Returns a real number uniformly in [0, 1).
//         * @deprecated clearer to use {@link #uniform()}
//         */
//        public static double Random()
//        {
//            return Uniform();
//        }

//        /**
//         * Returns an integer uniformly in [a, b).
//         * @throws IllegalArgumentException if <tt>b <= a</tt>
//         * @throws IllegalArgumentException if <tt>b - a >= Integer.MAX_VALUE</tt>
//         */
//        public static int Uniform(int a, int b)
//        {
//            if (b <= a)
//                throw new ArgumentException("Invalid range");

//            if ((long)b - a >= int.MaxValue)
//                throw new ArgumentException("Invalid range");

//            return a + Uniform(b - a);
//        }

//        /**
//         * Returns a real number uniformly in [a, b).
//         * @throws IllegalArgumentException unless <tt>a < b</tt>
//         */
//        public static double Uniform(double a, double b)
//        {
//            if (!(a < b))
//                throw new ArgumentException("Invalid range");

//            return a + Uniform() * (b - a);
//        }

//        /**
//         * Returns a boolean, which is true with probability p, and false otherwise.
//         * @throws IllegalArgumentException unless <tt>p >= 0.0</tt> and <tt>p <= 1.0</tt>
//         */
//        public static bool Bernoulli(double p)
//        {
//            if (!(p >= 0.0 && p <= 1.0))
//                throw new ArgumentException("Probability must be between 0.0 and 1.0");

//            return Uniform() < p;
//        }

//        /**
//         * Returns a boolean, which is true with probability .5, and false otherwise.
//         */
//        public static bool Bernoulli()
//        {
//            return Bernoulli(0.5);
//        }

//        /**
//         * Returns a real number with a standard Gaussian distribution.
//         */
//        public static double Gaussian()
//        {
//            // use the polar form of the Box-Muller transform
//            double r, x, y;

//            do
//            {
//                x = Uniform(-1.0, 1.0);
//                y = Uniform(-1.0, 1.0);
//                r = x * x + y * y;
//            } while (r >= 1 || r == 0);

//            return x * Math.Sqrt(-2 * Math.Log(r) / r);

//            // Remark:  y * Math.sqrt(-2 * Math.log(r) / r)
//            // is an independent random gaussian
//        }

//        /**
//         * Returns a real number from a gaussian distribution with given mean and stddev
//         */
//        public static double Gaussian(double mean, double stddev)
//        {
//            return mean + stddev * Gaussian();
//        }

//        /**
//         * Returns an integer with a geometric distribution with mean 1/p.
//         * @throws IllegalArgumentException unless <tt>p >= 0.0</tt> and <tt>p <= 1.0</tt>
//         */
//        public static int Geometric(double p)
//        {
//            if (!(p >= 0.0 && p <= 1.0))
//                throw new ArgumentException("Probability must be between 0.0 and 1.0");

//            // using algorithm given by Knuth
//            return (int)Math.Ceiling(Math.Log(Uniform()) / Math.Log(1.0 - p));
//        }

//        /**
//         * Return an integer with a Poisson distribution with mean lambda.
//         * @throws IllegalArgumentException unless <tt>lambda > 0.0</tt> and not infinite
//         */
//        public static int Poisson(double lambda)
//        {
//            if (!(lambda > 0.0))
//                throw new ArgumentException("Parameter lambda must be positive");

//            if (double.IsInfinity(lambda))
//                throw new ArgumentException("Parameter lambda must not be infinite");
//            // using algorithm given by Knuth
//            // see http://en.wikipedia.org/wiki/Poisson_distribution

//            int k = 0;
//            double p = 1.0;
//            double L = Math.Exp(-lambda);
//            do
//            {
//                k++;
//                p *= Uniform();
//            } while (p >= L);

//            return k - 1;
//        }

//        /**
//         * Returns a real number with a Pareto distribution with parameter alpha.
//         * @throws IllegalArgumentException unless <tt>alpha > 0.0</tt>
//         */
//        public static double Pareto(double alpha)
//        {
//            if (!(alpha > 0.0))
//                throw new ArgumentException("Shape parameter alpha must be positive");

//            return Math.Pow(1 - Uniform(), -1.0 / alpha) - 1.0;
//        }

//        /**
//         * Returns a real number with a Cauchy distribution.
//         */
//        public static double Cauchy()
//        {
//            return Math.Tan(Math.PI * (Uniform() - 0.5));
//        }

//        /**
//         * Returns a number from a discrete distribution: i with probability a[i].
//         * throws IllegalArgumentException if sum of array entries is not (very nearly) equal to <tt>1.0</tt>
//         * throws IllegalArgumentException unless <tt>a[i] >= 0.0</tt> for each index <tt>i</tt>
//         */
//        public static int Discrete(double[] array)
//        {
//            const double EPSILON = 1E-14;
//            var sum = 0.0;

//            for (int i = 0; i < array.Length; i++)
//            {
//                if (!(array[i] >= 0.0))
//                    throw new ArgumentException("array entry " + i + " must be nonnegative: " + array[i]);

//                sum = sum + array[i];
//            }

//            if (sum > 1.0 + EPSILON || sum < 1.0 - EPSILON)
//                throw new ArgumentException("sum of array entries does not approximately equal 1.0: " + sum);

//            // the for loop may not return a value when both r is (nearly) 1.0 and when the
//            // cumulative sum is less than 1.0 (as a result of floating-point roundoff error)
//            while (true)
//            {
//                var r = Uniform();
//                sum = 0.0;
//                for (int i = 0; i < array.Length; i++)
//                {
//                    sum = sum + array[i];
//                    if (sum > r) return i;
//                }
//            }
//        }

//        /**
//         * Returns a real number from an exponential distribution with rate lambda.
//         * @throws IllegalArgumentException unless <tt>lambda > 0.0</tt>
//         */
//        public static double Exp(double lambda)
//        {
//            if (!(lambda > 0.0))
//                throw new ArgumentException("Rate lambda must be positive");

//            return -Math.Log(1 - Uniform()) / lambda;
//        }

//        /**
//         * Rearrange the elements of an array in random order.
//         */
//        public static void Shuffle(Object[] array)
//        {
//            int N = array.Length;
//            for (int i = 0; i < N; i++)
//            {
//                int r = i + Uniform(N - i);     // between i and N-1
//                Object temp = array[i];
//                array[i] = array[r];
//                array[r] = temp;
//            }
//        }

//        /**
//         * Rearrange the elements of a double array in random order.
//         */
//        public static void Shuffle(double[] array)
//        {
//            int N = array.Length;
//            for (int i = 0; i < N; i++)
//            {
//                int r = i + Uniform(N - i);     // between i and N-1
//                double temp = array[i];
//                array[i] = array[r];
//                array[r] = temp;
//            }
//        }

//        /**
//         * Rearrange the elements of an int array in random order.
//         */
//        public static void Shuffle(int[] array)
//        {
//            int N = array.Length;

//            for (int i = 0; i < N; i++)
//            {
//                int r = i + Uniform(N - i);     // between i and N-1
//                int temp = array[i];
//                array[i] = array[r];
//                array[r] = temp;
//            }
//        }


//        /**
//         * Rearrange the elements of the subarray a[lo..hi] in random order.
//         */
//        public static void Shuffle(Object[] array, int low, int high)
//        {
//            if (low < 0 || low > high || high >= array.Length)
//            {
//                throw new IndexOutOfRangeException("Illegal subarray range");
//            }
//            for (int i = low; i <= high; i++)
//            {
//                int r = i + Uniform(high - i + 1);     // between i and hi
//                Object temp = array[i];
//                array[i] = array[r];
//                array[r] = temp;
//            }
//        }

//        public static void Shuffle<TElement>(TElement[] array, int low, int high)
//        {
//            if (low < 0 || low > high || high >= array.Length)
//                throw new IndexOutOfRangeException("Illegal subarray range");

//            for (int i = low; i <= high; i++)
//            {
//                int r = i + Uniform(high - i + 1);     // between i and hi
//                var temp = array[i];
//                array[i] = array[r];
//                array[r] = temp;
//            }
//        }


//        /**
//         * Rearrange the elements of the subarray a[lo..hi] in random order.
//         */
//        public static void Shuffle(double[] array, int low, int high)
//        {
//            if (low < 0 || low > high || high >= array.Length)
//            {
//                throw new IndexOutOfRangeException("Illegal subarray range");
//            }

//            for (int i = low; i <= high; i++)
//            {
//                int r = i + Uniform(high - i + 1);     // between i and hi
//                double temp = array[i];
//                array[i] = array[r];
//                array[r] = temp;
//            }
//        }

//        /**
//         * Rearrange the elements of the subarray a[lo..hi] in random order.
//         */
//        public static void Shuffle(int[] array, int low, int high)
//        {
//            if (low < 0 || low > high || high >= array.Length)
//            {
//                throw new IndexOutOfRangeException("Illegal subarray range");
//            }
//            for (int i = low; i <= high; i++)
//            {
//                int r = i + Uniform(high - i + 1);     // between i and hi
//                int temp = array[i];
//                array[i] = array[r];
//                array[r] = temp;
//            }
//        }
//    }
//}