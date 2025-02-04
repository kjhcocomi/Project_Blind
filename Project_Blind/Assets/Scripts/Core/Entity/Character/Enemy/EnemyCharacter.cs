using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blind
{
    public class EnemyCharacter : Character
    {
        protected CharacterController2D _characterController2D;
        protected Rigidbody2D rigid;
        protected SpriteRenderer _sprite;

        [SerializeField] protected Vector2 _sensingRange;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _runSpeed;
        [SerializeField] protected float _attackCoolTime;
        [SerializeField] protected float _attackSpeed;
        [SerializeField] protected Vector2 _attackRange;
        [SerializeField] protected int _damage;
        [SerializeField] protected float _stunTime;

        protected GameObject player;
        protected MeleeAttackable _attack;
        public LayerMask WallLayer;
        public Transform WallCheck;
        protected Transform startingPosition;
        protected Vector2 patrolDirection;
        protected PlayerFinder playerFinder;
        protected EnemyAttack attackSense;

        // HP UI
        protected UI_UnitHP _unitHPUI = null;

        
        protected void Awake()
        {
            base.Awake();
            _attack = GetComponent<MeleeAttackable>();
            _sprite = GetComponent<SpriteRenderer>();
            _characterController2D = GetComponent<CharacterController2D>();
            rigid = GetComponent<Rigidbody2D>();
            CreateHpUI();
            playerFinder = GetComponentInChildren<PlayerFinder>();
        }

        protected void CreateHpUI()
        {
            Debug.LogWarning("??");
            // UI매니저로 UI_UnitHP 생성
            _unitHPUI = UIManager.Instance.ShowWorldSpaceUI<UI_UnitHP>();
            // UI에서 UnitHP 참조
            _unitHPUI.HP = Hp;
            // 유닛 움직이면 같이 움직이도록 Parent 설정
            _unitHPUI.transform.SetParent(transform);
            // UI에서 이 오브젝트의 정보가 필요할 수도 있으므로 참조
            _unitHPUI.Owner = gameObject;
            // 오브젝트의 머리 위에 위치하도록 설정
            _unitHPUI.SetPosition(transform.position, Vector3.up * 4);
        }

        public bool ReturnFacing()
        {
            //return _sprite.flipX;
            if (transform.localScale.x > 0)
                return true;
            else return false;
        }

        public void hitted(int dir)
        {
            Debug.Log("Enemy Hitted !");
            StartCoroutine(CoHitted());
        }

        protected virtual IEnumerator CoHitted()
        {
            return null;
        }

        protected override void onHurt()
        {
            return;
        }

        protected override void HurtMove(Facing enemyFacing)
        {
            _characterController2D.Move(new Vector2((float)enemyFacing*1, 0));
            return;
        }
    }
}