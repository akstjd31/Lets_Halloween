using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button preparingButton;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI lifeText;

    private void Start()
    {
        preparingButton.onClick.AddListener(GameManager.Instance.OnClickReadyButton);

        WaveManager.Instance.onWaveStarted += OnWaveStarted;
        WaveManager.Instance.onWaveEnded += OnWaveEnded;
    }
    
    // 구독 해제
    private void OnDestroy()
    {
        WaveManager.Instance.onWaveStarted -= OnWaveStarted;
        WaveManager.Instance.onWaveEnded -= OnWaveEnded;
    }

    public void UpdatePlayerLifeUI(int life)
    {
        lifeText.text = $"Life: {life}";
    }

    private void OnWaveStarted()
    {
        Debug.Log("웨이브 시작");

        // 상점, 준비버튼 등 비활성화 작업
        preparingButton.gameObject.SetActive(false);
    }

    private void OnWaveEnded()
    {
        // 준비 버튼 활성화
        Debug.Log("웨이브 종료");
        preparingButton.gameObject.SetActive(true);
        waveText.text = $"Wave {WaveManager.Instance.GetWaveNumber()}";
    }
}
