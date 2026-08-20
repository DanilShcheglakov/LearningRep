using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public float moveSpeed = 1f;
    public int health = 100;
    public string pplayerName = "Hero";
}
