using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using AssetKits.ParticleImage;

public class UIManager : MonoBehaviour
{
    [Header("TopBarUI")]
    [SerializeField] private GameObject topBarUI;
    private TextMeshProUGUI waveText, difficultyText;

    [Header("MiddleBarUI")]
    [SerializeField] private GameObject MiddleBarUI;
    private Text moneyText;
    private Transform life;
    private ParticleImage coinAttractionParticle;
    private Timer preparingTimer;
    private List<Image> lifes;

    [Header("BottomBarUI")]
    [SerializeField] private GameObject BottomBarUI;
    private Button preparingButton;

    [Header("ShopUI")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Button shopButton;
    
    
    [Header("Prefab")]
    [SerializeField] private Image heart, heartDark;    // 하트 / 빈하트
    
    private void Awake()
    {
        // TopBar UI
        TextMeshProUGUI[] texts = topBarUI?.GetComponentsInChildren<TextMeshProUGUI>();
        if (texts != null)
        {
            waveText = texts[0];
            difficultyText = texts[1];
        }

        // MiddleBar UI
        moneyText = MiddleBarUI?.GetComponentInChildren<Text>();
        life = MiddleBarUI?.GetComponentInChildren<GridLayoutGroup>().transform;
        coinAttractionParticle = FindFirstObjectByType<ParticleImage>();

        // BottomBar UI
        preparingButton = BottomBarUI?.GetComponentInChildren<Button>();
        preparingButton?.onClick.AddListener(GameManager.Instance.OnClickReadyButton);
        preparingTimer = FindFirstObjectByType<Timer>();

        // Shop UI
        shopButton = shopUI?.GetComponentInChildren<Button>();
    }

    private void Start()
    {
        lifes = new List<Image>();

        if (GameManager.Instance != null)
        {
            difficultyText.text = $"[{GameManager.Instance.gameOptionData.difficulty}]";

            Unit unit = GameManager.Instance.gameOptionData.unit;
            moneyText.text = (unit as Player)?.Money.ToString("N0");

            SetPreparingTimer();
        }

        if (GameManager.Instance.gameOptionData.factionType.Equals(FactionType.Player))
            UpdatePlayerLifeUI(3, 3);
        
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.onWaveStarted += OnWaveStarted;
            WaveManager.Instance.onWaveEnded += OnWaveEnded;
        }
    }

    // 구독 해제
    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.onWaveStarted -= OnWaveStarted;
            WaveManager.Instance.onWaveEnded -= OnWaveEnded;
        }
    }

    private void SetPreparingTimer()
    {
        if (preparingTimer != null)
        {
            preparingTimer.timeRemaining = GameManager.Instance.elapsedTime;
            preparingTimer.minutes = (int)(preparingTimer.timeRemaining / 60);
            preparingTimer.seconds = (int)(preparingTimer.timeRemaining % 60);
            preparingTimer.transform.parent.gameObject.SetActive(false);
        }
    }

    // 라이프에 따른 하트 활성화/비활성화
    public void UpdatePlayerLifeUI(int maxLife, int currentLife)
    {
        if (maxLife < 1 || currentLife < 0)
            return;

        // 만약 처음이면 새로 만든다.
        if (!lifes.Any())
        {
            for (int i = 0; i < maxLife; i++)
            {
                Image heartPrefab = Instantiate(heart, Vector3.zero, Quaternion.identity, life);
                lifes.Add(heartPrefab);
            }

            return;
        }

        // 생성이 되어있다면 스프라이트만 변경
        for (int i = 0; i < maxLife; i++)
        {
            lifes[i].sprite = i < currentLife ? heart.sprite : heartDark.sprite;
        }
    }

    private void OnWaveStarted()
    {
        Debug.Log("웨이브 시작");
        UIActiveSetting(false);
    }

    private void OnWaveEnded()
    {
        // 준비 버튼 활성화
        Debug.Log("웨이브 종료");

        // 텍스트 업데이트
        Unit unit = GameManager.Instance.gameOptionData.unit;
        coinAttractionParticle.Play();

        SetPreparingTimer();
        UIActiveSetting(true);
        waveText.text = $"Wave {WaveManager.Instance.GetWaveNumber()}";

        if (unit is Player player)
        {
            moneyText.text = player?.Money.ToString("N0");
        }
    }
    
    // 준비, 전투 단계에 따른 UI 액티브 작업
    private void UIActiveSetting(bool active)
    {
        preparingButton.gameObject.SetActive(active);
        //preparingTimer.transform.parent.gameObject.SetActive(active);
        shopButton.transform.parent.gameObject.SetActive(active);
    }
}
