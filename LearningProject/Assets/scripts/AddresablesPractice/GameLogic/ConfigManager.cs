using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    [SerializeField] private string _configLabel = "configs";

    private Dictionary<System.Type, ScriptableObject> _configs = new Dictionary<System.Type, ScriptableObject>();
    private bool _isReady = false;

    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        await LoadConfigs();
    }

    private async Task LoadConfigs()
    {
        AsyncOperationHandle<IList<ScriptableObject>> handle = Addressables.LoadAssetsAsync<ScriptableObject>(_configLabel, null);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (ScriptableObject config in handle.Result)
            {
                System.Type type = config.GetType();
                if (!_configs.ContainsKey(type))
                    _configs.Add(type, config);
                else
                    Debug.LogWarning($"Duplicate config of type {type.Name} found. Using first.");
            }
            _isReady = true;
            Debug.Log("All configs loaded successfully.");
        }
        else
        {
            Debug.LogError("Failed to load configs from Addressables.");
        }
    }

    public T GetConfig<T>() where T : ScriptableObject
    {
        if (!_isReady)
        {
            Debug.LogWarning("ConfigManager not ready yet, returning null.");
            return null;
        }
        if (_configs.TryGetValue(typeof(T), out ScriptableObject config))
            return config as T;
        Debug.LogError($"Config of type {typeof(T).Name} not found.");
        return null;
    }
}
