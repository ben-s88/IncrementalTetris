using UnityEngine;
using TMPro;
using Unity.Services.LevelPlay;

public class UIManager : MonoBehaviour
{
    public GameObject HScore;
    TMP_Text HScoreLable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HScoreLable = HScore.GetComponent<TMP_Text>();
        string a = PlayerPrefs.GetInt("HScore").ToString();
        if (a != null)
        {
            HScoreLable.text = "High Score: " + a;
        }

        LevelPlay.SetMetaData("is_test_suite", "enable");

        LevelPlay.OnInitSuccess += LevelPlay_OnInitSuccess;
        LevelPlay.OnInitFailed += LevelPlay_OnInitFailed;

        LevelPlay.Init("2904a93b5");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HScore", Random.Range(1, 100000));
    }

    private void LevelPlay_OnInitFailed(LevelPlayInitError obj)
    {
        Debug.Log("LevelPlay Init failed error: " + obj.ErrorMessage);
    }

    private void LevelPlay_OnInitSuccess(LevelPlayConfiguration obj)
    {
        Debug.Log("LevelPlay Init Success");
        LevelPlay.LaunchTestSuite();
        var configBuilder = new LevelPlayBannerAd.Config.Builder();
        configBuilder.SetSize(LevelPlayAdSize.BANNER);
        configBuilder.SetPosition(LevelPlayBannerPosition.BottomCenter);

        var builtConfig = configBuilder.Build();
        LevelPlayBannerAd bannerAD = new("zgd45nlkbiky2550", builtConfig);
        bannerAD.LoadAd();
    }
}
