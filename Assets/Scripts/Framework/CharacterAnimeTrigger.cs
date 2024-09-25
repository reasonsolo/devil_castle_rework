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
        Debug.Log("anime finised " + cha.sm.currState);
        cha.sm.currState.AnimeFinish();
    }

    void AnimeHit()
    {
        
    }

}