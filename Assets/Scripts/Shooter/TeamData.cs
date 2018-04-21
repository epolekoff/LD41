using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTeam", menuName = "Shooter/Team")]
public class TeamData : ScriptableObject {

    /// <summary>
    /// Size of the team.
    /// </summary>
    [SerializeField()]
    public int TeamSize = 4;

    /// <summary>
    /// The layout of where the team members spawn on the map.
    /// </summary>
    [SerializeField()]
    public List<Vector2> StartingPositions;

    /// <summary>
    /// The material for this team.
    /// </summary>
    [SerializeField()]
    public Material TeamColor;
}
