using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BoxCollider = Unity.Physics.BoxCollider;

namespace StatePattern{
    public class Player : MonoBehaviour{
        public static readonly int IsGroundedAnimationParameter = Animator.StringToHash("Is Grounded");
        public static readonly int IsRunningAnimationParameter = Animator.StringToHash("Is Running");
        public static readonly int IsCrouchingAnimationParameter = Animator.StringToHash("Is Crouching");
        public static readonly int IsAimingAnimationParameter = Animator.StringToHash("Is Aiming");
        public static readonly int JumpAnimationParameter = Animator.StringToHash("Jump");
        public static readonly int ShootAnimationParameter = Animator.StringToHash("Shoot");
        public static readonly int ShootingBlendAnimationParameter = Animator.StringToHash("Shooting Blend");
        public static readonly int AimingBlendAnimationParameter = Animator.StringToHash("Aiming Blend");
        public static readonly int SpeedXAnimationParameter = Animator.StringToHash("Speed X");

        [SerializeField] private PlayerState _currentState;

        private readonly ContactPoint2D[] _groundContact = new ContactPoint2D[1];
        private readonly Dictionary<PlayerState, IPlayerState> _states = new();
        private bool _canShoot = true;
        private Coroutine _changeStateCoroutine;

        [field: SerializeField] public float Speed{ get; private set; }
        [field: SerializeField] public float JumpHeight{ get; private set; }
        [field: SerializeField] public float ShotCooldown{ get; private set; }
        [field: SerializeField] public float MorphBallAcceleration{ get; private set; }
        [field: SerializeField] public BoxCollider2D ExitMorphColliderCheck{ get; private set; }
        [field: SerializeField] public Projectile ProjectilePrefab{ get; private set; }
        [field: SerializeField] public Transform ProjectileSpawn{ get; private set; }
        [field: SerializeField] public Transform WallGrabRaycast{ get; private set; }
        [field: SerializeField] public float WallGrabRaycastDistance { get; private set; }
        [field: SerializeField] public ContactFilter2D GroundContactFilter{ get; private set; }

        public PlayerState LastState{ get; private set; }
        public Rigidbody2D Rigidbody{ get; private set; }
        public BoxCollider2D BoxCollider{ get; private set; }
        public Animator Animator{ get; private set; }
        public float InitialGravityScale{ get; private set; }

        private void Awake(){
            Rigidbody = GetComponent<Rigidbody2D>();
            BoxCollider = GetComponent<BoxCollider2D>();
            Animator = GetComponent<Animator>();

            InitialGravityScale = Rigidbody.gravityScale;

            _states.Add(PlayerState.Idle, new IdlePlayerState());
            _states.Add(PlayerState.Running, new RunningPlayerState());
            _states.Add(PlayerState.Jumping, new JumpingPlayerState());
            _states.Add(PlayerState.Falling, new FallingPlayerState());
            _states.Add(PlayerState.Crouching, new CrouchingPlayerState());
            _states.Add(PlayerState.MorphBall, new MorphBallPlayerState());
            _states.Add(PlayerState.WallGrab, new WallGrabState());

            LastState = PlayerState.Idle;
            ChangeState(PlayerState.Idle);
        }

        private void Update(){
            if (_changeStateCoroutine == null){ _states[_currentState].Update(this); }
            Animator.SetFloat(SpeedXAnimationParameter, Rigidbody.velocity.x);
            Animator.SetBool(IsGroundedAnimationParameter, CheckGround());
        }

        private void FixedUpdate(){
            if (_changeStateCoroutine == null){ _states[_currentState].FixedUpdate(this); }
        }

        public void ChangeState(PlayerState state){
            if (_changeStateCoroutine != null){ return; }
            _changeStateCoroutine = StartCoroutine(ChangeStateCoroutine(state));
        }

        public bool CheckGround(){
            int contacts = Rigidbody.GetContacts(GroundContactFilter, _groundContact);
            return contacts > 0;
        }

        public void Jump(){
            Vector2 velocity = Rigidbody.velocity;
            velocity.y = JumpHeight;
            Rigidbody.velocity = velocity;
            ChangeState(PlayerState.Jumping);
        }

        public void Shoot(){
            if (!_canShoot){ return; }
            Animator.SetTrigger(ShootAnimationParameter);
            Animator.SetBool(IsAimingAnimationParameter, true);
            StartCoroutine(AdjustShootingBlend());
            StartCoroutine(ShotCooldownCoroutine());
        }

        public void SpawnProjectile(){
            Projectile projectile = Instantiate(ProjectilePrefab, ProjectileSpawn.position, Quaternion.identity);
            Vector2 direction = ProjectileSpawn.right;
            direction.x *= transform.localScale.x;
            projectile.SetDirection(direction);
        }

        public WaitUntil WaitUntilCurrentAnimationFinishes(){
            return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        }

        public IEnumerator PlayAnimation(string animationName){
            Animator.Play(animationName, 0, 0);
            yield return new WaitUntil(() =>
                Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash(animationName));
        }
        
        public void ResetScale(){
            transform.localScale = Vector3.one;

        }
        
        public void SetScaleAfterMorphing(){
            float xScale = ((MorphBallPlayerState) _states[PlayerState.MorphBall]).DirectionOnExitMorphing;
            transform.localScale = new Vector3(xScale, 1, 1);
        }
        
        public bool CheckWallGrab(){
            LayerMask layerMask = LayerMask.GetMask("Ground");

            Vector2 origin1 = WallGrabRaycast.position;
            Vector2 direction1 = Vector2.right * transform.localScale.x;
            float distance1 = WallGrabRaycastDistance;
            RaycastHit2D hit1 = Physics2D.Raycast(origin1, direction1, distance1, layerMask);
            if (hit1.collider != null){ return false; }

            Vector2 origin2 = origin1 + direction1 * distance1;
            Vector2 direction2 = Vector2.down;
            float distance2 = 0.3f;
            RaycastHit2D hit2 = Physics2D.Raycast(origin2, direction2, distance2, layerMask);
            return hit2.collider != null;
        }
        
        private IEnumerator ChangeStateCoroutine(PlayerState state){
            yield return _states[_currentState]?.Exit(this);
            LastState = _currentState;
            _currentState = state;
            yield return _states[_currentState].Enter(this);
            _changeStateCoroutine = null;
        }

        private IEnumerator AdjustShootingBlend(){
            Animator.SetFloat(ShootingBlendAnimationParameter, 1f);
            Animator.SetFloat(AimingBlendAnimationParameter, 1f);
            const int shootingFrames = 6;
            for (int i = 0; i < shootingFrames; i++){
                if (i == 1){ SpawnProjectile(); }
                yield return null;
            }
            Animator.SetFloat(ShootingBlendAnimationParameter, 0f);
        }

        private IEnumerator ShotCooldownCoroutine(){
            _canShoot = false;
            yield return new WaitForSeconds(ShotCooldown);
            _canShoot = true;
        }
    }

    public enum PlayerState{
        Idle,
        Running,
        Jumping,
        Crouching,
        MorphBall,
        Falling,
        WallGrab
    }
}