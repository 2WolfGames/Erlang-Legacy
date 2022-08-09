using System.Collections;
using Core.Player.Data;
using Core.Player.Utility;
using Core.Shared;
using Core.Shared.Enum;
using Core.Utility;
using Core.Manager;
using UnityEngine;
using Core.UI;
using Core.UI.Notifications;
using Core.ScriptableEffect;

namespace Core.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public bool shakeCameraOnHurt = true;
        public Sprite newLifeSprite;
        public bool inRecoverProcess = false;
        public PlayerData playerData;
        public bool CanBeHit => protectable.CanBeHit;
        public bool IsProtected => !CanBeHit;
        public int FacingValue => facingController.FacingToInt;
        public PlayerData PlayerData { get => playerData; private set => playerData = value; }
        public bool IsGrounded => movementController.IsGrounded;
        public ParticleSystem healEffectParticle;
        public Stats Stats => playerData.Stats;
        [SerializeField] SFX soundEffects;
        [SerializeField] VolumeSettings volumeSettings;
        public bool BlockingUI
        {
            get => blockingUI;
            set
            {
                blockingUI = value;
                if (blockingUI)
                    OnBlockUI();
                else OnUnblockUI();
            }
        }

        public bool Controllable
        {
            get => controllable && !BlockingUI;
            set => controllable = value;
        }
        private AudioSource playerAudioSource;
        public Collider2D BodyCollider => GetComponent<Collider2D>();
        public Rigidbody2D Body => GetComponent<Rigidbody2D>();
        public Animator Animator => GetComponentInChildren<Animator>();
        public static PlayerController Instance { get; private set; }
        public bool Punching => abilityController.Punching;
        private bool controllable = true;
        private AbilityController abilityController => GetComponent<AbilityController>();
        private MovementController movementController => GetComponent<MovementController>();
        private FacingController facingController => GetComponent<FacingController>();
        private Protectable protectable => GetComponent<Protectable>();
        private bool isDashing => movementController.IsDashing;
        private float recoverTimeoutAfterHit => playerData.Stats.recoverTimeoutAfterHit;
        private bool blockingUI;
        private float baseGravityScale = 1f;
        private int currentHealth => playerData.Health.HP;

        protected void Awake()
        {
            var matches = FindObjectsOfType<PlayerController>();

            if (matches.Length > 1)
                Destroy(gameObject);
            else Instance = this;

            playerAudioSource = GetComponentInChildren<AudioSource>();

            baseGravityScale = Body.gravityScale;
            InitControllers();
        }

        private void InitControllers()
        {
            InitAbilityController();
            InitMovementController();
        }

        private void InitAbilityController()
        {
            abilityController.OnPunchStart += () =>
                PlayRandomSound(soundEffects.punch.clips, soundEffects.punch.volume);
            abilityController.OnRayStart += () =>
                PlayRandomSound(soundEffects.ray.clips, soundEffects.ray.volume);
        }

        private void InitMovementController()
        {
            movementController.OnDashStart += OnDashStart;
            movementController.OnJumpStart += () =>
                PlayRandomSound(soundEffects.jump.clips, soundEffects.jump.volume);
        }

        private void Start()
        {
            if (HasNoLifes()) Die();
        }

        public void OnDashComplete()
        {
            movementController.StopDashing();
            abilityController.OnDashComplete();

            controllable = true;

            if (!inRecoverProcess)
                protectable.SetProtection(ProtectionType.NONE);
        }

        // post: enables scripts that make damage
        //      and sets infinite protection
        private void OnDashStart()
        {
            controllable = false;
            protectable.SetProtection(ProtectionType.INFINITE);
            abilityController.OnDashStart();
            PlayRandomSound(soundEffects.dash.clips, soundEffects.dash.volume);
        }

        // pre: called by some function that stunds player (called by hit animation)
        // post: enable scripts & returns normal game constants
        private void OnUnblockUI()
        {
            //Debug.Log("freeze game play");
            // reset global game constant to normal state if needed...
        }

        // pre: (called by hit end animation)
        // post: detach component logic scripts & freeze player
        // PROP: instead of freezing player we can make time slow 
        public void OnBlockUI()
        {
            //Debug.Log("unfreeze game play");
        }

        public void Heal()
        {
            PlayRandomSound(soundEffects.heal.clips, soundEffects.heal.volume);
            if (playerData.Health.HP < playerData.Health.MaxHP)
            {
                playerData.Health.HP += 1;
            }
            healEffectParticle?.Play();
        }

        public void Heal(int hp)
        {
            PlayRandomSound(soundEffects.heal.clips, soundEffects.heal.volume);
            if (playerData.Health.HP < playerData.Health.MaxHP)
            {
                playerData.Health.HP += hp;
            }
            healEffectParticle?.Play();
        }

        // pre: --
        // post: applies damage to player. 1 unit of damage represent 1 unit of life taken
        public void Hurt(int damage, GameObject other)
        {
            if (!CanBeHit || IsDead())
                return;

            ShakeCamera();
            Freeze();
            ResetAbilities();
            GameManager.Instance?.FreezeTime(0.01f);
            TakeDamage(other.transform, damage);
        }

        private void TakeDamage(Transform agressor, int damage)
        {
            SetHealth(currentHealth - damage);
            if (HasNoLifes())
                Die();
            else
                RecoverFromTakingDamage(agressor);
        }

        private void RecoverFromTakingDamage(Transform agressor)
        {
            PlayRandomSound(soundEffects.hurt.clips, soundEffects.hurt.volume);
            ComputeSideHurtAnimation(agressor);
            OnRecoverStart();
        }

        private void PlayRandomSound(AudioClip[] sounds, float intensity)
        {
            SoundManager soundManager = SoundManager.Instance;
            if (soundManager == null)
            {
                Debug.LogWarning("SoundManager is not present in scene");
                return;
            }
            float volume = (volumeSettings ? volumeSettings.SoundVolume : 1)
                            * intensity;
            soundManager.PlayRandomSound(sounds, volume, playerAudioSource);
        }

        private void ShakeCamera()
        {
            if (shakeCameraOnHurt)
                CameraManager.Instance?.ShakeCamera();
        }

        private void ResetAbilities()
        {
            abilityController.PunchEnd();
            OnDashComplete();
        }

        private void SetHealth(int health)
        {
            playerData.Health.HP = health;
        }

        private void ComputeSideHurtAnimation(Transform other)
        {
            Side side = Function.RelativeCollisionSide(transform, other.transform);

            if (side == Side.Back)
                Animator.SetTrigger(CharacterAnimations.BackHurt);
            else Animator.SetTrigger(CharacterAnimations.FrontHurt);
        }

        // pre: called at first key frame hit (backward & forward) animations       
        private void OnRecoverStart()
        {
            Animator.SetBool(CharacterAnimations.Blink, true);
            inRecoverProcess = true;
            protectable.SetProtection(ProtectionType.INFINITE);
            controllable = false;
        }

        // desc: to be called at end of recover animations with and event
        public void OnRecoverComplete()
        {
            if (!inRecoverProcess)
                return;

            controllable = true;
            StartCoroutine(AfterRecoverComplete());
        }

        // pre:  after hurt animations & recover process running
        // post: trigger animations & and resets protection after few seconds
        //       if no dashing process is running, set character unprotected
        private IEnumerator AfterRecoverComplete()
        {
            yield return new WaitForSeconds(recoverTimeoutAfterHit);

            inRecoverProcess = false;

            if (!isDashing) // respects dashing protection
                protectable.SetProtection(ProtectionType.NONE);

            Animator.SetBool(CharacterAnimations.Blink, false);
        }

        //pre: --
        //post: faces player = Face
        public void SetFacing(Face Face)
        {
            facingController.SetFacing(Face);
            movementController.FaceDirection();
        }

        public void Die()
        {
            PlayRandomSound(soundEffects.death.clips, soundEffects.death.volume);
            controllable = false;
            Freeze();
            Animator.SetTrigger(CharacterAnimations.Die);
            StartCoroutine(BlockMovement(1f));
        }

        private IEnumerator BlockMovement(float afterTimeout)
        {
            yield return new WaitForSeconds(afterTimeout);
            Body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        public void Freeze()
        {
            Body.velocity = Vector2.zero;
        }

        public void ZeroGravity()
        {
            Body.gravityScale = 0;
        }

        public void BaseGravity()
        {
            Body.gravityScale = baseGravityScale;
        }

        public bool IsAlive()
        {
            return playerData.Health.HP > 0;
        }

        public bool IsDead()
        {
            return !IsAlive();
        }

        public bool HasNoLifes()
        {
            return IsDead();
        }

        public void AdquireAbility(Ability ability)
        {
            if (!AdquiredAbility(ability))
            {
                abilityController.ActiveAbility(ability);
                PowersPanelManager.Instance?.ManagePowersVisibility();
            }
        }

        public bool AdquiredAbility(Ability ability)
        {
            return abilityController.AdquiredAbility(ability);
        }

        //pre: --
        //post: Player max lifes increase
        public void IncreaseMaxLifes()
        {
            playerData.Health.MaxHP += 1;
            playerData.Health.HP = playerData.Health.MaxHP;
            NotificationManager.Instance?.PostNotification("New Life", "Your lifes increased", newLifeSprite);
        }
    }
}

