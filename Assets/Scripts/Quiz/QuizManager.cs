using UnityEngine;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    public QuizUIController uiController;
    private QuizAPIService apiService;
    private QuizState quizState;
    public SubmitPopupController submitPopupController;
    public QuizScoreUIHandler quizScoreUIHandler;
    private QuizTimer quizTimer;

    private string currentSubcategory;

    public void InitializeQuiz(string subcategory)
    {
        currentSubcategory = subcategory;
        quizState = new QuizState(subcategory);
        StartCoroutine(InitializeQuiz());
    }

    private void Start()
    {
        uiController = GetComponent<QuizUIController>();
        submitPopupController = GetComponent<SubmitPopupController>();
        quizScoreUIHandler = GetComponent<QuizScoreUIHandler>();
        apiService = new QuizAPIService();
        quizTimer = GetComponent<QuizTimer>();
        quizTimer.OnTimerEnd += HandleTimerEnd;

        if (!string.IsNullOrEmpty(ModuleData.CurrentSubcategory))
        {
            quizState = new QuizState(ModuleData.CurrentSubcategory);
            StartCoroutine(InitializeQuiz());
        }
        else
        {
            Debug.LogError("No subcategory set before loading Quiz scene");
        }
    }

    private IEnumerator InitializeQuiz()
    {
        yield return StartCoroutine(apiService.FetchQuizQuestions(quizState.Subcategory));
        if (apiService.QuizQuestions != null && apiService.QuizQuestions.Count > 0)
        {
            quizState.SetQuizQuestions(apiService.QuizQuestions);
            uiController.InitializeUI(quizState);
            quizTimer.StartTimer();
        }
        else
        {
        }
    }
    private void Update()
    {
        if (quizTimer != null)
        {
            uiController.UpdateTimerDisplay(quizTimer.GetFormattedTime());
        }
    }

    private void HandleTimerEnd()
    {
        FinalizeSubmission();
    }

    public void NavigateQuestion(int direction)
    {
        quizState.NavigateQuestion(direction);
        uiController.DisplayQuestion(quizState.CurrentQuestionIndex);
    }

    public void UpdateAnswer(int optionIndex, bool isSelected)
    {
        quizState.UpdateAnswer(optionIndex, isSelected);
    }

    public void SubmitQuiz()
    {
        quizTimer.PauseTimer();
        submitPopupController.ShowConfirmationPopup();
    }

    public void OnSubmitConfirmed()
    {
        quizTimer.StopTimer();
        FinalizeSubmission();
    }

    public void OnSubmitCancelled()
    {
        quizTimer.ResumeTimer();
    }

    public void FinalizeSubmission()
    {
        int score = quizState.CalculateScore();
        quizScoreUIHandler.Score = score;
        submitPopupController.ShowScoreUI(score);
    }
}