using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{

    #region States
    public PlayerIdle idle;
    public PlayerMove move;
    public PlayerJump jump;
    public PlayerFall fall;
    public PlayerWallSlide wallSlide;
    #endregion

    public SkillManager skillManager;

    public PlayerDash dash;
    public PrimaryAttack attack;

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
        wallSlide = new PlayerWallSlide("WallSlide", this);
        sm.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();
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