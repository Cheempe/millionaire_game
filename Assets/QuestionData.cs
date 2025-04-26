using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class QuestionData : ScriptableObject
{
    public string questionText;  // Текст питання
    public string[] answers;     // Варіанти відповідей
    public int correctAnswer;    // Індекс правильної відповіді (0-3)
}
