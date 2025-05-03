using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class BackpackPagination : MonoBehaviour
{
    
    public static BackpackPagination instance;
    [Header("References")]
    public ScrollRect scrollRect;
    public GameObject pagePrefab;
    public GameObject itemPrefab;
    public Button prevBtn;
    public Button nextBtn;
    public Text pageText;
    
    [Header("Info")]
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI nameText;
    public Button buyBtn;
    
    [Header("Buy")]
    public GameObject buyPanel;
    
    public Goods currentGoods;
    
    
    
    public StoreLibrary storeLibrary;
    
    [Header("Animation Settings")]
    public float scrollDuration = 0.3f;
    public Ease scrollEase = Ease.OutQuad;

    private bool isAnimating;
    
    
    private int currentPage = 0;
    private int totalPages = 0;
    private float pageWidth;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
        prevBtn.onClick.AddListener(PrevPage);
        nextBtn.onClick.AddListener(NextPage);
        buyBtn.onClick.AddListener(OnBuyBtnClick);
       
    }

    public void RefreshPages()
    {
        GeneratePages(storeLibrary.goodsList);
        ScrollToPage(currentPage);
    }


    IEnumerator Start()
    {
        // �ȴ�һ֡ȷ�����ּ������
        yield return null;
        
        // ��ʼ��ҳ��
        pageWidth = scrollRect.viewport.rect.width;
        Debug.Log(pageWidth);
        GeneratePages(storeLibrary.goodsList); // ʾ����������
        
        // ��ʼλ������
        ScrollToPage(0);
        UpdateUI();
        instance.infoPanel.SetActive(false);
    }
    
    
    public static void UpdateItemInfo(Goods targetGoods)
    {
        instance.infoPanel.SetActive(true);
         instance.currentGoods = targetGoods;
         instance.infoText.text = targetGoods.goodsInfo;
         instance.nameText.text = targetGoods.goodsName;
    }


    private void OnBuyBtnClick()
    {
        buyPanel.GetComponent<IsBuyPanel>().InitIsBuyPanel(currentGoods);
    }
    
    
    
    

    void GeneratePages(List<Goods> totalGoods)
    {
        // �����ҳ��
        foreach (Transform child in scrollRect.content)
            Destroy(child.gameObject);

        // ������ҳ��
        totalPages = Mathf.CeilToInt(totalGoods.Count / 4f);
        for (int i=0; i<totalPages; i++)
        {
            var page = Instantiate(pagePrefab, scrollRect.content);
            for (int j=0; j<4; j++)
            {
                int itemIndex = i*4 + j;
                if (itemIndex < totalGoods.Count)
                {
                    GameObject go = Instantiate(itemPrefab, page.transform);
                    go.GetComponent<GoodsSlot>().SetupGoodsSlot(totalGoods[itemIndex]);
                }

               
            }
        }

        // ��������������
        scrollRect.content.sizeDelta = new Vector2(
            pageWidth * totalPages,
            scrollRect.content.sizeDelta.y
        );
    }

    public void NextPage()
    {
        if(!isAnimating && currentPage < totalPages-1)
        {
            currentPage++;
            StartCoroutine(ScrollRoutine(currentPage));
            UpdateUI();
        }
    }

    public void PrevPage()
    {
        if(!isAnimating && currentPage > 0)
        {
            currentPage--;
            StartCoroutine(ScrollRoutine(currentPage));
            UpdateUI();
        }
    }
    
    IEnumerator ScrollRoutine(int targetPage)
    {
        isAnimating = true;
        UpdateUI(); // �������°�ť״̬

        float targetPos = (float)targetPage / (totalPages > 1 ? totalPages-1 : 1);
    
        // �������ȴ��������
        yield return DOTween.To(
            () => scrollRect.horizontalNormalizedPosition,
            x => scrollRect.horizontalNormalizedPosition = x,
            targetPos,
            scrollDuration
        ).SetEase(scrollEase).WaitForCompletion();

        isAnimating = false;
        UpdateUI(); // �����ٴθ���״̬
    }
    
    

    void ScrollToPage(int pageIndex)
    {
        float normalizedPos = (float)pageIndex / (totalPages > 1 ? totalPages-1 : 1);
        scrollRect.horizontalNormalizedPosition = normalizedPos;
    }

    void UpdateUI()
    {
        bool allowPrev = !isAnimating && currentPage > 0;
        bool allowNext = !isAnimating && currentPage < totalPages - 1;

        prevBtn.interactable = allowPrev;
        nextBtn.interactable = allowNext;
        
        pageText.text = $"�� {currentPage+1} ҳ / �� {totalPages} ҳ";
    }
}