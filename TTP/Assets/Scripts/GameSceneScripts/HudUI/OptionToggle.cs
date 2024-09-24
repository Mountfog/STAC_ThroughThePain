using UnityEngine;
using UnityEngine.UI;

public class OptionToggle : MonoBehaviour
{
    public Toggle toggle => GetComponent<Toggle>();
    public Animator animator => GetComponent<Animator>();

    void Start()
    {
        // Toggle ���� ��ȭ �� �ִϸ��̼� ����
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }
    private void OnEnable()
    {
        animator.SetBool("isOn", toggle.isOn);
    }

    // Toggle ���� ��ȭ�� ���� �ִϸ��̼� ����
    private void OnToggleChanged(bool isOn)
    {
        animator.SetBool("isOn", isOn);
        animator.SetTrigger("Anim");
    }
}
