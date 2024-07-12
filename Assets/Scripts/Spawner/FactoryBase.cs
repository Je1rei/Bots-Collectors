using UnityEngine;

public class FactoryBase : Factory<Base>
{
    public override Base Create(Transform transform)
    {
        Base newBase = Instantiate(Prefab);

        newBase.transform.position = transform.position;

        return newBase;
    }
}
