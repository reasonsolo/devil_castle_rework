using System.Collections;
using Unity.Mathematics;
using UnityEngine;
public class PrimaryAttack : Skill
{
    public int comboCounter;
    public const int maxCombo = 3;
    private const string comboCounterName = "Skill-Attack-Combo";
    private float comboWindow = 0.2f;
    private float lastAttack;
    public Vector2 [] attackMoves = {
        new Vector2(0.15f, 0.1f),
        new Vector2(0, 0.1f),
        new Vector2(0.15f, -.1f)
    };
    public float attackSpeed  = 1f;

    public PrimaryAttack() : base("Attack", "Fire1")
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
        cha.Move(move.x * cha.facingDir, move.y);
    }

    public override void AnimeFinish() {
        base.AnimeFinish();
        comboCounter++;
        if (comboCounter == maxCombo) {
            comboCounter = 0;
        }
        lastAttack = Time.time;
    }

}