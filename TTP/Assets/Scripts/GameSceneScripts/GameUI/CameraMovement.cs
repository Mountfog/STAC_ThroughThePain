using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTimeX = 0.25f;
    [SerializeField] private float smoothTimeY = 0.3f;
    [SerializeField] private float smoothTimeZ = 0.3f;
    private float velocityX = 0f;
    private float velocityY = 0f;
    private float velocityZ = 0f;
    [SerializeField]private float cameraScale = 1f;

    public Transform targetx;
    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        if (GameMgr.Inst.gameScene.battleFSM.IsGameState())
        {
            Vector3 targetPosition = targetx.position * cameraScale + offset;
            float x = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocityX, smoothTimeX);
            float y = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocityY, smoothTimeY);
            float z = Mathf.SmoothDamp(transform.position.z, targetPosition.z, ref velocityZ, smoothTimeZ);
            y = Mathf.Max(-2.8f, y);
            transform.position = new Vector3(x, y, z);
        }
    }
}
