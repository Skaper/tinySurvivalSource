using System.Collections;
using UnityEngine;

public class Crystal : Item
{
    [SerializeField] CrystalData crystalData;
    
    private WaitForSeconds wait05 = new(0.5f);
    private WaitForSeconds waitLifeTime;
    private Coroutine lifeTimeCoroutine;
    Coroutine coroutine;
    int expValue;

    void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        lifeTimeCoroutine = StartCoroutine(LifeTime());
    }

    private void OnDisable()
    {
        StopCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return waitLifeTime;
        ItemsPool.instance.ReturnToPool(gameObject, Type);
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        expValue = crystalData.GetExpValue();
        spriteRenderer.color = crystalData.GetColor();
        waitLifeTime = new(crystalData.GetLifeTime());
        speed = 7;
    }
    
    public int GetExpValue()
    {
        return expValue;
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            if (coroutine == null)
                coroutine = StartCoroutine(ItemAnimation());

            if (isCollided)
                GetCrystal();
        }
    }

    protected override IEnumerator DisableWithDelay()
    {
        yield return wait05;

        GetCrystal();
    }
    
    void GetCrystal()
    {
        player.GetComponent<Level>().GetExp((int)(expValue * Player.GetInstance().GetExpAdditional() / 100f));
        ItemsPool.instance.ReturnToPool(gameObject, Type);
    }
    
    
}
