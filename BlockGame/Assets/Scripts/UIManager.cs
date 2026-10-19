using UnityEngine;
using TMPro;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("HScore", Random.Range(1, 100000));
    }
}
