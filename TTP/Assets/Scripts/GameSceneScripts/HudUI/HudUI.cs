using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    public ActionDlg actionDlg = null;
    public UnitHealthDlg playerHealthDlg = null;
    public UnitHealthDlg playerPowerDlg = null;
    // Start is called before the first frame update
    void Start()
    {
        playerPowerDlg.Init(0, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
