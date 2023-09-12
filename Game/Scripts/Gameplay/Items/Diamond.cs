using System.Collections;
using UnityEngine;

public class Diamond : Item
{
    Coroutine coroutine;
    private WaitForSeconds wait05 = new(0.5f);
    void Awake()
    {
        Initialize();
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        speed = 10;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            if (coroutine == null)
                coroutine = StartCoroutine(ItemAnimation());

            if (isCollided)
                GetDiamond();
        }
    }
    
    void GetDiamond()
    {
        GameProgress.GetData().AddDiamonds(1);
        GameProgress.Save();
        ItemsPool.instance.ReturnToPool(gameObject, Type);
        
    }
    
    protected override IEnumerator DisableWithDelay()
    {
        yield return wait05;

        GetDiamond();
    }
}
