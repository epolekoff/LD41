using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShooterFactory
{

    private static string DefaultShooterPath = "Shooter";

    /// <summary>
    /// Create an entire team of shooters.
    /// </summary>
    /// <returns></returns>
    public static List<Shooter> CreateShooterTeam(int team, int teamSize)
    {
        List<Shooter> shooterList = new List<Shooter>();

        for(int i = 0; i < teamSize; i++)
        {
            Shooter shooter = CreateShooter(team);
            shooterList.Add(shooter);
        }

        return shooterList;
    }

    /// <summary>
    /// Create a single shooter.
    /// </summary>
    /// <returns></returns>
    public static Shooter CreateShooter(int team)
    {
        var shooterResource = Resources.Load(DefaultShooterPath);
        GameObject shooterObject = GameObject.Instantiate(shooterResource) as GameObject;
        Shooter shooter = shooterObject.GetComponent<Shooter>();

        return shooter;
    }
	
}
