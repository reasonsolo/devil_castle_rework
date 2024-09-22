using UnityEngine;

public class CharacterAnimeTrigger: MonoBehaviour {

    public Character cha;
    public CharacterAnimeTrigger() {
    }

    void Start()
    {
        cha = GetComponentInParent<Character>();
    }

    void AnimeFinish() {
        Debug.Log("anime finish" );
        cha.sm.currState.AnimeFinish();
    }

}