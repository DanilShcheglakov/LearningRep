using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ILoadingOperation
{
    private readonly string _sceneName;

    public SceneLoader(string sceneName )
    {
        _sceneName = sceneName;
    }

    public async Task LoadAsync(IProgress<float> progress = null, CancellationToken token = default)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneName);

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f) 
        {
            token.ThrowIfCancellationRequested();
            progress?.Report(operation.progress);
            await Task.Yield();
        }

        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            token.ThrowIfCancellationRequested();
            progress?.Report(Mathf.Lerp(0.9f,1f,operation.progress));
            await Task.Yield();
        }

        progress?.Report(1f);
    }
}
