using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Image _livesImage;

    // 0 = 3 lives, 1 = 2 lives, 2 = 1 live, 3 = no live
    [SerializeField] private Sprite[] _liveSprites;
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: 0";
        _livesImage.sprite = _liveSprites[3];
        
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int newScore)
    {
        _scoreText.text = $"Score: {newScore}";
    }

    public void UpdateLives(int lives)
    {
        _livesImage.sprite = _liveSprites[lives];

        if (lives == 0)
        {
            DisplayMessageOnPlayerDead();
        }
    }

    private void DisplayMessageOnPlayerDead()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _restartText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _restartText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
