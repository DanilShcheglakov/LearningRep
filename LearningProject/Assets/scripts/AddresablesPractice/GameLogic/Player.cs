using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Player : MonoBehaviour
{
    private PlayerConfig _config;

    private float _moveSpeed;
    private int _health;
    private string _name;

    private void Start()
    {
        if (ConfigManager.Instance != null)
        {
            _config = ConfigManager.Instance.GetConfig<PlayerConfig>();
            ApplyConfig();
        }
        else
        {
            throw new System.Exception("ConfigManager not found!");
        }
    }

    private void ApplyConfig()
    {
        _moveSpeed = _config.moveSpeed;
        _health = _config.health;
        _name = _config.name;
    }
}
