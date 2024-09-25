using UnityEngine;


public class SkillConfig
{
    public float damageFactor = 1f;
    public float radius = 1.6f;
    public float range = 0f;
    public float rangeXOffset = 0.6f;
    public float rangeYOffset = 0.1f;
    public int dir = 1;


    public SkillConfig(float dmgFactor, float radius, float range, float rangeXOffset = 0, float rangeYOffset = 0 , int dir = 1)
    {
        this.damageFactor = dmgFactor;
        this.radius = radius;
        this.range = range;
        this.rangeXOffset = rangeXOffset;
        this.rangeYOffset = rangeYOffset;
        this.dir = dir;
    }
}

public class SkillCost
{
    public int hp;
    public int mp;
    public int ap;
}

public abstract class AreaSkill : Skill
{
    public int hitCount {  get; protected set; }
    public AreaSkill(SkillType type, string name, string key, float duration = 0, float cd = 0, bool isBusy = true) :
        base(type, name, key, duration, cd, isBusy)
    {
    }
    
    public override void AnimeHit()
    {   
        hitCount ++;
        foreach (var hit in GetHitsObjcts())
        {
            var target = hit.collider.gameObject.GetComponent<Character>();
            if (target != null && target.CanBeTargetOf(cha, this))
            {
                HitObject(hit.collider.gameObject);
            }
        }
    }

    public abstract RaycastHit2D[] GetHitsObjcts();


    public abstract void HitObject(GameObject obj);
}

