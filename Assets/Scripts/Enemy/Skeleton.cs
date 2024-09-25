using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonProperty : CharacterAttribute
{
    public SkeletonProperty(int level = 1) : base(level, 100, 0, 100, 10, 5, 5)
    {
    }
}

public class Skeleton : Enemy 
{
    public Skeleton() : base("Skeleton")
    {
        attr = new SkeletonProperty();
    }

    protected override void Start()
    {
        base.Start();
    }

}
