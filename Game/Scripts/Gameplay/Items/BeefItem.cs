using System.Collections;
using UnityEngine;

public class BeefItem : Item
{
    [SerializeField] 
    [Range(0, 1f)] 
    private float HealthRecoverPercent = 0.2f;
    private WaitForSeconds wait05 = new(0.5f);
    Coroutine coroutine;
    
    void Awake()
    {
        Initialize();
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        speed = 5;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            if (coroutine == null)
                coroutine = StartCoroutine(ItemAnimation());

            if (isCollided)
                GetBeef();
        }
    }
    
    void GetBeef()
    {
        Player.GetInstance().RecoverHealthPercent(HealthRecoverPercent);
        ItemsPool.instance.ReturnToPool(gameObject, Type);
    }
    
    protected override IEnumerator DisableWithDelay()
    {
        yield return wait05;

        GetBeef();
    }
}
