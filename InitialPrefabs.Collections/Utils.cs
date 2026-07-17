using System;
using System.Runtime.CompilerServices;

namespace InitialPrefabs.Collections {

    /// <summary>
    /// Convenience class for MathUtilities.
    /// </summary>
    public static class MathUtils {
        /// <summary>
        /// Divides two integers and rounds it up to the nearest integer.
        /// </summary>
        /// <param name="numerator">The total to divide from.</param>
        /// <param name="denominator">The total to divide by.</param>
        /// <returns>The resulting division operation.</returns>
        /// <exception cref="DivideByZeroException">The <paramref name="denominator"/> cannot be 0 unless.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilToIntDivision(int numerator, int denominator) {
#if DEBUG
            if (denominator == 0) {
                throw new DivideByZeroException($"Cannot divide {numerator} by 0!");
            }
#endif
            return (numerator + denominator - 1) / denominator;
        }

        /// <summary>
        /// Returns the minimum between <paramref name="a"/> or <paramref name="b"/>.
        /// </summary>
        /// <param name="a">An integer.</param>
        /// <param name="b">An integer.</param>
        /// <returns>The minimum between <paramref name="a"/> or <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int a, int b) {
            return a < b ? a : b;
        }
    }
}