using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Image _targetImage;

    [Header("Settings")]
    [SerializeField] private string _imageURL;
    [SerializeField] private string _resourcePath;
    [SerializeField] private string _sceneName;

    private async void Start()
    {
        try
        {
            await RunLoadingSequence();
            Debug.Log("Done");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Loading has been canceled");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Loading Error: {ex.Message}");
        }
    }

    private async Task RunLoadingSequence()
    {
        ImageLoader imageLoader = new ImageLoader(_imageURL, _targetImage);
        ResourceLoadingOperation resourceLoader = new ResourceLoadingOperation(_resourcePath);
        SceneLoader sceneLoader = new SceneLoader(_sceneName);

        Texture2D loadedTexture = null;
        resourceLoader.OnResourceLoading += (texture) =>
        {
            loadedTexture = texture;
        };

        ILoadingOperation[] operations = {imageLoader, resourceLoader, sceneLoader};
        float weightPerOperation = 1f/operations.Length;

        IProgress<float> overallProgress = new Progress<float>(p =>
        {
            _progressBar.value = p;
        });

        using (CancellationTokenSource token = new CancellationTokenSource()) 
        {
            token.CancelAfter(TimeSpan.FromSeconds(30));

            float accumulateProgress = 0f;

            foreach (ILoadingOperation  operation in operations)
            {
                float localProgress = 0f;

                IProgress<float> innerProgress = new Progress<float>(p =>
                {
                    localProgress = p;
                    float totalProgress = accumulateProgress + localProgress * weightPerOperation;
                    overallProgress.Report((totalProgress));
                });

                await operation.LoadAsync(innerProgress, token.Token);

                accumulateProgress += weightPerOperation;
                overallProgress.Report(accumulateProgress);
            }

            await Task.Delay(5000);
        }

    }
}
