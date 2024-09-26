using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance;
    [SerializeField] private GameObject loaderCanvas;
    //[SerializeField] private Image progressBar;
    [SerializeField] CanvasGroup m_CG;
    [SerializeField] private Image loadingPanel;
    [SerializeField] private List<Image> loadingImage = new List<Image>();

    private float target = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        loaderCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        loadingPanel.GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.InQuint).From(1f);
        foreach(Image i in loadingImage)
        {
            i.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        m_CG.blocksRaycasts = false;
    }
    public async void LoadScene(string sceneName)
    {
        m_CG.blocksRaycasts = true;
        target = 0;
        //progressBar.fillAmount = 0;

        loadingPanel.DOFade(1f, 0.5f).From(0f, true).SetUpdate(true);
        foreach (Image i in loadingImage)
        {
            i.DOFade(1f, 0.5f).From(0f, true).SetUpdate(true);
        }

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            await Task.Delay(1000);
            target = scene.progress;
        }
        while (scene.progress < 0.9f);
        await Task.Delay(1000);
        foreach (Image i in loadingImage)
        {
            i.DOFade(0f, 1f).From(1f, true).SetUpdate(true);
        }
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        loadingPanel.DOFade(0f, 1f).From(1f, true).SetUpdate(true);
        m_CG.blocksRaycasts = false;
        //loaderCanvas.SetActive(false);
    }
    public void FadePanel()
    {

    }
    private void Update()
    {
        //progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }
}
