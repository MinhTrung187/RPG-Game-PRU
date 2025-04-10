using UnityEngine;

namespace Enemies.Swordman
{
    public class EnemySwordmanGroundedState : EnemyState
    {
        protected EnemySwordman enemy;
        protected Transform player;
        protected EnemySwordmanGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySwordman enemy) : base(enemyBase, stateMachine, animBoolName)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();
            player= GameObject.Find("Player").transform;
        }
        public override void Update()
        {
            base.Update();
            if (enemy.IsPlayerDetected() ||Vector2.Distance(enemy.transform.position, player.position) <2) 
            { 
                StateMachine.ChangeState(enemy.BattleState);
            }
        }
        public override void Exit()
        {
            base.Exit();
        }


    }

}
