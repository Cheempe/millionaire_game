using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class QuestionData : ScriptableObject
{
    public string questionText;  // ����� �������
    public string[] answers;     // ������� ��������
    public int correctAnswer;    // ������ ��������� ������ (0-3)
}
