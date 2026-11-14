using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatePortal", menuName = "Scriptable Objects/PlayerStatePortal")]
public class PlayerStatePortal : ScriptableObject
{
    [HideInInspector]
    public Vector2 playerPosition;
}