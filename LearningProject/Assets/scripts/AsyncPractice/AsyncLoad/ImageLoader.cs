using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class ImageLoader : ILoadingOperation
{
    private readonly string _imagePath;
    private readonly Image _targetImage;

    public ImageLoader(string imagePath, Image targetImage)
    {
        _imagePath = imagePath;
        _targetImage = targetImage;
    }

    public string ImageURL { get; }
    public UnityEngine.UI.Image TargetImage { get; }

    public async Task LoadAsync(IProgress<float> progress = null, CancellationToken token = default)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(_imagePath))
        {
           UnityWebRequestAsyncOperation operation =  request.SendWebRequest();

            while (!operation.isDone)
            {
                token.ThrowIfCancellationRequested();
                progress?.Report(request.downloadProgress);
                await Task.Yield();
            }

            if (request.result != UnityWebRequest.Result.Success) 
            {
                throw new Exception($"Image load error: {request.error}");
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            if (_targetImage != null)
            {
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                _targetImage.sprite = sprite;
            }

            progress?.Report(1f);
        }

    }
}
