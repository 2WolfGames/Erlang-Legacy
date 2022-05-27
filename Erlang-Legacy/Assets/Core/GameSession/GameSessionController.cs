using Core.Player.Controller;
using Core.Shared;
using Core.Shared.Enum;
using Core.Shared.SaveSystem;
using Core.UI;
using Core.UI.LifeBar;
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
        public bool LoadData {
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

            SetUpPlayerLifes();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //pre: --
        //post: seting up player lifes and search current player position if necessary
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (waiting)
                return;

            inDieProcess = false;

            if (entranceTag != EntranceID.None)
                SearchEntrance();
            else
                PlacePlayer();

            SetUpPlayerLifes();
        }

        //pre: if not waiting, player.instance != null
        //post: if player has died game resets to last save
        private void Update()
        {
            if (waiting)
                return;

            var playerCurrentHealth = PlayerController.Instance.PlayerData.Health.HP;
            if (!inDieProcess & playerCurrentHealth == 0)
            {
                inDieProcess = true;
                ResetGameToLastSave();
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
                                                    savePoint.position);
            SaveSystem.SavePlayerState(playerState);
            currentSavePos = savePoint.position;
        }

        //pre: player.instance != null
        //post: returns player to it's status of the last save
        public void ResetGameToLastSave()
        {
            PlayerController.Instance.OnDie();

            FindObjectOfType<InGameCanvas>()?.ActiveDeathImage();
            StartCoroutine(Loader.LoadWithDelay((SceneID)LoadSavedData(), 5f));
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

        //pre: player.instance != null && lifebarcontroller.instance != null
        //post: lifebar contains current player lifes 
        private void SetUpPlayerLifes()
        {
            var playerHealth = PlayerController.Instance.PlayerData.Health;
            LifeBarController.Instance.SetUpLifes(playerHealth.HP, playerHealth.MaxHP);
        }
    }
}
