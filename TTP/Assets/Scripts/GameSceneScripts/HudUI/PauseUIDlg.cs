using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIDlg : MonoBehaviour
{
    private void OnEnable()
    {
        ResourceMgr.Instance.Blur(30,0.5f);
    }
}
