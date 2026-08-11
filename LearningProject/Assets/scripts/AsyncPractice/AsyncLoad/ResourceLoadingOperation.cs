using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ResourceLoadingOperation : ILoadingOperation
{
    private readonly string _resourcePath;

    public event Action<Texture2D> OnResourceLoading;

    public ResourceLoadingOperation(string resourcePath)
    {
        _resourcePath = resourcePath;
    }

    public async Task LoadAsync(IProgress<float> progress = null, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        ResourceRequest request = Resources.LoadAsync<Texture2D>(_resourcePath);

        while (!request.isDone)
        {
            token.ThrowIfCancellationRequested();
            progress?.Report(request.progress);
            await Task.Yield();
        }

        Texture2D texture = request.asset as Texture2D;

        if (texture == null)
        {
            throw new Exception($"Resource {_resourcePath} don't found or don't Texture2D");
        }

        OnResourceLoading?.Invoke(texture);
        Debug.Log($"Ресурс '{_resourcePath}' успешно загружен");

        progress?.Report(1f);
    }
}
