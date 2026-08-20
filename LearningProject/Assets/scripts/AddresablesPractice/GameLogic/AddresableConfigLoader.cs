using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class AddresableConfigLoader<TConfig> : MonoBehaviour where TConfig : ScriptableObject
{
    [SerializeField] protected AssetReferenceT<TConfig> _configRef;

    protected TConfig Config { get; private set; }

    protected virtual async void Start()
    {
        if (_configRef == null)
        {
            Debug.LogError($"AssetReference for {typeof(TConfig).Name} is not assigned on {gameObject.name}");
            return;
        }

        AsyncOperationHandle<TConfig> handle = _configRef.LoadAssetAsync();
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Config = handle.Result;
            ApplyConfig();
        }
        else
        {
            Debug.LogError($"Failed to load config of type {typeof(TConfig).Name} on {gameObject.name}");
        }
    }

    protected abstract void ApplyConfig();

    protected virtual void OnDestroy()
    {
        if (_configRef != null && _configRef.IsValid())
        {
            Addressables.Release(_configRef);
        }
    }
}
