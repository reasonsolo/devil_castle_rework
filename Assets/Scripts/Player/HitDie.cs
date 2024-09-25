using UnityEngine;

public class PlayerHit : PlayerState
{
    public PlayerHit(string stateName, Player player) : base(stateName, player)
    {
    }

    public override void AnimeFinish()
    {
        base.AnimeFinish();
        isBusy = false;
        player.sm.ChangeState(player.idle);
    }

    public override void Enter()
    {
        base.Enter();
        isBusy = true;
    }

    public override void Update()
    {
        base.Update();
    }
}
public class PlayerDie: PlayerState
{
    public PlayerDie(string stateName, Player player) : base(stateName, player)
    {
    }

    public override void AnimeFinish()
    {
        base.AnimeFinish();
        isBusy = false;
    }

    public override void Enter()
    {
        base.Enter();
        isBusy = true;
    }

    public override void Update()
    {
        base.Update();
    }
}
