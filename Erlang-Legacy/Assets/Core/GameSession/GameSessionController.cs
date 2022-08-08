using System.Collections.Generic;
using Core.Player;
using Core.Player.Controller;
using Core.Shared;
using Core.Shared.Enum;
using Core.Shared.SaveSystem;
using Core.UI;
using UnityEngine;
using Core.UI.Notifications;
using UnityEngine.SceneManagement;

namespace Core.GameSession
{
    public class GameSessionController : MonoBehaviour
    {
        [SerializeField] Sprite gameSavedSprite;
        private bool loadData = false;
        private bool nonPlayableScene => !SceneManagementFunctions.CurrentSceneIsGameplay();
        public Vector3 currentSavePos { get; private set; }
        private EntranceID entranceTag;
        private bool inDieProcess = false;
        public bool LoadData
        {
            get => loadData;
            set
            {
                loadData = value;
            }
        }
        public static GameSessionController Instance { get; private set; }

        //pre: --
        //post: if these is no gamesessioncontroller this becomes the one
        //      else it destroys itself
        private void Awake()
        {
            if (GameSessionController.Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
            }
        }

        //pre: --
        //post: seting up player lifes and charges player if it's necessary
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (nonPlayableScene)
                return;

            if (loadData)
            {
                LoadSavedData();
                PlacePlayer();
                PowersPanelManager.Instance.ManagePowersVisibility();
            }
        }

        //pre: --
        //post: seting up player lifes and search current player position if necessary
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (nonPlayableScene)
                return;

            inDieProcess = false;

            if (loadData) //player has died 
            {
                LoadSavedData();
                PlacePlayer();
            }
            else if (entranceTag != EntranceID.None) //player came from other scene
            {
                SearchEntrance();
            }

            PowersPanelManager.Instance.ManagePowersVisibility();
        }

        //pre: if not nonPlayableScene, player.instance != null
        //post: if player has died game resets to last save
        private void Update()
        {
            if (nonPlayableScene)
                return;
            bool isDead = PlayerController.Instance.IsDead();
            if (!inDieProcess && isDead)
            {
                inDieProcess = true;
                RecoverLastSaveScene();
            }
        }

        //pre: currentPointTransform != null
        //post: currantSavePos is currentPointTransform position
        public void SavePlayerCurrentPoint(Transform currentPointTransform)
        {
            if (inDieProcess)
                return;
            currentSavePos = currentPointTransform.position;
        }

        //pre: save point != null && player.instance != null
        //post: saves current stats of player 
        //      and currantSavePos is savePoint position
        public void SavePlayerState(Transform savePoint)
        {
            if (inDieProcess)
                return;

            PlayerState playerState = new PlayerState(((int)SceneManagementFunctions.GetCurrentSceneEnum()),
                                                    PlayerController.Instance.PlayerData.Health.HP,
                                                    PlayerController.Instance.PlayerData.Health.MaxHP,
                                                    savePoint.position,
                                                    PlayerAbilitiesAdquiredSnapshot());
            SaveSystem.SavePlayerState(playerState);
            currentSavePos = savePoint.position;

            NotificationManager.Instance.PostNotificationWithDelay("Game Saved", "Keep going little hero!", gameSavedSprite, 2f);
        }

        private Dictionary<Ability, bool> PlayerAbilitiesAdquiredSnapshot()
        {
            AbilityController abilitiesController = PlayerController.Instance?.GetComponent<AbilityController>();
            AbilitiesAcquired adquiredAbilities = abilitiesController?.abilitiesAcquired;
            Dictionary<Ability, bool> abilitiesState = new Dictionary<Ability, bool>{
                {Ability.Dash, adquiredAbilities.Acquired(Ability.Dash)},
                {Ability.Ray, adquiredAbilities.Acquired(Ability.Ray)},
            };
            return abilitiesState;
        }

        //pre: player.instance != null
        //post: returns player to it's status of the last save
        public void RecoverLastSaveScene()
        {
            FindObjectOfType<InGameCanvas>()?.ActiveDeathImage();

            loadData = true;

            PlayerState playerState = SaveSystem.LoadPlayerState();

            StartCoroutine(Loader.LoadWithDelay((SceneID)playerState.scene, 5f));
        }

        //pre: entranceTag != entranceID.None
        //post: entranceTang is assigned, in next scene player is goint to apear at 
        //        SceneEntrance.EntrancePoint of this tag
        public void NextSceneEntrance(EntranceID entranceTag)
        {
            this.entranceTag = entranceTag;
        }

        //pre: there is saved data && player.instance != null
        //post: player stats are the ones saved in data
        private int LoadSavedData()
        {
            PlayerState playerState = SaveSystem.LoadPlayerState();

            var playerHealth = PlayerController.Instance.PlayerData.Health;
            playerHealth.HP = playerState.max_health;
            playerHealth.MaxHP = playerState.max_health;

            LoadAbilitiesAdcquired(playerState);

            currentSavePos = playerState.GetPosition();
            loadData = false;

            return playerState.scene;
        }

        private void LoadAbilitiesAdcquired(PlayerState playerState)
        {
            bool dashAcquired = false;
            bool rayAcquired = false;

            Dictionary<Ability, bool> mem_abilitiesAdquired = playerState.abilitiesAdquired;
            AbilityController abilityController = PlayerController.Instance.GetComponent<AbilityController>();

            mem_abilitiesAdquired.TryGetValue(Ability.Dash, out dashAcquired);
            mem_abilitiesAdquired.TryGetValue(Ability.Ray, out rayAcquired);

            AbilitiesAcquired adquiredAbilities = abilityController.abilitiesAcquired;
            adquiredAbilities.DashAcquired = dashAcquired;
            adquiredAbilities.RayAcquired = rayAcquired;
        }

        //pre: entranceTag is not EntranceID.None
        //post: searches the SceneEntrance with tag equal to entranceTag 
        //      to start entrance proces 
        private void SearchEntrance()
        {
            SceneEntrance[] lstSceneEntrance = FindObjectsOfType<SceneEntrance>();
            int i = 0;

            while (i < lstSceneEntrance.Length && entranceTag != EntranceID.None)
            {
                SceneEntrance se = lstSceneEntrance[i];
                if (se.gameObject.CompareTag(entranceTag.ToString()))
                {
                    se.MakeEntrance();
                    currentSavePos = se.GetEntrancePoint();
                    entranceTag = EntranceID.None;
                }
                i++;
            }

            if (entranceTag != EntranceID.None)
            {
                Debug.LogError("GameSessionController.SearchEntrance: " +
                                "Entrance not found. Entrance tag: " + entranceTag.ToString());
                entranceTag = EntranceID.None;
            }
        }

        //pre: player.instance != null
        //post: player position = currentSavePosition
        public void PlacePlayer()
        {
            var player = PlayerController.Instance;
            player.transform.position = currentSavePos;
        }

    }
}
