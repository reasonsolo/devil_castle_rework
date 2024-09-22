using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Skill
{
    public string skillName { get; protected set; }
    public string description { get; protected set; }
    public float cd { get; protected set; }
    public float cdTimer { get; protected set; }

    public float timer {get; protected set; }

    public string keyName { get; protected set; }
    public Character cha { get; protected set; }

    public float duration { get; protected set; }

    public bool IsAnimeFinished { get; protected set; }

    public Skill(string name, string key, float duration = 0, float cd = 0)
    {
        this.skillName = name;
        this.cd = cd;
        this.duration = duration;
        this.keyName = key;
        this.IsAnimeFinished = false;
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
        IsAnimeFinished = false;
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
            return IsAnimeFinished;
        }
    }
    public virtual void Finish() { }

    public virtual void AnimeFinish() {
        IsAnimeFinished = true;
     }

    public bool IsReady => cdTimer <= 0;
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
    }

}