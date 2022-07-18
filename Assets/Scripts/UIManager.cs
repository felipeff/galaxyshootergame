using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private TextMeshProUGUI _gameOverText;
    [SerializeField]
    private Text _restartGameText;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private GameObject _pausePanel;

    private GameManager _gameManager;

    private int _bestScore = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";

        _gameOverText.enabled = false;
        _restartGameText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }

        _bestScore = PlayerPrefs.GetInt("BestScore", 0);
        _bestScoreText.text = "Best Score: " + _bestScore.ToString();

    }


    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
        CheckForBestScore(score);
    }

    public void CheckForBestScore(int score)
    {
        if(score > _bestScore)
        {
            _bestScore = score;
            _bestScoreText.text = "Best Score: " + _bestScore.ToString();
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.enabled = true;
        _restartGameText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void ResumePlay()
    {
        _gameManager.ResumeGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

}
