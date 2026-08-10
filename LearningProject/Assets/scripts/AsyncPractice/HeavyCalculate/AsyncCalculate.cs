using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncCalculate : MonoBehaviour
{
    [SerializeField] private int _calculateNumber;

    private CancellationTokenSource _tokenSource;
    private int _cancellationTimeSeconds = 10;

    private async void Start()
    {
        await Task.Delay(5000);

        if (_tokenSource != null)
        {
            DisposeToken(ref _tokenSource);
        }

        _tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(_cancellationTimeSeconds));

        try
        {
            int result = await Task.Run(
                () => HeavyCalculate.ArithmeticProgression(_calculateNumber, _tokenSource.Token),
                _tokenSource.Token
                );

            Debug.Log($"Result: {result}");
        }

        catch (OperationCanceledException)
        {
            Debug.Log("Operation Cancelled");
        }
        catch (Exception ex)
        {
            Debug.Log($"Exeption {ex.Message}");
        }
        finally
        {
            DisposeToken(ref _tokenSource);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_tokenSource != null && !_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
            }
        }
    }

    private void OnDestroy()
    {
        DisposeToken(ref _tokenSource);
    }

    private void DisposeToken(ref CancellationTokenSource token)
    {
        if (token != null)
        {
            if (!token.IsCancellationRequested)
            {
                token.Cancel();
            }

            token.Dispose();
            token = null;
        }
    }
}
