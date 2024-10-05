using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizUIController : MonoBehaviour
{
    public TextMeshProUGUI questionNumberText;
    public TextMeshProUGUI questionText;
    public Toggle[] optionToggles;
    public Button submitButton;
    public Button leftButton;
    public Button rightButton;

    private QuizManager quizManager;
    private QuizState quizState;

    private void Start()
    {
        quizManager = GetComponent<QuizManager>();
        SetupListeners();
    }

    public void InitializeUI(QuizState state)
    {
        quizState = state;
        DisplayQuestion(0);
    }

    public void DisplayQuestion(int index)
    {
        QuizQuestion question = quizState.GetCurrentQuestion();
        questionNumberText.text = $"Q{index + 1}";
        questionText.text = question.question_text;

        for (int i = 0; i < optionToggles.Length; i++)
        {
            optionToggles[i].GetComponentInChildren<TextMeshProUGUI>().text = question.GetOption(i);
            optionToggles[i].isOn = quizState.IsOptionSelected(index, i);
            optionToggles[i].group = question.correct_answer.Count > 1 ? null : optionToggles[0].group;
        }

        leftButton.interactable = index > 0;
        rightButton.interactable = index < quizState.TotalQuestions - 1;
    }

    private void SetupListeners()
    {
        submitButton.onClick.AddListener(quizManager.SubmitQuiz);
        leftButton.onClick.AddListener(() => quizManager.NavigateQuestion(-1));
        rightButton.onClick.AddListener(() => quizManager.NavigateQuestion(1));

        for (int i = 0; i < optionToggles.Length; i++)
        {
            int index = i;
            optionToggles[i].onValueChanged.AddListener((bool isOn) => quizManager.UpdateAnswer(index, isOn));
        }
    }

    public void DisplayQuizResult(QuizResult result)
    {
        Debug.Log($"Quiz Score: {result.score}, Improved: {result.improved}");
    }
}