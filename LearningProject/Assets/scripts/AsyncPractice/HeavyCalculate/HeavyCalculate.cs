using System.Threading;
using UnityEngine;

public static class HeavyCalculate
{
    public static int ArithmeticProgression(int n, CancellationToken token)
    {
        long sum = 0;

        for (int i = 0; i <= n; i++)
        {
            token.ThrowIfCancellationRequested();
            sum  += i;
        }

        return (int)sum;
    }
}
