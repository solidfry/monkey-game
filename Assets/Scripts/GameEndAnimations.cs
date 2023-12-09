using Database;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndAnimations : MonoBehaviour
{
    [SerializeField] private Image _blackOut;

    private void FadeToBlack()
    {
        _blackOut.gameObject.SetActive(true);
        _blackOut.DOColor(Color.black, 4f).OnComplete(() => SceneManager.LoadScene("EndScreen"));
    }

    private void OnEnable() => CombinationDatabase.OnFinalCombination += FadeToBlack;
    private void OnDisable() => CombinationDatabase.OnFinalCombination -= FadeToBlack;
}
