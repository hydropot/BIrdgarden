using UnityEngine;
using UnityEngine.SceneManagement;

public class UISystem : MonoBehaviour
{
    [Header("Main UI Elements")]
    public GameObject gridRoot;             // ����������������������
    public GameObject panelMainMenu;            // Menu ���˵�
    public GameObject panelUIBlock;         // UI Block ����
    public GameObject panelShop;      // �̵�Panel��ȫ��
    public GameObject panelBuilding;            // ����Panel������ģʽ��
    public GameObject buttonCloseBuild;     // �˳�����ģʽ��ť

    [Header("Buttons")]
    public GameObject buttonMenuOpen;

    //private bool isBuildMode = false;

    void Start()
    {
        panelMainMenu.SetActive(false);
        panelShop.SetActive(false);
        panelBuilding.SetActive(false);

        buttonMenuOpen.SetActive(true);
    }

    // �򿪲˵�
    public void OnMenuOpen()
    {
        panelMainMenu.SetActive(true);
        panelUIBlock.SetActive(true);
        //buttonMenuOpen.SetActive(false);
    }

    // �رյ�ǰҳ��
    public void OnCloseButtonClick(GameObject button)//������ҳ���ϵĹرհ�ť/UI BLOCK��Ҫ���䴦�ڱ��رյ����ĸ����£�һ��رգ�������Ϊ��ť/UI BLOCK����
    {
        if (button != null && button.transform.parent != null)
        {
            button.transform.parent.gameObject.SetActive(false);
        }
    }

    /*
    // ���UI Block����رղ˵�
    public void OnOverlayClick()
    {
        OnMenuClose();
    }
    */


    //��ʽ1������ȫ��Panel
    public void OnOpenButtonClick1(GameObject panel)//������ȫ����(����Ҫԭ�˵��رյ�)��ť
    {
        //panelShop.SetActive(true);
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    // ��ʽ2������ģʽ��Panel
    public void OnOpenButtonClick2(GameObject panel)//�����ڿ���ģʽ��(��Ҫԭ�˵��رյ�)��ť
    {
        panelMainMenu.SetActive(false);

        panel.SetActive(true);

        //isBuildMode = true;
    }

    // �˳�����ģʽ���ص��˵�
    public void OnCloseBuildMode()
    {
        gridRoot.SetActive(false);
        panelBuilding.SetActive(false);
        buttonCloseBuild.SetActive(false);

        panelMainMenu.SetActive(true);
        panelUIBlock.SetActive(true);

        //isBuildMode = false;
    }


    //δ���
    // ��ʽ3���¼���Scene
    public void OnOpenButtonClick3()
    {
        SceneManager.LoadScene("Scene1");
    }

    // Scene�е��˳���ť
    public void OnExitScene1()
    {
        SceneManager.LoadScene("SampleScene");
    }
}