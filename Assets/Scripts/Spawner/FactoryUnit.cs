using UnityEngine;

public class FactoryUnit : Factory<Unit>
{
    [SerializeField] private Transform _spawnPoint;

    public override Unit Create()
    {
        Unit unit = Instantiate(Prefab, _spawnPoint);

        return unit;
    }
}

