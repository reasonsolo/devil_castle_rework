using UnityEditor.Callbacks;
using UnityEngine;
public class PlayerJump: PlayerState
{
    public PlayerJump(string stateName, Player player) : base(stateName, player)
    {
    }
    public override void Enter() { 
        base.Enter();
        player.Move(rb.velocity.x, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0) {
            sm.ChangeState(player.fall);
        }
    }
}

public class PlayerFall : PlayerState
{
    public PlayerFall(string stateName, Player player) : base(stateName, player)
    {
    }
    public override void Enter() {
        base.Enter();
     }
    public override void Update() {
        base.Update();
        if (player.IsGroundDetected || rb.velocity.y == 0)
        {
            sm.ChangeState(player.idle);
        }
     }
}