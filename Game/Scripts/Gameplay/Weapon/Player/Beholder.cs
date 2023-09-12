using UnityEngine;

public class Beholder : PlayerWeapon
{
    private Transform player;
    const float speed = 200f;

    private void Start()
    {
        player = Player.instance.transform;
    }

    private void Update()
    {
        transform.RotateAround(player.transform.position, new Vector3(0f, 0f, 1f), speed * Time.deltaTime);
        transform.Rotate(new Vector3(0f, 0f, -1f), speed * Time.deltaTime);
    }
}