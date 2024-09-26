using UnityEngine;

public class CharacterAnimeTrigger: MonoBehaviour {

    protected Character cha;
    public CharacterAnimeTrigger() {
    }

    void Start()
    {
        cha = GetComponentInParent<Character>();
    }

    void AnimeFinish() {
        cha.sm.currState.AnimeFinish();
    }

    void AnimeHit()
    {
        cha.sm.currState.AnimeHit();
    }

}