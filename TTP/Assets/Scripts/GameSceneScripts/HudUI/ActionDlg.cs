using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionDlg : MonoBehaviour
{
    //������ AddListner�� �÷��̾� ������  ó���Ϸ��� �ߴµ�, InputSystem���� �� ��������
    public BtnAction btnJump = null;
    public BtnAction btnRoll = null;
    public BtnAction btnAttack = null;
    public BtnAction btnSkill = null;
    public GameObject m_joystick = null;
}
