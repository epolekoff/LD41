using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraLingerState : AbsState {

    private const float MoveSpeed = 0.1f;

    public override void Update(IStateMachineEntity entity)
    {
        base.Update(entity);

        GameCamera camera = (GameCamera)entity;
        camera.transform.position += -camera.transform.forward * MoveSpeed;
    }
}
