using UnityEngine;

public class PatrolState : IState
{
    float timer;
    float randomTime;
    float minRandomTime = 3f;
    float maxRandomTime = 6f;

    public void OnEnter(Enemy enemy) {
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);
    }

    public void OnExecute(Enemy enemy) {
        timer += Time.deltaTime;

        if (enemy.Target != null) {
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);
            if (enemy.IsTargetInRange()) {
                enemy.ChangeState(new AttackState());
            } else {
                enemy.Moving();
            }
        }

        if (timer < randomTime) {
            enemy.Moving();
        } else {
            enemy.ChangeState(new IdleState());
        }

    }

    public void OnExit(Enemy enemy) {
        
    }

}
