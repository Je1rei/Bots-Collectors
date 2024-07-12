public class SpawnerApple : Spawner<Apple> 
{
    private void Start()
    {
        InvokeRepeating(nameof(GetObject), 0.0f, GetRepeatRate());
    }
}