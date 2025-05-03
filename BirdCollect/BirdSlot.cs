using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BirdSlot : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public Image iconImage;

    public GameObject UIblock;

    //public TextMeshProUGUI priceText;

    public Bird bird;



    public void ItemOnClicked()
    {
        BirdEncyclopedia.UpdateBirdInfo(bird);
        AudioManager.instance.PlayFX("drop4");

    }
    public void SetupBirdSlot(Bird targetbird)
    {
        if (targetbird == null)
        {
            return;
        }
        bird = targetbird;

        nameText.text = bird.Name;

        iconImage.sprite = bird.Icon;
        //iconImage.SetNativeSize();

        //priceText.text = bird.goodsPrice.ToString();

        Button button = GetComponent<Button>();

        if (bird.isLocked == true)
        {
            UIblock.SetActive(true);
            if (button != null) button.enabled = false;

        }
        else
        {
            UIblock.SetActive(false);
            if (button != null) button.enabled = true;
        }
    }
}
