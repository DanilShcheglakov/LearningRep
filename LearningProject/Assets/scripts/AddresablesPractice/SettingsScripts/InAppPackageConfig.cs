using UnityEngine;

[CreateAssetMenu(fileName = "InAppPackageConfig", menuName = "Configs/InAppPackageConfig")]

public class InAppPackageConfig : ScriptableObject
{
    public string packageName = "Starter Pack";
    public int price = 499;
    public string currency = "USD";

}
