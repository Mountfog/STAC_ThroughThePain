using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ResultUIDlg : MonoBehaviour
{
    #region Vars

    [Header("UI Elements")]
    public Image panelImg = null;
    public GameObject uiElements = null;

    [Space(10.0f)]

    public GameObject title_Clear = null;
    public GameObject title_Fail = null;

    public Transform[] starIcons = new Transform[] { };
    public GameObject[] fx_StarEffect = new GameObject[] { };
    public Text txt_PlayTime = null;
    public ParticleSystem fx_Clear = null;
    public ParticleSystem fx_Fail = null;

    [Space(10.0f)]

    public Button btnReturn = null;
    public Button btnReTry = null;
    public Button btnNextStage = null;

    [Space(20.0f)]

    [Header("Etc")]
    public Camera cam = null;
    public Volume volume = null;
    DepthOfField dop = null;

    bool panelActive = false;

    #endregion

    private void Awake()
    {
        cam = Camera.main;

        volume.profile.TryGet(out dop);
        dop.focalLength.value = 1.0f;   // 백그라운드 블러 설정
    }

    private void Start()
    {
        panelImg.raycastTarget = false;
        uiElements.transform.localPosition = new Vector3(0, -400, 0);

        starIcons[0].GetChild(0).gameObject.SetActive(false);
        starIcons[1].GetChild(0).gameObject.SetActive(false);
        starIcons[2].GetChild(0).gameObject.SetActive(false);

        btnReturn.GetComponent<CanvasGroup>().alpha = 0;
        btnReTry.GetComponent<CanvasGroup>().alpha = 0;
        btnNextStage.GetComponent<CanvasGroup>().alpha = 0;

        btnReturn.onClick.AddListener(OnClicked_BtnReturn);
        btnReTry.onClick.AddListener(OnClicked_BtnRetry);
        btnNextStage.onClick.AddListener(OnClicked_BtnNextStage);

        //transform.gameObject.SetActive(false);
    }

    public void Init(bool isClear, float playTime)
    {
        panelActive = true;
        panelImg.raycastTarget = true;

        title_Clear.SetActive(isClear);
        title_Fail.SetActive(!isClear);
        fx_Clear.gameObject.SetActive(isClear);
        fx_Fail.gameObject.SetActive(!isClear);

        StartCoroutine(Enum_UIAnimate(playTime, isClear));
    }


    void OnClicked_BtnReturn() { }
    void OnClicked_BtnRetry() { }
    void OnClicked_BtnNextStage() { }



        #region UI Animation

    IEnumerator Enum_UIAnimate(float playTime, bool isClear)
    {
        StartCoroutine(Enum_PanelShow(0.75f));
        yield return new WaitForSeconds(1.0f);

        StartCoroutine(Enum_ShowElements(1.2f));
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(Enum_TimerEffect(1.25f, playTime));
        yield return new WaitForSeconds(1.5f);

        if (isClear)
        {
            for (int i = 0; i < 3; i++)     // 이거 기준 해줘
            {
                StartCoroutine(Enum_StarEffect(1.0f, i));
                yield return new WaitForSeconds(0.4f);
            }
        }

        yield return new WaitForSeconds(1.25f);

        StartCoroutine(Enum_ButtonShow(1.25f, btnReturn));
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(Enum_ButtonShow(1.25f, btnReTry));
        yield return new WaitForSeconds(0.15f);

        if (isClear) StartCoroutine(Enum_ButtonShow(1.25f, btnNextStage));
    }


    IEnumerator Enum_PanelShow(float time)
    {
        float lerpTime = time;
        float curTime = 0.0f;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;
            t = 1 - Mathf.Pow(1 - t, 4);

            dop.focalLength.value = Mathf.Lerp(1, 30, t);                           // 블러
            panelImg.color = Color.Lerp(Color.clear, new Color32(0, 0, 0, 220), t); // 패널
            cam.orthographicSize = Mathf.Lerp(8.0f, 6.5f, t);                       // 카메라

            yield return null;
        }
    }
    IEnumerator Enum_ShowElements(float time)
    {
        float lerpTime = time;
        float curTime = 0.0f;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;
            t = 1 - Mathf.Pow(1 - t, 4);

            uiElements.transform.localPosition = Vector3.Lerp(new Vector3(0, -400, 0), Vector3.zero, t);

            yield return null;
        }
    }
    IEnumerator Enum_TimerEffect(float time, float playTime)
    {
        float lerpTime = time;
        float curTime = 0.0f;

        float timeValue = 0.0f;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;

            timeValue = Mathf.Lerp(0, playTime, t);

            string m = string.Format("{0:00}", (int)timeValue / 60);
            string s = string.Format("{0:00}", (int)timeValue % 60);

            txt_PlayTime.text = $"플레이 시간 : {m}:{s}";

            yield return null;
        }
    }


    IEnumerator Enum_StarEffect(float time, int starIdx)
    {
        float lerpTime = time;
        float curTime = 0.0f;

        GameObject starIcon = starIcons[starIdx].transform.GetChild(0).gameObject;
        starIcon.SetActive(true);

        starIcon.transform.localEulerAngles = new Vector3(0, 0, 180);
        starIcon.transform.localScale = Vector3.one * 3;
        starIcon.GetComponent<Image>().color = Color.clear;

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;
            t = t * t * t * t;

            starIcon.transform.localEulerAngles = Vector3.Lerp(new Vector3(0, 0, 120), Vector3.zero, t);
            starIcon.transform.localScale = Vector3.Lerp(Vector3.one * 2, Vector3.one, t);
            starIcon.GetComponent<Image>().color = Color.Lerp(Color.clear, Color.white, t);

            yield return null;
        }

        fx_StarEffect[starIdx].gameObject.SetActive(true);
    }

    IEnumerator Enum_ButtonShow(float time, Button _button)
    {
        float lerpTime = time;
        float curTime = 0.0f;

        Vector3 startPos = _button.transform.localPosition - new Vector3(0, 30, 0);
        Vector3 endPos = _button.transform.localPosition;

        CanvasGroup group = _button.GetComponent<CanvasGroup>();

        while (curTime != lerpTime)
        {
            curTime = curTime > lerpTime ? lerpTime : curTime += Time.deltaTime;
            float t = curTime / lerpTime;
            t = 1 - Mathf.Pow(1 - t, 4);

            _button.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            group.alpha = Mathf.Lerp(0, 1, t);

            yield return null;
        }
    }

    #endregion
}
