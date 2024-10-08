using UnityEngine;

public class SkillArea
{
    public float radius;
    public float range;
    public float rangeXOffset = 0;
    public float rangeYOffset = 0;
    public int dir = 1;

    public SkillArea(float radius, float range, float rangeXOffset, float rangeYOffset, int dir)
    {
        this.radius = radius;
        this.range = range;
        this.rangeXOffset = rangeXOffset;
        this.rangeYOffset = rangeYOffset;
        this.dir = dir;
    }

    public RaycastHit2D[] GetHitsObjcts(Vector3 position, int facingDir)
    {
        return Physics2D.CircleCastAll(Center(position, facingDir), radius , Vector2.right * dir * facingDir, range);
    }

    public Vector2 Center(Vector3 position, int facingDir)
    {
        return new Vector2(position.x + dir * facingDir * (rangeXOffset + radius),
            position.y + rangeYOffset);
    }
}

public class SkillConfig
{
    public float damageFactor = 1f;
    public SkillArea area;

    public SkillConfig(float dmgFactor, float radius, float range, float rangeXOffset = 0, float rangeYOffset = 0 , int dir = 1)
    {
        this.damageFactor = dmgFactor;
        area = new SkillArea(radius, range, rangeXOffset, rangeYOffset, dir);
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
    public int hitCount { get; protected set; }
    public AreaSkill(SkillType type, string name, string key, float duration = 0, float cd = 0, bool isBusy = true) :
        base(type, name, key, duration, cd, isBusy)
    {
    }
    
    public override void AnimeHit()
    {   
        hitCount ++;
        var hitObjs = GetHitsObjcts();
        Debug.Log("hit obj " + hitObjs.Length + " count " + hitCount);
        foreach (var hit in hitObjs)
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

