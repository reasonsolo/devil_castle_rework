using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillType
{
    Attack,
    Buff,
    Debuff,
    Heal,
    Movement,
    Summon,
}


public class Skill
{
    public SkillType skillType { get; protected set; }
    public string skillName { get; protected set; }
    public string description { get; protected set; }
    public float cd { get; protected set; }
    public float cdTimer { get; protected set; }

    public float duration { get; protected set; }
    public float timer {get; protected set; }
    public string keyName { get; protected set; }
    public Character cha { get; protected set; }
    public bool isBlocking { get; protected set; }
    public bool isAnimeFinished { get; protected set; }

    public Skill(SkillType type, string name, string key, float duration = 0, float cd = 0, bool isBlocking = true)
    {
        this.skillType = type;
        this.skillName = name;
        this.cd = cd;
        this.duration = duration;
        this.keyName = key;
        this.isAnimeFinished = false;
        this.isBlocking = isBlocking;
    }

    public void SetUser(Character cha) {
        this.cha = cha;
    }

    public virtual bool IsValid() {
        return cdTimer == 0;
    }

    public void Refresh()
    {
        cdTimer = 0;
    }

    public IEnumerator CoolDown(float startTime = 0f)
    {
        yield return new WaitForSeconds(cd - startTime);
        Debug.Log(skillName + " ready");
        Refresh();
    }

    public virtual void Start() {
        timer = 0;
        cdTimer = cd;
        isAnimeFinished = false;
        if (cd > 0) {
            IEnumerator cdCoro = CoolDown(0);
            cha.StartCoroutine(cdCoro);
        }
     }
    public virtual void Update()
    {
        timer += Time.deltaTime;
    }

    public virtual bool IsFinished() {
        if (duration > 0)
        {
            return timer >= duration;
        }
        else
        {
            return isAnimeFinished;
        }
    }
    public virtual void Finish() { }

    public virtual void AnimeFinish()
    {
        isAnimeFinished = true;
        if (isBlocking)
        {
            cha.isBusy = false;
        }
    }
    public virtual void AnimeHit()
    {
    }

    public bool IsReady => cdTimer <= 0;

    public virtual void DrawGizmos() {
    }
}

public class SkillManager
{
    protected Character cha;

    public List<Skill> skills { get; protected set; }

    public Dictionary<string, Skill> skillMap { get; protected set; }
    public Dictionary<string, string> keyNameMap { get; protected set; }
    public SkillManager(Character cha, List<Skill> skills)
    {
        this.cha = cha;
        skillMap = new Dictionary<string, Skill>();
        keyNameMap = new Dictionary<string, string>();
        foreach (Skill skill in skills)
        {
            skill.SetUser(cha);
            skillMap[skill.skillName] = skill;
            keyNameMap[skill.keyName] = skill.skillName;
        }
    }

    public bool IsSkillReady(string name)
    {
        return skillMap[name].IsReady;
    }

    public Skill GetSkill(string name)
    {
        return skillMap[name];
    }

    public Skill GetActivateSkill()
    {
        foreach (String keyName in keyNameMap.Keys)
        {
            if (Input.GetButtonDown(keyName))
            {
                Skill skill = skillMap[keyNameMap[keyName]];
                if (skill != null && skill.IsValid()) {
                    return skill;
                }
            }
        }
        return null;
    }

    public void Update()
    {
        foreach (Skill skill in skills)
        {
            skill.Update();
        }
    }

    public void RefreshAll()
    {
        foreach (Skill skill in skills)
        {
            skill.Refresh();
        }

    }
}

public class SkillState : CharacterState
{
    public Skill skill;
    public SkillState(Skill skill, Character cha) : base("Skill-" + skill.skillName, cha)
    {
        this.skill = skill;
        skill.SetUser(cha);
        this.isBusy = skill.isBlocking;
    }

    public override void Enter()
    {
        base.Enter();
        this.skill.Start();
    }
    public override void Update()
    {
        base.Update();
        skill.Update();
        //Debug.Log("skill " + skill.skillName + " finish " + skill.IsFinished());
        if (skill.IsFinished())
        {
            cha.sm.ChangeDefault();    
        }
    }

    public override void Exit()
    {
        skill.Finish();
        base.Exit();
    }

    public override void AnimeFinish()
    {
        base.AnimeFinish();
        skill.AnimeFinish();
        if (skill.IsFinished())
        {
            cha.sm.ChangeDefault();    
        }
    }
    public override void AnimeHit()
    {
        skill.AnimeHit();
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();
        skill.DrawGizmos();
    }
}
public class Dash: Skill {

    private float dashSpeedFactor = 3f;
    public Dash() : base(SkillType.Movement, "Dash", "Fire3", 0.3f, 1f)
    {
    }
    public override void Start() {
        base.Start();
        cha.rb.velocity = new Vector2(dashSpeedFactor * cha.attr.moveSpeed * cha.facingDir, 0);
    }
    public override void Update()
    {
        base.Update();
        cha.rb.velocity = new Vector2(dashSpeedFactor * cha.attr.moveSpeed * cha.facingDir, 0);
    }
}

