using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;

public class BirdSlot : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public GameObject UIblock;

    public SkeletonGraphic skeletonGraphic;

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
        UISystem.current.ChangeSkinToName(skeletonGraphic, bird.name);

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
