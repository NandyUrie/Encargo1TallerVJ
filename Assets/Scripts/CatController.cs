using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : AnimalController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.aType = AnimalType.Cat;
    }

    protected override void ReactToPrey()
    {
        target = animalTarget.transform.position;
    }

    protected override void Move()
    {
        speed = maxSpeed;
        base.Move();
    }
}
