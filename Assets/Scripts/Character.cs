using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Components
    public Animator anim;
    public Rigidbody2D rb;
    #endregion

    #region Attributes
    [Header("MoveInfo")] 
    public float moveSpeed = 7;
    public float jumpForce = 12;
    #endregion

    #region States
    public CharacterStateMachine sm {get; protected set;}
    #endregion

    #region Status
    public int facingDir = 1;
    public bool isBusy = false;
    #endregion

    #region EnvironChecks
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform facingCheck;
    [SerializeField] private Transform stageCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float facingCheckDistance;
    #endregion

    public bool IsGroundDetected => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public bool IsWallDetected => Physics2D.Raycast(facingCheck.position, facingDir > 0 ? Vector2.right : Vector2.left,
     facingCheckDistance, groundLayer);
    public bool IsStageDetected => !Physics2D.Raycast(facingCheck.position + new Vector3(facingDir * facingCheckDistance, 0), Vector2.down,
     groundCheckDistance, groundLayer);

    protected virtual void Start() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform;
        facingCheck = transform;
        stageCheck = transform;
        facingDir = 1;
    }

    protected virtual void Update() {
        ColliderChecks();
        sm.Update();
    }

    public virtual void Move(Vector2 velo) {
        rb.velocity = velo;
    }
    public virtual void Move(float xV, float yV) {
        rb.velocity = new Vector2(xV, yV);
    }

    public virtual void ColliderChecks() {

    }

    public virtual void Flip() {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
    }

    public virtual void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position,
        new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, 0));
        Gizmos.DrawLine(facingCheck.position,
       new Vector3(facingCheck.position.x + facingDir * facingCheckDistance, facingCheck.position.y, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(stageCheck.position + new Vector3(facingDir * facingCheckDistance, 0),
       stageCheck.position + new Vector3(facingDir * facingCheckDistance, -groundCheckDistance));
    }

    public virtual void AnimeFinished() {
        sm.currState.AnimeFinish();
    }
}
