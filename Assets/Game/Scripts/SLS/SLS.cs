using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

public class SLS : MonoSingleton<SLS>
{
    private Save SV = new Save();

    [Button("PlayerPrefs Clear")]
    private void ClearPlayerPrefs()
    {
        Debug.Log("PlayerPrefs Cleared!");
        PlayerPrefs.DeleteAll();
    }
    private void Start()
    {
        UploadData();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
            Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    public void Save()
    {
        SV.AudioEnabled = AudioController.Instance.AudioEnabled;
        SV.TapTicEnabled = TapTicController.Instance.TapTicEnabled;
        SV.CountOfLoops = LevelController.Instance.CountOfLoops;
        SV.Money = UIController.Instance.Money;
        PlayerPrefs.SetString("SV", JsonUtility.ToJson(SV));
    }
    private void UploadData()
    {
        if (PlayerPrefs.HasKey("SV"))
        {
            SV = JsonUtility.FromJson<Save>(PlayerPrefs.GetString("SV"));
            UIController.Instance.Money = SV.Money;
            LevelController.Instance.CountOfLoops = SV.CountOfLoops;
            TapTicController.Instance.TapTicEnabled = SV.TapTicEnabled;
            AudioController.Instance.AudioEnabled = SV.AudioEnabled;
        }
    }
}
public class Save
{
    public bool AudioEnabled;
    public bool TapTicEnabled;
    public int CountOfLoops;
    public int Money;
}
