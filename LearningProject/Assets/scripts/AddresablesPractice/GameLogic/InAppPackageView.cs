using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class InAppPackageView : AddresableConfigLoader<InAppPackageConfig>
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _priceText;

    private InAppPackageConfig _config;

    protected override void ApplyConfig()
    {
        if (_title != null)
            _title.text = _config.packageName;
        if (_priceText != null)
            _priceText.text = $"{_config.price} {_config.currency}";
    }
}


