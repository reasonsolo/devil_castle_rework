using System.Collections.Generic;
using UnityEngine;


class PlayerAttribute : CharacterAttribute
{
    public PlayerAttribute(int level, int hp, int mp, int ap, int attack, int defense, int movespeed) : 
        base(level, hp, mp, ap, attack, defense, movespeed)
    {
    }
    public PlayerAttribute() : base(1, 100, 100, 100, 10, 5, 5)
    {
        this.jumpForce = 12;
    }
}

public class Player : Character
{

    #region States
    public PlayerIdle idle;
    public PlayerMove move;
    public PlayerJump jump;
    public PlayerFall fall;
    public PlayerWallSlide wallSlide;
    public PlayerHit hit;
    public PlayerDie die;
    #endregion

    public SkillManager skillManager;

    public PlayerDash dash;
    public PrimaryAttack attack;

    public Player()
    {
        charType = CharacterType.Player;
        attr = new PlayerAttribute();
    }

    public override void HitBy(Skill skill)
    {
        base.HitBy(skill);
        if (skill.skillType == SkillType.Attack)
        {
            sm.ChangeState(hit);
        }
    }

    protected override void Start()
    {
        base.Start();
        dash = new PlayerDash(this);
        attack = new PrimaryAttack();

        List<Skill> skills = new List<Skill> {
            dash,
            attack
         };
        skillManager = new SkillManager(this, skills);

        sm = new CharacterStateMachine(this);
        idle = new PlayerIdle("Idle", this);
        move = new PlayerMove("Move", this);
        fall = new PlayerFall("Fall", this);
        jump = new PlayerJump("Jump", this);
        hit = new PlayerHit("Hit", this);
        die = new PlayerDie("die", this);
        wallSlide = new PlayerWallSlide("WallSlide", this);
        sm.Initialize(idle);
    }

    protected override void Update()
    {
        if (!sm.currState.isBusy)
        {
            base.Update();
        }
    }
    
}
public abstract class PlayerState : CharacterState
{
    protected Player player;
    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;
    public PlayerState(string stateName, Player player) : base(stateName, player)
    {
        this.player = player;
        this.rb = player.rb;
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        base.Update();
        if (!player.isBusy)
        {
            Skill skill = player.skillManager.GetActivateSkill();
            if (skill != null) {
                sm.ChangeState(new PlayerSkillState(skill, player));
                return;
            }
        }
    }

}