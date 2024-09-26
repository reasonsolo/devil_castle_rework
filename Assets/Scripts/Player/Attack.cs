using System.Collections;
using Unity.Mathematics;
using UnityEngine;
public class PrimaryAttack : AreaSkill
{
    public int comboCounter;
    public const int maxCombo = 3;
    private const string comboCounterName = "Skill-Attack-Combo";
    private float comboWindow = 0.2f;
    private float lastAttack;
    public Vector2[] attackMoves = {
        new Vector2(0.15f, 0.1f),
        new Vector2(0, 0.1f),
        new Vector2(0.15f, -.2f)
    };
    public float attackSpeed = 1f;

    private SkillConfig[] configs =
    {
        new SkillConfig(1f, 1.6f, 0f, 0.5f),
        new SkillConfig(1f, 1.6f, 0f, 0.5f),
        new SkillConfig(1f, 1.6f, 0.2f, 0.5f),
    };

    public PrimaryAttack() : base(SkillType.Attack, "Attack", "Fire1")
    {
    }

    public void SetCombo(int combo)
    {
        comboCounter = math.min(maxCombo - 1, combo);
        lastAttack = Time.time;
    }
    public override void Start()
    {
        if (lastAttack > 0 && Time.time - lastAttack > comboWindow)
        {
            comboCounter = 0;
        }
        base.Start();
        cha.anim.SetInteger(comboCounterName, comboCounter);
        Vector2 move = attackMoves[comboCounter];
        cha.Move(move.x * cha.facingDir, move.y);
    }
    public override void Update()
    {
        base.Update();
        cha.anim.SetInteger(comboCounterName, comboCounter);
        Vector2 move = attackMoves[comboCounter];
        cha.Move(move.x * cha.facingDir, move.y + cha.rb.velocity.y);
    }

    public override void AnimeFinish()
    {
        base.AnimeFinish();
        comboCounter++;
        if (comboCounter == maxCombo)
        {
            comboCounter = 0;
        }
        lastAttack = Time.time;
    }

    public override RaycastHit2D[] GetHitsObjcts()
    {
        var area = configs[comboCounter].area;
        return area.GetHitsObjcts(cha.transform.position, cha.facingDir);
    }

    public override void HitObject(GameObject obj)
    {
        var target = obj.GetComponent<Character>();
        if (target != null)
        {
            Debug.Log("target " + target + " hit by " + this);
            target.HitBy(this);
            var config = configs[comboCounter];
            target.ReduceHpMpAp((int)(config.damageFactor * (float)cha.attr.attack), 0, 0);
        }
    }
    public override void DrawGizmos()
    {
        if (timer > 0 && !IsFinished())
        {
            var area = configs[comboCounter].area;
            Gizmos.color = Color.green;
            var center = area.Center(cha.transform.position, cha.facingDir);
            Gizmos.DrawWireSphere(center, area.radius);
            Gizmos.DrawWireSphere(center + Vector2.right * area.dir * cha.facingDir * area.range, area.radius);

        }
    }
}