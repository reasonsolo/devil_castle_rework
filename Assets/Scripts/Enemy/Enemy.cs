using UnityEngine;
using System.Collections.Generic;


public class EnemyState : CharacterState
{
    public Enemy enemy;
    public EnemyState(string stateName, Enemy enemy) : base(stateName, enemy)
    {
        this.enemy = enemy;
    }
};

public class EnemyIdle : EnemyState
{
    public EnemyIdle(Enemy enemy) : base(enemy.type + "Idle", enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.Move(0, 0);
    }

    public override void Update()
    {
        base.Update();
        enemy.Move(0, 0);
        if (enemy.IsPlayerTargeted)
        {
            enemy.sm.ChangeState(enemy.move);
        }
    }
}
public class EnemyMove: EnemyState
{
    public EnemyMove(Enemy enemy) : base(enemy.type + "Move", enemy)
    {
    }

    public override void Update()
    {
        base.Update();
        Gizmos.color = Color.red;
        Debug.Log("enemy " + enemy.IsPlayerTargeted + " " + enemy.IsStageDetected + " " + enemy.IsPlayerDetected);
        if (enemy.IsPlayerDetected)
        {
            enemy.sm.ChangeState(enemy.attack);
            return;
        }
        if (enemy.IsPlayerTargeted && !enemy.IsStageDetected)
        {
            enemy.ChasePlayer();
        }
        else
        {
            enemy.sm.ChangeState(enemy.idle);
        }

    }
}
public class EnemyAttack: EnemyState
{
    public EnemyAttack(Enemy enemy) : base(enemy.type + "Attack", enemy)
    {
    }

    public override void AnimeFinish()
    {
        base.AnimeFinish();
        enemy.isBusy = false;
        enemy.sm.ChangeState(enemy.idle);
    }

    public override void Enter()
    {
        enemy.isBusy = true;
        base.Enter();
        enemy.Move(0, 0);
    }

    public override void Update()
    {
        base.Update();
        enemy.Move(0, 0);
    }
}

public class Enemy : Character
{
    public string type {  get; protected set; }
    public SkillManager skillManager { get; protected set; }
    public List<Skill> skills { get; protected set; }
    public EnemyIdle idle { get; protected set; }
    public EnemyMove move { get; protected set; }
    public EnemyAttack attack { get; protected set; }
    
    public float xTargetDistance { get; protected set; }
    public float yTargetDistance { get; protected set; }



    protected Player player;
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float playerCheckDistance;
    [SerializeField] protected float playerTargetDistance;
    [SerializeField] protected float playerTargetDistanceFromBack;

    public bool IsPlayerDetected => Physics2D.Raycast(playerCheck.position, facingDir > 0 ? Vector2.right : Vector2.left,
     playerCheckDistance, playerLayer);
    public bool IsPlayerTargeted => Physics2D.Raycast(playerCheck.position - new Vector3(playerTargetDistanceFromBack * facingDir, 0), facingDir > 0 ? Vector2.right : Vector2.left,
     playerTargetDistance + playerTargetDistanceFromBack, playerLayer);


    public Enemy(string type)
    {
        this.type = type;
        this.skills = new List<Skill>();
        xTargetDistance = 7;
        yTargetDistance = 0;
    }

    protected override void Start()
    {
        base.Start();
        sm = new CharacterStateMachine(this);
        idle = new EnemyIdle(this);
        move = new EnemyMove(this);
        attack = new EnemyAttack(this);
        playerCheck = transform;
        player = GameObject.Find("Player").GetComponent<Player>();
        skillManager = new SkillManager(this, skills);
        sm.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void OnDrawGizmos() {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position- new Vector3(playerTargetDistanceFromBack * facingDir, 0),
        playerCheck.position + new Vector3(facingDir * playerTargetDistance, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position,
        playerCheck.position + new Vector3(facingDir * playerCheckDistance, 0));
    }

    public virtual void TurnToPlayer()
    {
        float xDist = player.transform.position.x - transform.position.x;
        if (xDist * facingDir < 0)
        {
            Flip();
        }
    }
    public virtual void ChasePlayer()
    {
        TurnToPlayer();
        Move(moveSpeed * facingDir, rb.velocity.y);
    }
}