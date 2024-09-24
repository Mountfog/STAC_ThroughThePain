using UnityEngine;
using UnityEngine.UI;

public class OptionToggle : MonoBehaviour
{
    public Toggle toggle => GetComponent<Toggle>();
    public Animator animator => GetComponent<Animator>();

    void Start()
    {
        // Toggle 상태 변화 시 애니메이션 적용
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }
    private void OnEnable()
    {
        animator.SetBool("isOn", toggle.isOn);
    }

    // Toggle 상태 변화에 따른 애니메이션 실행
    private void OnToggleChanged(bool isOn)
    {
        animator.SetBool("isOn", isOn);
        animator.SetTrigger("Anim");
    }
}
