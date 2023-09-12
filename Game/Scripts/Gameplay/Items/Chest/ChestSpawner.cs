using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public SpriteRenderer Renderer;

    public float RespawnTime = 30;

    private GameObject _chestItem;
    private float _respawnTimer;
    private void Awake()
    {
        Renderer.enabled = false;
    }

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        if (_respawnTimer >= RespawnTime && _chestItem.activeSelf == false)
        {
            Spawn();
        }
        else
        {
            _respawnTimer += Time.deltaTime;
        }
    }

    private void Spawn()
    {
        _chestItem = ItemsPool.instance.GetItem(ItemType.Chest);
        _chestItem.transform.position = transform.position;
        _chestItem.SetActive(true);
        _respawnTimer = 0f;
    }
}
