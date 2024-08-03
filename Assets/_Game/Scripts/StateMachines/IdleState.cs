using UnityEngine;

public class IdleState : IState
{
    float timer;
    float randomTime;
    float minRandomTime = 2f;
    float maxRandomTime = 4f;

    public void OnEnter(Enemy enemy) {
        enemy.StopMoving();
        timer = 0;
        randomTime = Random.Range(minRandomTime, maxRandomTime);
    }

    public void OnExecute(Enemy enemy) {
        timer += Time.deltaTime;

        if (timer > randomTime) {
            enemy.ChangeState(new PatrolState());
        }

    }

    public void OnExit(Enemy enemy) {
        
    }

}
