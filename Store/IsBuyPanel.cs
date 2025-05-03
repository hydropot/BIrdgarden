using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class IsBuyPanel : MonoBehaviour
{
    public Text titleText;
    
    public string titleFormat ="�Ƿ񻨷� {0}  ���� ��";
    
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
        //��Ǯ �����Ʒ 
        bool success = GameManager.current.SpendCurrency(CurrencyType.Coins, currentGoods.goodsPrice);

        if (success)
        {
            myLibrary.goodsList.Add(currentGoods);
            currentGoods.isSold = true;
            BackpackPagination.instance.RefreshPages(); // ������ʾ
            ShopManager.current.ReloadShop();
            gameObject.SetActive(false);

            //GameManager.current.UPSaveToCloud();
        }
        else
        {
            Debug.Log("����ʧ�ܣ���Ҳ��㣡");
            // ������Ҳ���Լ�һЩ��ʾ UI�����絯��������Ǯ������ʾ��
            StartCoroutine(ShowWarning());
        }
    }

    private IEnumerator ShowWarning()
    {
        text_Warning.gameObject.SetActive(true);

        // ���ó�ʼ͸����Ϊ 1����͸����
        Color color = text_Warning.color;
        color.a = 1f;
        text_Warning.color = color;

        float duration = 1f; // ����ʱ��
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

        // ����͸����Ϊ 1 �Ա��´�ʹ��
        color.a = 1f;
        text_Warning.color = color;
    }


}
