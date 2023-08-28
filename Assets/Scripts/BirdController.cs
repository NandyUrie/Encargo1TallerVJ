using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : AnimalController
{
    [SerializeField] private float flyHeight;

    protected override void Start()
    {
        base.Start();
        this.aType = AnimalType.Bird;
    }

    protected override void Move()
    {
        speed = maxSpeed ;
        base.Move();
        transform.position = new Vector3(transform.position.x, flyHeight, transform.position.z);
    }

    protected override void ReactToPrey()
    {
        target = animalTarget.transform.position;
    }

    protected override void ReactToPredator()
    {
        flyHeight = 16f;
        Move();
    }

    protected override void Idle()
    {
        flyHeight = 1f;
        base.Idle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gato")
        {
            transform.position = new Vector3(Random.Range(-patrolSizeX, patrolSizeX), .5f, Random.Range(-patrolSizeZ, patrolSizeZ));
        }
    }

}
