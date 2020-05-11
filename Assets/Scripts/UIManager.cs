using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _lifeImage;
    [SerializeField] private Sprite[] _lifeSprites;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartLevelText;

    [SerializeField] private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The game manager is NULL!");
        }
    }

    public void UpdateScore(int points)
    {
        _scoreText.text = "Score: " + points;
    }

    public void UpdateLives(int currentLives)
    {
        _lifeImage.sprite = _lifeSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(gameOverFlickerRoutine());
    }


    IEnumerator gameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
