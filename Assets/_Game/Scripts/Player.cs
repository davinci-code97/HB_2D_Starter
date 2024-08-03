using UnityEngine;

public class Player : Character
{
    const string COIN_PLAYERPREFS = "coin";

    const string IDLE_ANIMATION_TRIGGER = "idle";
    const string RUN_ANIMATION_TRIGGER = "run";
    const string ATTACK_ANIMATION_TRIGGER = "attack";
    const string DIE_ANIMATION_TRIGGER = "die";
    const string JUMP_ANIMATION_TRIGGER = "jump";
    const string THROW_ANIMATION_TRIGGER = "throw";
    const string FALL_ANIMATION_TRIGGER = "fall";

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 10f;

    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    private int coinCount = 0;

    private bool isGrounded = false;
    //private bool isJumping = false;
    private bool isAttacking;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 savePoint;
    public Vector3 SavePoint => savePoint;

    private void Awake() {
        coinCount = PlayerPrefs.GetInt(COIN_PLAYERPREFS, 0);
    }

    private void Update() {
        if (IsDead) return;

        if (isAttacking) return;

        isGrounded = GroundCheck();

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Flip sprite
        if (horizontalInput != 0) transform.rotation = Quaternion.Euler(new Vector3(0, horizontalInput > 0 ? 0 : 180, 0));

        // Run
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (isGrounded) {
            // Idle
            if (horizontalInput != 0) ChangeAnim(RUN_ANIMATION_TRIGGER);
            else ChangeAnim(IDLE_ANIMATION_TRIGGER);

            // Jump
            if (verticalInput > 0 || Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }

            // Attack
            if (Input.GetKeyDown(KeyCode.J)) {
                Attack();
            }

            // Throw
            if (Input.GetKeyDown(KeyCode.K)) {
                Throw();
            }

        }

        // Fall
        if (!isGrounded && rb.velocity.y < 0f) {
            //isJumping = false;
            ChangeAnim(FALL_ANIMATION_TRIGGER);
        }

    }

    public override void OnInit() {
        base.OnInit();
        isAttacking = false;

        transform.position = savePoint;
        DeactiveAttack();
        
        NewSavePoint();
        UIManager.instance.SetCoin(coinCount);
    }

    public override void OnDespawn() {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath() {
        base.OnDeath();
    }

    private bool GroundCheck() {
        float groundCheckHeight = 1.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckHeight, groundLayer);

        return hit.collider != null;
    }

    public void Jump() {
        ChangeAnim(JUMP_ANIMATION_TRIGGER);
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    public void Attack() {
        if (isGrounded) {
            rb.velocity = Vector2.zero;
            ChangeAnim(ATTACK_ANIMATION_TRIGGER);
            isAttacking = true;
            Invoke(nameof(ResetAttack), 0.5f);
            ActiveAttack();
            Invoke(nameof(DeactiveAttack), .3f);
        }
    }

    public void Throw() {
        if (isGrounded) {
            rb.velocity = Vector2.zero;
            ChangeAnim(THROW_ANIMATION_TRIGGER);
            isAttacking = true;
            Invoke(nameof(ResetAttack), 0.5f);

            Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
        }
    }

    private void ResetAttack() {
        isAttacking = false;
        ChangeAnim(IDLE_ANIMATION_TRIGGER);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Coin")) {
            coinCount++;
            PlayerPrefs.SetInt(COIN_PLAYERPREFS, coinCount);
            UIManager.instance.SetCoin(coinCount);
            Destroy(collision.gameObject);
        }

        const string DEATHZONE_TAG = "DeathZone";
        if (collision.CompareTag(DEATHZONE_TAG)) {
            IsDead = true;
            ChangeAnim(DIE_ANIMATION_TRIGGER);
            Invoke(nameof(OnInit), 1f);
        }
    }

    internal void NewSavePoint() {
        savePoint = transform.position;
    }

    private void ActiveAttack() {
        attackArea.SetActive(true);
    }

    private void DeactiveAttack() {
        attackArea.SetActive(false);
    }

    public void SetMove(float horizontalInput) {
        this.horizontalInput = horizontalInput;
    }

}
