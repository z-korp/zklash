using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerInfoUI : MonoBehaviour
{
    public TextMeshProUGUI txtLife;
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtTrophy;

    private float currentLife;
    private float currentGold;
    private float currentTrophy;

    private uint targetLife;
    private uint targetGold;
    private uint targetTrophy;

    private float lerpDuration = 0.2f; // Duration of the lerping effect in seconds
    private Coroutine lerpCoroutine;

    public static PlayerInfoUI instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerInfo found!");
            return;
        }
        instance = this;
    }

    void Start()
    {
        currentLife = 0;
        currentGold = 0;
        currentTrophy = 0;

        EventManager.OnRefreshPlayerStats += RefreshPlayerStats;
    }

    void OnDestroy()
    {
        EventManager.OnRefreshPlayerStats -= RefreshPlayerStats;
    }

    void Update()
    {
        RefreshPlayerStats();
    }

    public void RefreshPlayerStats()
    {
        if (GameManager.Instance != null)
        {
            var teamEntity = PlayerData.Instance.teamEntity;
            if (teamEntity != null)
            {
                var team = GameManager.Instance.worldManager.Entity(teamEntity).GetComponent<Team>();
                if (team != null)
                {
                    UpdatePlayerStats(team.health, team.gold, team.level);
                }
            }
        }
    }

    public void UpdatePlayerStats(uint lives, uint gold, uint trophies)
    {
        targetLife = lives;
        targetGold = gold;
        targetTrophy = trophies - 1;

        if (lerpCoroutine != null)
        {
            StopCoroutine(lerpCoroutine);
        }
        lerpCoroutine = StartCoroutine(LerpPlayerStats());
    }

    private IEnumerator LerpPlayerStats()
    {
        float elapsedTime = 0f;

        float startLife = currentLife;
        float startGold = currentGold;
        float startTrophy = currentTrophy;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            currentLife = Mathf.Lerp(startLife, targetLife, t);
            currentGold = Mathf.Lerp(startGold, targetGold, t);
            currentTrophy = Mathf.Lerp(startTrophy, targetTrophy, t);

            UpdateUI();

            yield return null;
        }

        // Ensure the final values are set accurately
        currentLife = targetLife;
        currentGold = targetGold;
        currentTrophy = targetTrophy;

        UpdateUI(true); // Force update to the exact target values
        lerpCoroutine = null;
    }

    // Method to update the UI with the current values
    void UpdateUI(bool useTargetValues = false)
    {
        if (useTargetValues)
        {
            txtLife.text = targetLife.ToString();
            txtGold.text = targetGold.ToString();
            txtTrophy.text = targetTrophy.ToString();
        }
        else
        {
            txtLife.text = Mathf.RoundToInt(currentLife).ToString();
            txtGold.text = Mathf.RoundToInt(currentGold).ToString();
            txtTrophy.text = Mathf.RoundToInt(currentTrophy).ToString();
        }
    }

    // Examples of methods to modify the values, to be called from other scripts or events
    public void AddLife(uint amount)
    {
        UpdatePlayerStats((uint)currentLife + amount, (uint)currentGold, (uint)currentTrophy);
    }

    public void AddGold(uint amount)
    {
        UpdatePlayerStats((uint)currentLife, (uint)currentGold + amount, (uint)currentTrophy);
    }

    public void AddTrophy(uint amount)
    {
        UpdatePlayerStats((uint)currentLife, (uint)currentGold, (uint)currentTrophy + amount);
    }

    public uint getLifes()
    {
        return (uint)currentLife;
    }

    public uint getTrophies()
    {
        return (uint)currentTrophy;
    }
}