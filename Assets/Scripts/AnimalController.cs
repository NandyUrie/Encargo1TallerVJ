using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AnimalController : MonoBehaviour
{
    [SerializeField] protected float targetUpdatePeriod;
    [SerializeField] private float targetMaxDistance;
    [Range(0f, 1f)] [SerializeField] private float rotateSensitivity;
    [Range(0f, 1f)] [SerializeField] private float acceleration;
    [SerializeField] protected float maxSpeed;
    [SerializeField] private LayerMask animalMask;
    [SerializeField] protected AnimalType aType;
    [SerializeField] protected float sightDistance = 10f;
    [SerializeField] protected List<AnimalController> animalsOnSight = new List<AnimalController>(10);
    protected float speed;
    [SerializeField] protected float patrolSizeX;
    [SerializeField] protected float patrolSizeZ;
    [SerializeField] protected float fieldOfView = 0.7f;

    protected AnimalController animalTarget;
    [SerializeField] private AnimalType[] prey;
    [SerializeField] private AnimalType[] predator;


    private Rigidbody rigid;
    protected Vector3 target;
    protected float nextTargetUpdate;
    private float currentSpeed;
    


    public enum AnimalType
    {
        Worm,
        Bird,
        Cat
    }

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody>();
        speed = maxSpeed;
    }

    protected virtual void ReactToPrey()
    {
        Debug.LogWarning("Reaccion a presa no definida");
    }

    protected virtual void ReactToPredator()
    {
        Debug.LogWarning("Reaccion a depredador no definida");
    }

    protected virtual void Idle()
    {
        if (Time.time >= nextTargetUpdate)
        {
            target = new Vector3(Random.Range(-patrolSizeX, patrolSizeX), .5f, Random.Range(-patrolSizeZ, patrolSizeZ));
            nextTargetUpdate = Time.time + targetUpdatePeriod;
        }
    }

    protected void UpdateTarget()
    {
        if (animalTarget is null )
        {
            Idle();
        }
        else if (predator.Contains(animalTarget.aType))
        {
            ReactToPredator();
        }
        else if (prey.Contains(animalTarget.aType))
        {
            ReactToPrey();
        }
    }

    protected virtual void Move()
    {
        Vector3 dir = target - transform.position;
        //Debug.Log(dir);
        if (dir.sqrMagnitude <= targetMaxDistance * targetMaxDistance)
        {
            rigid.velocity = Vector3.zero;
            return;
        }


        //transform.forward = dir;
        float prev_angle = transform.rotation.eulerAngles.y;
        float target_angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float new_angle = Mathf.LerpAngle(prev_angle, target_angle, rotateSensitivity);
        
        
        transform.rotation = Quaternion.Euler(0f, new_angle, 0f) ;
        //transform.rotation = Quaternion.Euler(0f, a, 0f);
        //transform.LookAt(target);

        float targetSpeed = 0f;
        if (Vector3.Dot(transform.forward, dir.normalized) > 0.5f)
        {
            targetSpeed = speed;
        }
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration);
        rigid.velocity = transform.forward * currentSpeed;
    }

    protected void FixedUpdate()
    {
        UpdateTarget();
        Move();
        FindOnSight();
    }
    
    protected void FindOnSight()
    {
        FindAnimalsOnRange();
        FilterOnSight(animalsOnSight, fieldOfView);
        if (!animalsOnSight.Contains(animalTarget))
        {
            animalTarget = null;
        }
    }

    protected void FilterOnSight(List<AnimalController> animals, float a)
    {
        for (int i = 0; i < animals.Count; i++)
        {
            AnimalController animal = animals[i];
            Vector3 dir = animal.transform.position - transform.position;
            Vector3 normDir = dir.normalized;
            float dot = Vector3.Dot(transform.forward, normDir);
            if (dot < a)
            {
                animals.RemoveAt(i--);
            }
            else if (predator.Contains(animal.aType) || prey.Contains(animal.aType))
            {
                if(animalTarget is null)
                {
                    animalTarget = animal;
                }
                else if (IsPredator(animal) && IsPrey(animalTarget))
                {
                    animalTarget = animal;
                }
                else if (IsPrey(animal) && IsPredator(animalTarget))
                {
                    continue;
                }
                else if(IsPrey(animal) || IsPredator(animal))
                {
                    float dist = (animal.transform.position - transform.position).sqrMagnitude;
                    float otherDist = (animalTarget.transform.position - transform.position).sqrMagnitude;

                    if(dist < otherDist)
                    {
                        animalTarget = animal;
                    }
                }
            }
        }
    }

    private bool IsPrey(AnimalController animalito)
    {
        return prey.Contains(animalito.aType);
    }

    private bool IsPredator(AnimalController animalito)
    {
        return predator.Contains(animalito.aType);
    }

    protected void FindAnimalsOnRange()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, sightDistance, animalMask);
        animalsOnSight = new List<AnimalController>();

        foreach (Collider coll in colls)
        {
            AnimalController ac = coll.GetComponent<AnimalController>();
            if (ac == this)
                continue;
            animalsOnSight.Add(ac);
        }
    }

    
    public AnimalType GetAnimalType()
    {
        return this.aType;
    }

#if UNITY_EDITOR

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, target);
        Gizmos.color = new Color(0.5f, 0.5f, 0.0f, 0.2f);
        Gizmos.DrawSphere(transform.position, sightDistance);

    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (AnimalController a in animalsOnSight)
            Gizmos.DrawLine(transform.position, a.transform.position);
    }
#endif

}
