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
        public static bool loadSavedData = false;
        private bool waiting => !SceneManagementFunctions.CurrentSceneIsGameplay();
        public Vector3 currentSavePos { get; private set; }
        private EntranceID entranceTag;
        private bool hasDied = false;

        public static GameSessionController Instance { get; private set; }

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

        private void Start()
        {
            if (waiting)
                return;

            if (loadSavedData)
            {
                LoadSavedData();
                PlacePlayer();
            }

            SetUpPlayerLifes();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (waiting)
                return;

            hasDied = false;

            if (entranceTag != EntranceID.None)
                SearchEntrance();
            else
                PlacePlayer();

            SetUpPlayerLifes();
        }

        private void Update()
        {
            if (waiting)
                return;

            var playerCurrentHealth = PlayerController.Instance.PlayerData.Health.HP;
            if (!hasDied & playerCurrentHealth == 0)
            {
                ResetGameToSavePoint();
            }
        }

        public void SavePlayerCurrentPoint(Transform currentPointTransform)
        {
            if (hasDied)
                return;
            currentSavePos = currentPointTransform.position;
        }

        public void SavePlayerState(Transform savePoint)
        {
            PlayerState playerState = new PlayerState(((int)SceneManagementFunctions.GetCurrentSceneEnum()),
                                                    PlayerController.Instance.PlayerData.Health.HP,
                                                    PlayerController.Instance.PlayerData.Health.MaxHP,
                                                    savePoint.position);
            SaveSystem.SavePlayerState(playerState);
        }

        public void ResetGameToSavePoint()
        {
            hasDied = true;
            PlayerController.Instance.Controllable = false;

            FindObjectOfType<InGameCanvas>()?.ActiveDeathImage();
            StartCoroutine(Loader.LoadWithDelay((SceneID)LoadSavedData(), 4));
        }

        public void NextSceneEntrance(EntranceID entranceTag)
        {
            this.entranceTag = entranceTag;
        }

        private int LoadSavedData()
        {
            PlayerState playerState = SaveSystem.LoadPlayerState();

            var playerHealth = PlayerController.Instance.PlayerData.Health;
            playerHealth.HP = playerState.health;
            playerHealth.MaxHP = playerState.max_health;

            currentSavePos = playerState.GetPosition();
            loadSavedData = false;

            return playerState.scene;
        }

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
                Debug.LogError("Entrance not found. Entrance tag: " + entranceTag.ToString());
                entranceTag = EntranceID.None;
            }
        }

        private void PlacePlayer()
        {
            var player = PlayerController.Instance;
            player.transform.position = currentSavePos;
        }

        private void SetUpPlayerLifes()
        {
            var playerHealth = PlayerController.Instance.PlayerData.Health;
            LifeBarController.Instance.SetUpLifes(playerHealth.HP, playerHealth.MaxHP);
        }
    }
}
