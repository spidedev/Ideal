using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objects/Object Settings")]

public class ObjectSettings : ScriptableObject
{
    public enum Type
    {
        NPC,
        Object
    }

    public Type type;
    
    [Header("Prompt Settings")]
    public bool CanCooldown;
    public float MaxTimeToPressDown, WaitAfterCooldown;
}
