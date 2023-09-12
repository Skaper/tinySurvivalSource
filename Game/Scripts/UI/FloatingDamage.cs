using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingDamage : ParticleItem
{
    [SerializeField] private TextMeshPro label;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private float showTime = 0.3f;

    private WaitForSecondsRealtime _wait;

    private void Awake()
    {
        _wait = new WaitForSecondsRealtime(showTime);
    }

    public void Show(string value, Vector3 position)
    {
        
        label.text = value;
        transform.position = position + offset;
        StartCoroutine(InactiveText());
        
    }

    IEnumerator InactiveText()
    {
        yield return _wait;

        ParticleItemsPool.instance.ReturnToPool(gameObject, ParticleItemType.DamageText);
        gameObject.SetActive(false);
    }
}
