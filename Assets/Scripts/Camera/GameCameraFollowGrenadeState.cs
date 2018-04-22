using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraFollowGrenadeState : AbsState
{
    private const float FollowSpeed = 0.2f;
    private const float FollowDistanceY = 5f;
    private const float FollowDistanceZ = 3f;
    private const float FollowDistanceX = 3f;


    Grenade m_grenade;

    /// <summary>
    /// Constructor
    /// </summary>
    public GameCameraFollowGrenadeState(Grenade grenade)
    {
        m_grenade = grenade;
    }

    /// <summary>
    /// Follow it
    /// </summary>
    /// <param name="entity"></param>
    public override void Update(IStateMachineEntity entity)
    {
        GameCamera camera = (GameCamera)entity;
        //Vector3 fromCameraToGrenade = (m_grenade.transform.position - camera.transform.position).normalized;

        Vector3 targetPosition = m_grenade.transform.position + new Vector3(FollowDistanceX, FollowDistanceY, FollowDistanceZ);
        if(Vector3.Distance(camera.transform.position, targetPosition) > 1)
            camera.transform.position += (targetPosition - camera.transform.position).normalized * FollowSpeed;
        //camera.transform.SetParent(m_grenade.transform, true);
        camera.transform.LookAt(m_grenade.transform);
    }

    public override void Exit(IStateMachineEntity entity)
    {
        GameCamera camera = (GameCamera)entity;
        camera.transform.parent = null;
    }
}
