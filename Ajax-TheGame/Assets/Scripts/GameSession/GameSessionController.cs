using Core.Player.Controller;
using Core.Shared;
using Core.UI.LifeBar;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionController : MonoBehaviour
{
    private bool waiting;
    private Vector3 currentPoint;
    private EntranceID entranceTag;
    private bool searchCurrentPoint = false;
    private bool setUpLifes = false;
    private bool hasDied = false;

    public static GameSessionController Instance { get; private set; }
    public static bool loadSavedData = false;

    void Awake()
    {
        int numGameSessionControllers = FindObjectsOfType<GameSessionController>().Length;
        if (numGameSessionControllers > 1)
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
        waiting = !SceneManagementFunctions.CurrentSceneIsGameplay();
        if (waiting)
            return;

        if (loadSavedData)
            LoadSavedData();

        setUpLifes = true;

        PlacePlayer();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        waiting = !SceneManagementFunctions.CurrentSceneIsGameplay();
        if (waiting)
            return;

        setUpLifes = true;
        hasDied = false;

        if (searchCurrentPoint)
            SceneChangedSearchCurrentPoint();

        PlacePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
            return;

        if (setUpLifes)
        {
            var playerHealth = PlayerController.Instance.PlayerData.Health;
            LifeBarController.Instance.SetUpLifes(playerHealth.HP, playerHealth.MaxHP);
            setUpLifes = false;
        }

        if (PlayerController.Instance.PlayerData.Health.HP == 0)
        {
            ResetGameToSavePoint();
        }
    }

    public void SavePlayerCurrentPoint(Transform currentPointTransform)
    {
        if (hasDied)
            return;
        currentPoint = currentPointTransform.position;
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

    public Vector3 GetCurrentPoint()
    {
        return currentPoint;
    }

    public void SearchCurrentPoint(EntranceID entranceTag)
    {
        searchCurrentPoint = true;
        this.entranceTag = entranceTag;
    }

    private int LoadSavedData()
    {
        PlayerState playerState = SaveSystem.LoadPlayerState();

        var playerHealth = PlayerController.Instance.PlayerData.Health;
        playerHealth.HP = playerState.health;
        playerHealth.MaxHP = playerState.max_health;

        currentPoint = playerState.GetPosition();

        loadSavedData = false;

        return playerState.scene;
    }

    private void SceneChangedSearchCurrentPoint()
    {
        foreach (SceneEntrance se in FindObjectsOfType<SceneEntrance>())
        {
            if (se.gameObject.CompareTag(entranceTag.ToString()))
            {
                currentPoint = se.GetEntrancePoint();
                searchCurrentPoint = false;
                break;
            }
        }
    }

    private void PlacePlayer()
    {
        var player = PlayerController.Instance;
        player.transform.position = GetCurrentPoint();
    }
}
