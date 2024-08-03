using UnityEngine;

public class Enemy : Character
{
    const string IDLE_ANIMATION_TRIGGER = "idle";
    const string RUN_ANIMATION_TRIGGER = "run";
    const string ATTACK_ANIMATION_TRIGGER = "attack";

    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject attackArea;

    private IState currentState;
    private bool isRight = true;

    private Character target;
    public Character Target => target;

    private void Update() {
        if (currentState != null && !IsDead) {
            currentState.OnExecute(this);
        }

    }

    public override void OnInit() {
        base.OnInit();

        ChangeState(new IdleState());
        DeactiveAttack();
    }

    public override void OnDespawn() {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    protected override void OnDeath() {
        ChangeState(null);
        base.OnDeath();
    }

    public void ChangeState(IState newState) {
        if (currentState != null) {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null) {
            currentState.OnEnter(this);
        }
    }

    public void Moving() {
        ChangeAnim(RUN_ANIMATION_TRIGGER);
        rigidbody.velocity = moveSpeed * transform.right;
    }

    public void StopMoving() {
        rigidbody.velocity = Vector2.zero;
        ChangeAnim(IDLE_ANIMATION_TRIGGER);
    }

    public void Attack() {
        ChangeAnim(ATTACK_ANIMATION_TRIGGER);
        ActiveAttack();
        Invoke(nameof(DeactiveAttack), .3f);
    }

    public bool IsTargetInRange() {
        return target != null && Vector2.Distance(transform.position, target.transform.position) <= attackRange;
    }
    public void ChangeDirection(bool isRight) {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }
    
    internal void SetTarget(Character character) {
        this.target = character;

        if (IsTargetInRange()) {
            ChangeState(new AttackState());
        } else if (character != null) {
            ChangeState(new PatrolState());
        } else {
            ChangeState(new IdleState());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyWall")) {
            ChangeDirection(!isRight);
        }
    }

    private void ActiveAttack() {
        attackArea.SetActive(true);
    }

    private void DeactiveAttack() {
        attackArea.SetActive(false);
    }

}
