using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(string stateName, Player player) : base(stateName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.Move(0, 0);
    }
    public override void Update()
    {
        base.Update();
        //if (Input.GetButtonDown("Fire1")) {
        //    sm.ChangeState(player.attack);
        //    return;
        //}
        if (Input.GetButtonDown("Jump") && player.IsGroundDetected) {
            sm.ChangeState(player.jump);
            return;
        }
        if (!player.IsGroundDetected)
        {
            if (player.IsWallDetected)
            {
                sm.ChangeState(player.wallSlide);
            }
            else if (player.rb.velocity.y < 0) 
            {
                sm.ChangeState(player.fall);
            }
            return;
        }
        if (xInput != 0 && !cha.isBusy)
        {
            sm.ChangeState(player.move);
        }
    }
}

public class PlayerMove : PlayerState
{
    public PlayerMove(string stateName, Player player) : base(stateName, player)
    {
    }

    public override void Enter()
    {
        rb = player.rb;
        float xInput = Input.GetAxisRaw("Horizontal");
        if (player.facingDir * xInput < 0)
        {
            player.Flip();
        }
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (xInput == 0)
        {
            player.sm.ChangeState(player.idle);
            return;
        }
        if (player.facingDir * xInput < 0)
        {
            player.Flip();
        }
        player.Move(player.facingDir * player.moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump")) {
            sm.ChangeState(player.jump);
            return;
        } 
    }
}
