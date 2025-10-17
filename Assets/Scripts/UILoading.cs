using H_Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : BaseUI
{
    public override UIType Type => UIType.Loading;

    [SerializeField] private H_FillBar loadingBar;
    [SerializeField] private float loadingTime = 3f;

    public override void Show()
    {
        loadingBar.UpdateFillBar(0);
        loadingBar.UpdateText($"Loading 0%");
        base.Show();
        StartCoroutine(FillLoadingBar());
    }

    private IEnumerator FillLoadingBar()
    {
        float elapsed = 0f;

        yield return new WaitForSeconds(1f);
        while (elapsed < loadingTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / loadingTime);
            loadingBar.UpdateFillBar(progress);

            int percent = Mathf.RoundToInt(progress * 100);
            loadingBar.UpdateText($"Loading {percent}%");

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        loadingBar.UpdateText("Loading 100%");
        yield return new WaitForSeconds(0.2f);
        Hide();
        UICtrl.I.Show(UIType.Home);
    }
}
