using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class UIController : MonoSingleton<UIController>
{
    [ReadOnly] public int Money = 0;
    [SerializeField, Foldout("Game Panels")] private GameObject _winUI;
    [SerializeField, Foldout("Game Panels")] private GameObject _loseUI;
    [SerializeField, Foldout("Game Panels")] private GameObject _tutorialUI;
    [Foldout("Game Panels")] public BlackScreenAnim BlackScreen;
    [SerializeField, Foldout("Panels Text")] private TextMeshProUGUI _moneyText;
    [SerializeField, Foldout("Panels Text")] private TextMeshProUGUI _levelIndexText;
    [SerializeField, Foldout("Panels Text")] private TextMeshProUGUI _gradeText;
    private bool _wasLastPan = false;

    private void OnEnable()
    {
        EventController.Instance.OnWin.OnEvent += LevelWin;
        EventController.Instance.OnLose.OnEvent += LevelLose;
        EventController.Instance.OnTutorial.OnEventBool += Tutorial;
    }

    private void Start()
    {
        StartCoroutine(StartCor());
    }

    private IEnumerator StartCor()
    {
        yield return new WaitForFixedUpdate();
        LevelController.Instance.LevelIndexReload(_levelIndexText);
        MoneyUpdate();
        yield break;
    }

    private void Tutorial(bool _)
    {
        _tutorialUI.SetActive(_);
    }

    private void LevelWin()
    {
        if (_wasLastPan)
            return;
        _loseUI.SetActive(false);
        _winUI.SetActive(true);
        _wasLastPan = true;
    }

    private void LevelLose()
    {
        if (_wasLastPan)
            return;
        _loseUI.SetActive(true);
        _winUI.SetActive(false);
        _wasLastPan = true;
    }

    public void HideLastPanels()
    {
        _winUI.SetActive(false);
        _loseUI.SetActive(false);
        _wasLastPan = false;
        LevelController.Instance.LevelIndexReload(_levelIndexText);
    }

    public void MoneyUpdate()
    {
        _moneyText.text = "" + Money;
    }

    public void GradeChangeCor(int maxGrade)
    {
        IEnumerator GradeChange()
        {
            for (float t = 0; t < 1; t += Time.deltaTime / 2f)
            {
                _gradeText.text = "" + (int)Mathf.Lerp(0, maxGrade, t);
                yield return null;
            }
            _gradeText.text = "" + maxGrade;
        }
        StartCoroutine(GradeChange());
    }

    private void OnDisable()
    {
        EventController.Instance.OnWin.OnEvent -= LevelWin;
        EventController.Instance.OnLose.OnEvent -= LevelLose;
        EventController.Instance.OnTutorial.OnEventBool -= Tutorial;
    }
}

// UIController.Instance.StopTutorial();                 - to stop Tutorial Panel
// UIController.Instance.LevelEnd(bool _flag);           - to show Win/Lose Panel
// UIController.Instance.MoneyUpdate();                  - to show current count of money
// UIController.Instance.GradeChangeCor(int maxGrade);   - to smoothly show grade count
// Yours ever 3R
