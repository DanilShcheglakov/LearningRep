using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Player : AddresableConfigLoader<PlayerConfig>
{
    private PlayerConfig _config;

    private float _moveSpeed;
    private int _health;
    private string _name;

    protected override void ApplyConfig()
    {
        _moveSpeed = _config.moveSpeed;
        _health = _config.health;
        _name = _config.name;
    }
}
