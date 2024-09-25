using UnityEngine;
using System.Collections.Generic;

public class Enemy : Character
{
    public string type {  get; protected set; }
    public SkillManager skillManager { get; protected set; }
    public List<Skill> skills { get; protected set; }
    public EnemyIdle idle { get; protected set; }
    public EnemyMove move { get; protected set; }
    public EnemyBattle attack { get; protected set; }
    public EnemyHit hit { get; protected set; }
    public EnemyDie die { get; protected set; }
    
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
        this.charType = CharacterType.Enemy;
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
        attack = new EnemyBattle(this);
        hit = new EnemyHit(this);
        die = new EnemyDie(this);
        playerCheck = transform;
        player = GameObject.Find("Player").GetComponent<Player>();
        skillManager = new SkillManager(this, skills);
        sm.Initialize(idle);
    }

    protected override void Update()
    {
        if (attr.hp <= 0)
        {
            sm.ChangeState(die);
        }
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
        Move(attr.moveSpeed * facingDir, rb.velocity.y);
    }

    public override void HitBy(Skill skill)
    {
        base.HitBy(skill);
        if (skill.skillType == SkillType.Attack)
        {
            sm.ChangeState(hit);
        }
    }
}
public class EnemyAttack : AreaSkill
{
    protected SkillConfig config =
        new SkillConfig(1f, 1.6f, 0f, 0.5f);
    public EnemyAttack(string name, string key, float duration = 0, float cd = 0, bool isBusy = true)
        : base(SkillType.Attack, name, key, duration, cd, isBusy)
    {
    }

    public override RaycastHit2D[] GetHitsObjcts()
    {
        Vector2 attackCenter = new Vector2(cha.transform.position.x + cha.facingDir * config.dir * config.rangeXOffset + config.radius / 2,
            cha.transform.position.y + config.rangeYOffset);
        return Physics2D.CircleCastAll(attackCenter, config.range / 2, Vector2.right * config.dir * cha.facingDir, config.range);
    }

    public override void HitObject(GameObject obj)
    {
        var target = obj.GetComponent<Character>();
        if (target != null)
        {
            target.HitBy(this);
            target.ReduceHpMpAp((int)(config.damageFactor * (float)cha.attr.attack), 0, 0);
        }
    }
}