public class Lightning : PlayerWeapon
{
    void OnEnable()
    {
        StartCoroutine(StartDestroy());
        transform.position =  EnemySpawner.GetInstance().GetRandomPositionInScreen();
    }
}