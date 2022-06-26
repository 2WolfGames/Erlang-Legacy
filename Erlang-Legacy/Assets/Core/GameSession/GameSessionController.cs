using System.Collections.Generic;
using Core.Player;
using Core.Player.Controller;
using Core.Shared;
using Core.Shared.Enum;
using Core.Shared.SaveSystem;
using Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.GameSession
{
    public class GameSessionController : MonoBehaviour
    {
        private bool loadData = false;
        private bool waiting => !SceneManagementFunctions.CurrentSceneIsGameplay();
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
            if (waiting)
                return;

            if (loadData)
            {
                LoadSavedData();
                PlacePlayer();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //pre: --
        //post: seting up player lifes and search current player position if necessary
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (waiting)
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


            Debug.Log("GameSessionController.OnSceneLoaded()");

        }

        //pre: if not waiting, player.instance != null
        //post: if player has died game resets to last save
        private void Update()
        {
            if (waiting)
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
        }

        private Dictionary<Ability, bool> PlayerAbilitiesAdquiredSnapshot()
        {
            AbilityController abilitiesController = PlayerController.Instance?.GetComponent<AbilityController>();
            AdquiredAbilities adquiredAbilities = abilitiesController?.adquiredAbilities;
            Dictionary<Ability, bool> abilitiesState = new Dictionary<Ability, bool>{
                {Ability.Dash, adquiredAbilities.Adquired(Ability.Dash)},
                {Ability.Ray, adquiredAbilities.Adquired(Ability.Ray)},
            };
            return abilitiesState;
        }

        //pre: player.instance != null
        //post: returns player to it's status of the last save
        public void RecoverLastSaveScene()
        {
            Debug.Log("GameSessionController.RecoverLastSaveScene()");
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
            Debug.Log("Loading data");
            PlayerState playerState = SaveSystem.LoadPlayerState();

            var playerHealth = PlayerController.Instance.PlayerData.Health;
            playerHealth.HP = playerState.health;
            playerHealth.MaxHP = playerState.max_health;

            currentSavePos = playerState.GetPosition();
            loadData = false;

            return playerState.scene;
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
        private void PlacePlayer()
        {
            var player = PlayerController.Instance;
            player.transform.position = currentSavePos;
        }

    }
}
