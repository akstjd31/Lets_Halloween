using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button preparingButton;
    [SerializeField] private TextMeshProUGUI waveText, difficultyText;
    [SerializeField] private Transform life;
    [SerializeField] private Image heart, darkHeart;    // 하트 / 빈하트
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private GameObject preparingTimer;
    private List<Image> lifes;
    private Timer timer;

    private void Awake()
    {
        preparingButton?.onClick.AddListener(GameManager.Instance.OnClickReadyButton);
        timer = preparingTimer?.transform.GetChild(0).GetComponent<Timer>();
    }

    private void Start()
    {
        lifes = new List<Image>();
        
        if (GameManager.Instance != null)
        {
            difficultyText.text = $"[{GameManager.Instance.gameOptionData.difficulty}]";

            Unit unit = GameManager.Instance.gameOptionData.unit;
            moneyText.text = (unit as Player)?.Money.ToString();
            
            SetPreparingTimer();
        }
            
        UpdatePlayerLifeUI(3, 3);
        
        WaveManager.Instance.onWaveStarted += OnWaveStarted;
        WaveManager.Instance.onWaveEnded += OnWaveEnded;
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
        timer.timeRemaining = GameManager.Instance.elapsedTime;
        timer.minutes = (int)(timer.timeRemaining / 60);
        timer.seconds = (int)(timer.timeRemaining % 60);
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
            lifes[i].sprite = i < currentLife ? heart.sprite : darkHeart.sprite;
        }
    }

    private void OnWaveStarted()
    {
        Debug.Log("웨이브 시작");

        // 상점, 준비버튼 등 비활성화 작업
        preparingButton.gameObject.SetActive(false);
        preparingTimer.SetActive(false);
    }

    private void OnWaveEnded()
    {
        // 준비 버튼 활성화
        Debug.Log("웨이브 종료");

        SetPreparingTimer();
        preparingButton.gameObject.SetActive(true);
        preparingTimer.SetActive(true);
        waveText.text = $"Wave {WaveManager.Instance.GetWaveNumber()}";
    }
}
