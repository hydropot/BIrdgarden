using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class IsBuyPanel : MonoBehaviour
{
    public Text titleText;
    
    public string titleFormat ="是否花费 {0}  购买 ？";
    
    public Text nameText;
    
    public Image iconImage;
    
    
    public Button confirmBtn;
    public Button cancelBtn;

    public StoreLibrary myLibrary;
    
    public Goods currentGoods;

    public TextMeshProUGUI text_Warning;

    private void Awake()
    {
        confirmBtn.onClick.AddListener(OnConfirmBtnClick);
        cancelBtn.onClick.AddListener(OnCancelBtnClick);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void InitIsBuyPanel(Goods goods)
    {
        titleText.text = string.Format(titleFormat, goods.goodsPrice);
        nameText.text = goods.goodsName;
        iconImage.sprite = goods.goodsSprite;
        currentGoods = goods;
        gameObject.SetActive(true);
        text_Warning.gameObject.SetActive(false);
    }

    private void OnCancelBtnClick()
    {
        gameObject.SetActive(false);
    }

    private void OnConfirmBtnClick()
    {
        //扣钱 获得物品 
        bool success = GameManager.current.SpendCurrency(CurrencyType.Coins, currentGoods.goodsPrice);

        if (success)
        {
            myLibrary.goodsList.Add(currentGoods);
            currentGoods.isSold = true;
            BackpackPagination.instance.RefreshPages(); // 更新显示
            ShopManager.current.ReloadShop();
            gameObject.SetActive(false);

            //GameManager.current.UPSaveToCloud();
        }
        else
        {
            Debug.Log("购买失败，金币不足！");
            // 这里你也可以加一些提示 UI，比如弹出“不够钱”的提示。
            StartCoroutine(ShowWarning());
        }
    }

    private IEnumerator ShowWarning()
    {
        text_Warning.gameObject.SetActive(true);

        // 设置初始透明度为 1（不透明）
        Color color = text_Warning.color;
        color.a = 1f;
        text_Warning.color = color;

        float duration = 1f; // 淡出时间
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            color.a = alpha;
            text_Warning.color = color;
            yield return null;
        }

        text_Warning.gameObject.SetActive(false);

        // 重置透明度为 1 以便下次使用
        color.a = 1f;
        text_Warning.color = color;
    }


}
