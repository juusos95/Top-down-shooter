using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private int points;
    [SerializeField] private TextMeshProUGUI scoreText;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPoint()
    {
        points++;
        scoreText.text = "Score: " + points;
    }
}
