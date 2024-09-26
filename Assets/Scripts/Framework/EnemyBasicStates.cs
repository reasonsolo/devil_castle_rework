using UnityEngine;
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
public class EnemyBattle: EnemyState
{
    public EnemyBattle(Enemy enemy) : base(enemy.type + "Attack", enemy)
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

public class EnemyHit : EnemyState
{
    public EnemyHit(Enemy enemy) : base(enemy.type + "Hit", enemy)
    {
    }

    public override void AnimeFinish()
    {
        base.AnimeFinish();
        isBusy = false;
        sm.ChangeDefault();
    }

    public override void Enter()
    {
        base.Enter();
        isBusy = true;
    }

    public override void Update()
    {
        base.Update();
    }
}
public class EnemyDie: EnemyState
{
    public EnemyDie(Enemy enemy) : base(enemy.type + "Die", enemy)
    {
        this.priority = 1000;
    }

    public override void AnimeFinish()
    {
        base.AnimeFinish();
        isBusy = false;
    }

    public override void Enter()
    {
        base.Enter();
        cha.Move(0, 0);
        isBusy = true;
    }

    public override void Update()
    {
        base.Update();
    }
}
