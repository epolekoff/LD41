using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraFollowBulletState : AbsState
{
    private const float FollowSpeed = 0.1f;
    private const float FollowDistanceY = 0.75f;
    private const float FollowDistanceZ = -1f;
    private const float FollowDistanceX = 0.5f;


    Bullet m_bullet;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="bullet"></param>
    public GameCameraFollowBulletState(Bullet bullet)
    {
        m_bullet = bullet;
    }

    /// <summary>
    /// Follow it
    /// </summary>
    /// <param name="entity"></param>
    public override void Update(IStateMachineEntity entity)
    {
        GameCamera camera = (GameCamera)entity;
        camera.transform.rotation = Quaternion.LookRotation(m_bullet.transform.position - camera.transform.position);
        Vector3 targetBulletPosition = m_bullet.transform.position + (m_bullet.transform.right * FollowDistanceZ) + (m_bullet.transform.up * FollowDistanceY) + (m_bullet.transform.forward * FollowDistanceX);
        camera.transform.position = targetBulletPosition;// (targetBulletPosition - camera.transform.position) * FollowSpeed;
        camera.transform.SetParent(m_bullet.transform, true);
    }

    public override void Exit(IStateMachineEntity entity)
    {
        GameCamera camera = (GameCamera)entity;
        camera.transform.parent = null;
    }
}
