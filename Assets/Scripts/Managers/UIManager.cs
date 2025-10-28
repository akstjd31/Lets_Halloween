using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button preparingButton;
    [SerializeField] private TextMeshProUGUI waveText;
    int wave;

    private void Start()
    {
        wave = 1;
        preparingButton.onClick.AddListener(GameManager.Instance.OnClickReadyButton);
        
        WaveManager.Instance.onWaveStarted += OnWaveStarted;
        WaveManager.Instance.onWaveEnded += OnWaveEnded;
    }

    private void OnWaveStarted()
    {
        Debug.Log("웨이브 시작");
        waveText.text = $"Wave {wave++}";
        // 상점, 준비버튼 등 비활성화 작업
    }

    private void OnWaveEnded()
    {
        Debug.Log("웨이브 종료");
    }

    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.onWaveStarted -= OnWaveStarted;
            WaveManager.Instance.onWaveEnded -= OnWaveEnded;
        }
    }
}
