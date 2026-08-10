using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MoveAnim : MonoBehaviour
{
    private CancellationTokenSource _currentCts;
    private float _lastClickTime;
    private int _clickCount;

    private async void Start()
    {
        await StartDemoAnimation();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float now = Time.time;

            if (now - _lastClickTime < 0.5f)
            {
                _clickCount++;

                if (_clickCount >= 3)
                {
                    if (_currentCts != null && !_currentCts.IsCancellationRequested)
                    {
                        Debug.Log("Обнаружен тройной тап! Отменяем анимацию.");
                        _currentCts.Cancel();
                    }
                    _clickCount = 0;
                }
            }
            else
            {
                _clickCount = 1;
            }

            _lastClickTime = now;
        }
    }

    public async Task StartDemoAnimation()
    {
        try
        {
            await PlayMoveToAsync(new Vector3(5f, 0f, 5f), 2f);
            Debug.Log("После завершения анимации: делаем что-то ещё (например, включаем эффект).");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Анимация была отменена — выполняем логику после отмены.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Неожиданная ошибка: {ex.Message}");
        }
    }

    public async Task PlayMoveToAsync(Vector3 target, float duration, CancellationToken externalToken = default)
    {
        if (_currentCts != null)
        {
            _currentCts.Cancel();
            _currentCts.Dispose();
            _currentCts = null;
        }

        using (CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken))
        {
            _currentCts = linkedCts;
            CancellationToken token = linkedCts.Token;

            Vector3 startPos = transform.position;
            float elapsed = 0f;

            try
            {
                while (elapsed < duration)
                {
                    token.ThrowIfCancellationRequested();

                    float t = elapsed / duration;
                    transform.position = Vector3.Lerp(startPos, target, t);

                    await Task.Delay(1, token);

                    elapsed += Time.deltaTime;
                }

                transform.position = target;
                Debug.Log("Анимация успешно завершена.");
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                transform.position = target;
                Debug.Log("Анимация прервана отменой, но объект перемещён в конечную точку.");
                throw;
            }
            finally
            {
                if (_currentCts == linkedCts)
                {
                    _currentCts = null;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (_currentCts != null)
        {
            _currentCts.Cancel();
            _currentCts.Dispose();
            _currentCts = null;
        }
    }
}
