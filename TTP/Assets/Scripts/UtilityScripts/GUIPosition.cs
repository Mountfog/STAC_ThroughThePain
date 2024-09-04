using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GUIPosition : MonoBehaviour
{
    #region Cameramovement
    private Camera uiCamera; //UI ī�޶� ���� ����
    private Canvas canvas; //ĵ������ ���� ����
    private RectTransform rectParent; //�θ��� rectTransform ������ ������ ����
    private RectTransform rectHp; //�ڽ��� rectTransform ������ ����

    //HideInInspector�� �ش� ���� �����, ���� ������ �ʿ䰡 ���� �� 
    public Vector3 offset = Vector3.zero; //HpBar ��ġ ������, offset�� ��� HpBar�� ��ġ �������
    public Transform enemyTr; //�� ĳ������ ��ġ
    public Vector3 enemyTrPos;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>(); //�θ� �������ִ� canvas ��������, Enemy HpBar canvas��
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    //LateUpdate�� update ���� ������, ���� �������� Update���� ����Ǵ� ������ ���Ŀ� HpBar�� �����
    private void LateUpdate()
    {
        if (!enemyTr.IsDestroyed())
        {
            enemyTrPos = enemyTr.position;
            var screenPos = Camera.main.WorldToScreenPoint(enemyTrPos + offset); //������ǥ(3D)�� ��ũ����ǥ(2D)�� ����, offset�� ������Ʈ �Ӹ� ��ġ
            /*
            if(screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
                //x, y, (z) ����ī�޶󿡼� XY�������� �Ÿ�
            }
            ������������ �ڵ� ��� HpBar�� ���̴� ������ �־ ���� �ڵ�� �Ⱥ��̰� ������, ���� ���ͺ� �����̶� �ʿ����
            */

            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); //��ũ����ǥ���� ĵ�������� ����� �� �ִ� ��ǥ�� ����?

            rectHp.localPosition = localPos; //�� ��ǥ�� localPos�� ����, �ű⿡ hpbar�� ���
        }
        else
        {
            var screenPos = Camera.main.WorldToScreenPoint(enemyTrPos + offset); //������ǥ(3D)�� ��ũ����ǥ(2D)�� ����, offset�� ������Ʈ �Ӹ� ��ġ
            /*
            if(screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
                //x, y, (z) ����ī�޶󿡼� XY�������� �Ÿ�
            }
            ������������ �ڵ� ��� HpBar�� ���̴� ������ �־ ���� �ڵ�� �Ⱥ��̰� ������, ���� ���ͺ� �����̶� �ʿ����
            */

            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); //��ũ����ǥ���� ĵ�������� ����� �� �ִ� ��ǥ�� ����?

            rectHp.localPosition = localPos;
        }
    }
    #endregion
}
