using UnityEngine;
public class PlayerWallSlide : PlayerState
{

    private float wallJumpFactor = Mathf.Sqrt(0.5f);
    private float wallSlideFactor = Mathf.Sqrt(0.5f);
    public PlayerWallSlide(string stateName, Player player) : base(stateName, player)
    {
        this.isBusy = true;
    }

    public override void Enter()
    {
        base.Enter();
        player.Move(0, player.rb.velocity.y * wallSlideFactor);
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsWallDetected || player.IsGroundDetected)
        {
            sm.ChangeState(player.idle);
            return;
        }
        if (yInput < 0)
        {
            sm.ChangeState(player.fall);
            return;
        }
        if (Input.GetButtonDown("Jump"))
        {
            player.Flip();
            player.Move(wallJumpFactor * player.jumpForce * player.facingDir, wallJumpFactor * player.jumpForce);
            sm.ChangeState(player.jump);
            return;
        }
        player.Move(0, player.rb.velocity.y * wallSlideFactor);
    }
}