using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private const string GAME_SCENE = "game";

    [SerializeField] private GameObject _stageFailedScreen;
    [SerializeField] private GameObject _stagePassedScreen;
    [SerializeField] private GameObject _rope;
    [SerializeField] private float _changeScreenDelay = 1f;

    private void HandleStageFailed()
    {
        StopAllCoroutines();
        StartCoroutine(ShowScreenAfterDelay(_stageFailedScreen));
    }

    private void HandleStagePassed()
    {
        StartCoroutine(ShowScreenAfterDelay(_stagePassedScreen));
    }

    [UsedImplicitly]
    public void RestartStage()
    {
        StartCoroutine(RestartStageCoroutine());
    }

    private IEnumerator RestartStageCoroutine()
    {
        yield return new WaitForSeconds(_changeScreenDelay);
        SceneManager.LoadScene(GAME_SCENE);
    }
    
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private IEnumerator ShowScreenAfterDelay(GameObject screen)
    {
        yield return new WaitForSeconds(_changeScreenDelay);
        HideAllScreens();
        _rope.SetActive(false);
        screen.SetActive(true);
    }

    private void HideAllScreens()
    {
        _stageFailedScreen.SetActive(false);
        _stagePassedScreen.SetActive(false);
    }
    
    public void TriggerStageFailed()
    {
        HandleStageFailed();
    }

    public void TriggerStagePassed()
    {
        HandleStagePassed();
    }
}