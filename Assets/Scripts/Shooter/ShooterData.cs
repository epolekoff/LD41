using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewShooter", menuName ="Shooter/ShooterData")]
public class ShooterData : ScriptableObject {

    /// <summary>
    /// Move Range
    /// </summary>
    [SerializeField()]
    public int MoveRange;

    /// <summary>
    /// Health 
    /// </summary>
    [SerializeField()]
    public int Health;
}
