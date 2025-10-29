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
    
    public void UpdatePlayerLifeUI(int life)
    {
        lifeText.text = $"Life: {life}";
    }

    private void OnWaveStarted()
    {
        Debug.Log("웨이브 시작");
        waveText.text = $"Wave {WaveManager.Instance.GetWaveNumber()}";
        // 상점, 준비버튼 등 비활성화 작업
    }

    private void OnWaveEnded()
    {
        Debug.Log("웨이브 종료");
    }
}
