using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : AnimalController
{
    [SerializeField] private float speedFreq = 1f;
    Vector3 scale;
    
    protected override void Start()
    {
        base.Start();
        scale = transform.localScale;
        this.aType = AnimalType.Worm;
    }

    protected override void Move()
    {
        speed = maxSpeed * (Mathf.Sin(Time.time * speedFreq) / 2f + 0.5f);
        transform.localScale = new Vector3(scale.x, scale.y, scale.z/2f + scale.z / 2f * (Mathf.Cos(Time.time * speedFreq) / 2f + 0.5f));
        base.Move();

    }

    protected override void ReactToPredator()
    {
        Idle();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pajaro")
        {
            transform.position = new Vector3(Random.Range(-patrolSizeX, patrolSizeX), .5f, Random.Range(-patrolSizeZ, patrolSizeZ));
        }
    }
}
