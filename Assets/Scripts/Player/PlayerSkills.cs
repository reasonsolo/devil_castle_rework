using UnityEngine;

public class PlayerSkillState : SkillState
{
    protected Player player;
    public PlayerSkillState(Skill skill, Player player) : base(skill, player)
    {
        this.player = player;
    }

    public override void Update()
    {
        base.Update();
        if (this.skill.IsFinished()) {
            player.sm.ChangeState(player.idle);
        }
    }
}

public class PlayerDash : Dash
{
    public Player player;
    public PlayerDash(Player player)
    {
        this.player = player;
        this.isBusy = false; 
    }


    public override void Update()
    {
        base.Update();
        if (Input.GetButtonDown(player.attack.keyName))
        {
            player.attack.SetCombo(PrimaryAttack.maxCombo-1);
            player.sm.ChangeState(new PlayerSkillState(player.attack, player));
        }
    }
}
