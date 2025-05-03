using Spine.Unity;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class BirdBehavior : MonoBehaviour
{
    public float actlevel = 1f;
    private SkeletonMecanim skeletonMecanim;
    private Animator animator;

    public Bird bird;
    public BirdLibrary mybirdlibrary;

    private Transform grandParent;
    private ObjectType itemType;

    private bool isFlyingAway = false;

    enum BirdState
    {
        Idle = 0,
        Eating = 1,
        Watching = 2,
        Singing = 3,
        TurningL = 4,
        TurningR = 5
    }

    void Start()
    {
        skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
        animator = GetComponentInChildren<Animator>();

        grandParent = transform.parent?.parent;
        if (grandParent != null)
        {
            PlaceableObject placeableObject = grandParent.GetComponent<PlaceableObject>();
            if (placeableObject != null)
            {
                itemType = placeableObject.item.Type;
                Debug.Log("祖父节点类型: " + itemType);
            }
        }

        animator.SetInteger("State", (int)BirdState.Idle);
        StartCoroutine(RandomBehaviorLoop());
        //会循环播放对应鸟类鸣唱
        AudioManager.instance.PlayBirdsong(this.gameObject.name);
    }

    private void OnEnable()
    {
        //解锁图鉴系统中的鸟类
        if (!mybirdlibrary.birdList.Contains(bird)) mybirdlibrary.birdList.Add(bird);
        bird.isLocked = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFlyingAway)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                //AudioManager.instance.PlayBirdsong("bird1");
                Debug.Log("点击到了鸟类！" + this.gameObject);
                StopAllCoroutines(); // 中断其他行为协程
                StartCoroutine(FlyAway());
                AudioManager.instance.PlayFX("flapping");

            }
        }
    }

    IEnumerator FlyAway()
    {
        isFlyingAway = true;
        animator.SetBool("IsFlyingAway", true);

        animator.CrossFade("prefly", 0f); // 高优先级中断当前动画
        yield return new WaitForSeconds(0.5f);
        animator.CrossFade("fly", 0f); // 飞行动画
        float duration = 3f;
        float time = 0f;
        Vector3 startPos = transform.position;
        // 向右上飞
        Vector3 localFlyDir = new Vector3(1f, 1f, 0f).normalized; // 前上方方向（局部）
        Vector3 worldFlyDir = transform.TransformDirection(localFlyDir); // 转换为世界坐标
        Vector3 endPos = startPos + worldFlyDir * 10f;


        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator RandomBehaviorLoop()
    {
        while (!isFlyingAway)
        {
            yield return new WaitForSeconds(Random.Range(1f*actlevel, 2f * actlevel));

            float eatWeight = itemType == ObjectType.Bowls ? 0.5f : 0.1f;
            float turnWeight = 0.2f;
            float watchWeight = 0.15f;
            float singWeight = itemType == ObjectType.Trees ? 0.7f : 0.3f;

            float total = eatWeight + turnWeight + watchWeight + singWeight;
            float roll = Random.Range(0f, total);

            if (roll < eatWeight)
            {
                animator.SetInteger("State", (int)BirdState.Eating);
                yield return new WaitForSeconds(2f);
            }
            else if (roll < eatWeight + turnWeight)
            {
                yield return StartCoroutine(DoTurnBehavior());
            }
            else if (roll < eatWeight + turnWeight + watchWeight)
            {
                animator.SetInteger("State", (int)BirdState.Watching);
                yield return new WaitForSeconds(2.5f); // 可用循环实现
            }
            else
            {
                animator.SetInteger("State", (int)BirdState.Singing);
                // 单一播放音频
                AudioManager.instance.PlayFX(this.gameObject.name + "_" + Random.Range(1, 7));
                yield return new WaitForSeconds(1f);
                AudioManager.instance.PlayFX(this.gameObject.name + "_" + Random.Range(1, 7));
                yield return new WaitForSeconds(1f);
                AudioManager.instance.PlayFX(this.gameObject.name + "_" + Random.Range(1, 7));
                yield return new WaitForSeconds(1f);
            }

            animator.SetInteger("State", (int)BirdState.Idle);
        }
    }

    IEnumerator DoTurnBehavior()
    {
        animator.SetInteger("State", (int)BirdState.TurningL);
        yield return new WaitForSeconds(0.1f);
        Flip(true);

        animator.SetInteger("State", (int)BirdState.Idle);
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("State", (int)BirdState.TurningL);
        yield return new WaitForSeconds(0.1f);
        Flip(false);
    }

    void Flip(bool faceLeft)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (faceLeft ? -1 : 1);
        transform.localScale = scale;
    }
}
