using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BirdEncyclopedia : MonoBehaviour
{
    public static BirdEncyclopedia instance;

    [Header("UI References")]
    public ScrollRect scrollRect;
    public GameObject pagePrefab;
    public GameObject birdItemPrefab;
    public Button prevBtn;
    public Button nextBtn;
    public Text pageText;

    [Header("Info Panel")]
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI nameText;

    [Header("Data Source")]
    public BirdLibrary birdLibrary; // 包含所有鸟类数据

    [Header("Animation Settings")]
    public float scrollDuration = 0.3f;
    public Ease scrollEase = Ease.OutQuad;

    private int currentPage = 0;
    private int totalPages = 0;
    private float pageWidth;
    private bool isAnimating;

    private Bird currentBird;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

        prevBtn.onClick.AddListener(PrevPage);
        nextBtn.onClick.AddListener(NextPage);
    }

    IEnumerator Start()
    {
        yield return null; // 等待一帧布局更新
        pageWidth = scrollRect.viewport.rect.width;

        GeneratePages(birdLibrary.birdList);
        ScrollToPage(0);
        UpdateUI();
        infoPanel.SetActive(false);
    }

    public void RefreshPages()
    {
        GeneratePages(birdLibrary.birdList);
        ScrollToPage(currentPage);
    }

    public static void UpdateBirdInfo(Bird bird)
    {
        instance.infoPanel.SetActive(true);
        instance.currentBird = bird;
        instance.infoText.text = bird.Description;
        instance.nameText.text = bird.Name;
    }

    void GeneratePages(List<Bird> birds)
    {
        foreach (Transform child in scrollRect.content)
            Destroy(child.gameObject);

        totalPages = Mathf.CeilToInt(birds.Count / 4f);
        for (int i = 0; i < totalPages; i++)
        {
            var page = Instantiate(pagePrefab, scrollRect.content);
            for (int j = 0; j < 4; j++)
            {
                int index = i * 4 + j;
                if (index < birds.Count)
                {
                    var go = Instantiate(birdItemPrefab, page.transform);
                    go.GetComponent<BirdSlot>().SetupBirdSlot(birds[index]);
                }
            }
        }

        scrollRect.content.sizeDelta = new Vector2(
            pageWidth * totalPages,
            scrollRect.content.sizeDelta.y
        );
    }

    public void NextPage()
    {
        if (!isAnimating && currentPage < totalPages - 1)
        {
            currentPage++;
            StartCoroutine(ScrollRoutine(currentPage));
            UpdateUI();
        }
    }

    public void PrevPage()
    {
        if (!isAnimating && currentPage > 0)
        {
            currentPage--;
            StartCoroutine(ScrollRoutine(currentPage));
            UpdateUI();
        }
    }

    IEnumerator ScrollRoutine(int targetPage)
    {
        isAnimating = true;
        UpdateUI();

        float targetPos = (float)targetPage / (totalPages > 1 ? totalPages - 1 : 1);

        yield return DOTween.To(
            () => scrollRect.horizontalNormalizedPosition,
            x => scrollRect.horizontalNormalizedPosition = x,
            targetPos,
            scrollDuration
        ).SetEase(scrollEase).WaitForCompletion();

        isAnimating = false;
        UpdateUI();
    }

    void ScrollToPage(int pageIndex)
    {
        float normalizedPos = (float)pageIndex / (totalPages > 1 ? totalPages - 1 : 1);
        scrollRect.horizontalNormalizedPosition = normalizedPos;
    }

    void UpdateUI()
    {
        bool allowPrev = !isAnimating && currentPage > 0;
        bool allowNext = !isAnimating && currentPage < totalPages - 1;

        prevBtn.interactable = allowPrev;
        nextBtn.interactable = allowNext;

        pageText.text = $"第 {currentPage + 1} 页 / 共 {totalPages} 页";
    }
}
