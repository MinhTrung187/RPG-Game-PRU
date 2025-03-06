using MainCharacter;
using UnityEngine;

namespace Enemies.Skeleton
{
    public class SkeletonBattleState : EnemyState
    {
        private EnemySkeleton _skeleton;
        private Transform _player;
        private int _moveDir;

        public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton skeleton) :
            base(enemyBase, stateMachine, animBoolName)
        {
            _skeleton = skeleton;
        }

        public override void Enter()
        {
            base.Enter();

            // _player = GameObject.Find("Player").transform;
            AttachCurrentPlayerIfNotExists();
            
            //if player in attack range, block skeleton movement
            if (PlayerInAttackRange() && !CanAttack())
            {
                _skeleton.SetZeroVelocity();
                StateMachine.ChangeState(_skeleton.IdleState);
            }
        }

        public override void Update()
        {
            base.Update();

            if (_skeleton.IsPlayerDetected())
            {
                StateTimer = _skeleton.battleTime;

                if (_skeleton.IsGroundDetected() && _skeleton.IsPlayerDetected().distance <= _skeleton.attackDistance &&
                    CanAttack())
                {
                    StateMachine.ChangeState(_skeleton.AttackState);
                    return;
                }
            }
            else
            {
                if (StateTimer < 0 || Vector2.Distance(_player.transform.position, _skeleton.transform.position) > 7)
                {
                    StateMachine.ChangeState(_skeleton.IdleState);
                }
            }

            _moveDir = _player.position.x > _skeleton.transform.position.x ? 1 : -1;

            //if player in attack range, block skeleton movement
            if (PlayerInAttackRange())
            {
                _skeleton.SetZeroVelocity();
                StateMachine.ChangeState(_skeleton.IdleState);
                return;
            }

            _skeleton.SetVelocity(_skeleton.moveSpeed * _moveDir, Rb.linearVelocity.y);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public bool CanAttack()
        {
            AttachCurrentPlayerIfNotExists();
            
            if (Mathf.Approximately(_skeleton.lastTimeAttacked, 0) || Time.time >= _skeleton.lastTimeAttacked + _skeleton.attackCooldown)
            {
                // _skeleton.lastTimeAttacked = Time.time;
                return true;
            }

            return false;
        }

        public bool PlayerInAttackRange()
        {
            AttachCurrentPlayerIfNotExists();
            
            var result = _skeleton.IsPlayerDetected().distance <= _skeleton.attackDistance &&
                   (_skeleton.FacingDir == -1 && _player.transform.position.x <= _skeleton.transform.position.x ||
                    _skeleton.FacingDir == 1 && _player.transform.position.x >= _skeleton.transform.position.x);
            
            return result;
        }

        private void AttachCurrentPlayerIfNotExists()
        {
            if (_player == null)
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.player !=null)
        {
            _player = PlayerManager.Instance.player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found, attempting to find by tag...");
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                _player = foundPlayer.transform;
            }
        }
    }
        }
    }
}