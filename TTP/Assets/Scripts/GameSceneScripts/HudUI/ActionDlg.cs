using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionDlg : MonoBehaviour
{
    //원래는 AddListner로 플레이어 움직임  처리하려고 했는데, InputSystem으로 다 때워버림
    public BtnAction btnJump = null;
    public BtnAction btnRoll = null;
    public BtnAction btnAttack = null;
    public BtnAction btnSkill = null;
    public GameObject m_joystick = null;
}
