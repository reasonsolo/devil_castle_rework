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
public class Dash: Skill {

    private float dashSpeed = 15f;
    public Dash() : base("Dash", "Fire3", 0.3f, 1f)
    {
    }
    public override void Start() {
        base.Start();
        cha.rb.velocity = new Vector2(dashSpeed * cha.facingDir, 0);
    }
    public override void Update()
    {
        base.Update();
        cha.rb.velocity = new Vector2(dashSpeed * cha.facingDir, 0);
    }
}
