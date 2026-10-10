using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public int scoreTotal = 0;
    public int numberOfThrownArrows = 0;
    
    public TextMeshProUGUI textScore;

    public void AddScore(int points)
    {
        scoreTotal += points;
        numberOfThrownArrows++;
        if(textScore != null)
        {
            textScore.text = $"Score: {scoreTotal}\nThrows: {numberOfThrownArrows}";
        }
    }
}