using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Instance;
    [SerializeField] private GameObject loaderCanvas;
    //[SerializeField] private Image progressBar;
    [SerializeField] private Image loadingPanel;
    [SerializeField] private Image loadingImage;
    private float target = 0;
    private void Awake()
    {
        if(Instance == null)
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
        loadingImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    public async void LoadScene(string sceneName)
    {
        target = 0;
        //progressBar.fillAmount = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        loadingPanel.DOFade(1f, 0.5f).From(0f,true);
        loadingImage.DOFade(1f, 0.5f).From(0f,true);
        do
        {
            await Task.Delay(1000);
            target = scene.progress;
        }
        while (scene.progress < 0.9f);
        await Task.Delay(1000);
        loadingImage.DOFade(0f, 1f).From(1f, true);
        await Task.Delay(2000);
        scene.allowSceneActivation = true;
        loadingPanel.DOFade(0f, 1f).From(1f, true);
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
