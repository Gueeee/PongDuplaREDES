using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameplayUI : MonoBehaviour {
    [Header("Placar")]
    public TextMeshProUGUI scoreTextP1;
    public TextMeshProUGUI scoreTextP2;

    [Header("Tela de Vitória")]
    public GameObject winPanel;
    public TextMeshProUGUI winText;

    void Start() {
        if (winPanel != null) winPanel.SetActive(false);

        if (GameManager.Instance != null) {
            GameManager.Instance.OnScoreChanged += UpdateScoreUI;
            GameManager.Instance.winningTeam.OnValueChanged += OnWinnerChanged;

            UpdateScoreUI(GameManager.Instance.scorePlayer1.Value, GameManager.Instance.scorePlayer2.Value);
            
            if (GameManager.Instance.winningTeam.Value != 0) {
                ShowWinnerScreen(GameManager.Instance.winningTeam.Value);
            }
        }
    }

    void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnScoreChanged -= UpdateScoreUI;
            GameManager.Instance.winningTeam.OnValueChanged -= OnWinnerChanged;
        }
    }

    private void UpdateScoreUI(int p1Score, int p2Score) {
        scoreTextP1.text = p1Score.ToString();
        scoreTextP2.text = p2Score.ToString();
    }

    private void OnWinnerChanged(int previous, int current) {
        if (current != 0) {
            ShowWinnerScreen(current);
        }
    }

    private void ShowWinnerScreen(int winningTeam) {
        if (winPanel != null) winPanel.SetActive(true);

        if (winText != null) {
            if (winningTeam == 1) {
                winText.text = "TIME 1 VENCEU!";
            } else if (winningTeam == 2) {
                winText.text = "TIME 2 VENCEU!";
            }
        }
    }

    public void BackToMenu() {
        if (NetworkManager.Singleton != null) {
            NetworkManager.Singleton.Shutdown();
        }

        if (GameManager.Instance != null) {
            Destroy(GameManager.Instance.gameObject);
        }

        GameManager.Instance?.GoToScene("MainMenu"); 
    }
}