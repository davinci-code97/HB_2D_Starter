using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected CombatText combatTextPrefab;

    protected bool IsDead { get; set; }

    private float hp;

    private string currentAnimName;

    private void Start() {
        OnInit();
    }

    public virtual void OnInit() {
        IsDead = false;
        hp = 100f;
        healthBar.OnInit(100f, transform);
    }

    public virtual void OnDespawn() { 
        
    }

    protected virtual void OnDeath() {
        IsDead = true;
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }

    protected void ChangeAnim(string animName) {
        if (currentAnimName != animName) {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }

    public void OnHit(float damage) {
        if (!IsDead) {
            hp -= damage;

            if (hp <= 0) {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHP(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        } 
    }


}
