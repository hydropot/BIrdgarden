using UnityEngine;
using UnityEngine.SceneManagement;

public class UISystem : MonoBehaviour
{
    [Header("Main UI Elements")]
    public GameObject gridRoot;             // 父级，控制所有网格显隐
    public GameObject panelMainMenu;            // Menu 主菜单
    public GameObject panelUIBlock;         // UI Block 覆盖
    public GameObject panelShop;      // 商店Panel（全屏
    public GameObject panelBuilding;            // 建筑Panel（建筑模式）
    public GameObject buttonCloseBuild;     // 退出建筑模式按钮

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

    // 打开菜单
    public void OnMenuOpen()
    {
        panelMainMenu.SetActive(true);
        panelUIBlock.SetActive(true);
        //buttonMenuOpen.SetActive(false);
    }

    // 关闭当前页面
    public void OnCloseButtonClick(GameObject button)//可用于页面上的关闭按钮/UI BLOCK，要求其处在被关闭的面板的父级下（一起关闭）。参数为按钮/UI BLOCK自身
    {
        if (button != null && button.transform.parent != null)
        {
            button.transform.parent.gameObject.SetActive(false);
        }
    }

    /*
    // 点击UI Block区域关闭菜单
    public void OnOverlayClick()
    {
        OnMenuClose();
    }
    */


    //方式1：开启全屏Panel
    public void OnOpenButtonClick1(GameObject panel)//适用于全屏等(不需要原菜单关闭的)按钮
    {
        //panelShop.SetActive(true);
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    // 方式2：开启模式类Panel
    public void OnOpenButtonClick2(GameObject panel)//适用于开启模式等(需要原菜单关闭的)按钮
    {
        panelMainMenu.SetActive(false);

        panel.SetActive(true);

        //isBuildMode = true;
    }

    // 退出建筑模式，回到菜单
    public void OnCloseBuildMode()
    {
        gridRoot.SetActive(false);
        panelBuilding.SetActive(false);
        buttonCloseBuild.SetActive(false);

        panelMainMenu.SetActive(true);
        panelUIBlock.SetActive(true);

        //isBuildMode = false;
    }


    //未完成
    // 方式3：新加载Scene
    public void OnOpenButtonClick3()
    {
        SceneManager.LoadScene("Scene1");
    }

    // Scene中的退出按钮
    public void OnExitScene1()
    {
        SceneManager.LoadScene("SampleScene");
    }
}