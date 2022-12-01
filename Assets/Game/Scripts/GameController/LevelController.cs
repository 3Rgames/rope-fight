using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using NaughtyAttributes;

public class LevelController : MonoSingleton<LevelController>
{
    [MinValue(0), MaxValue(120)]
    [SerializeField] private int _frameRate;
    private int allLevels;
    private int lvlIndex;
    [HideInInspector] public int CountOfLoops;

    private void Start()
    {
        Application.targetFrameRate = _frameRate;

        allLevels = SceneManager.sceneCountInBuildSettings;
        lvlIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (PlayerPrefs.HasKey("Level"))
        {
            lvlIndex = PlayerPrefs.GetInt("Level");
            PlayerPrefs.DeleteKey("Level");
        }

        LoadScene(false);
    }

    public void LevelReload()
    {
        RestartCurrentLevel();
    }

    public void LevelLose()
    {
        RestartCurrentLevel();
    }

    private void RestartCurrentLevel()
    {
        LoadScene(true);
        UIController.Instance.HideLastPanels();
        EventController.Instance.OnTutorial.DoEvent(true);
    }

    public void NextLevel()
    {
        lvlIndex++;
        if (lvlIndex >= allLevels)
        {
            lvlIndex = 1;
            CountOfLoops++;
        }
        SLS.Instance.Save();

        LoadScene(true);

        UIController.Instance.HideLastPanels();
        PlayerPrefs.SetInt("Level", lvlIndex);
        EventController.Instance.OnTutorial.DoEvent(true);
    }

    private void LoadScene(bool _unload)
    {
        StartCoroutine(WaitLoadScene());

        IEnumerator WaitLoadScene()
        {
            if (_unload)
            {
                yield return new WaitForSeconds(UIController.Instance.BlackScreen.ShowBlackScreen());
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            }
            var async = SceneManager.LoadSceneAsync(lvlIndex, LoadSceneMode.Additive);
            yield return new WaitUntil(() => async.isDone);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(lvlIndex));

            UIController.Instance.BlackScreen.HideBlackScreen();
        }
    }

    public void LevelIndexReload(TextMeshProUGUI levelIndexText)
    {
        int numb = lvlIndex + (allLevels - 1) * CountOfLoops;
        levelIndexText.text = "Level " + numb;
    }
}