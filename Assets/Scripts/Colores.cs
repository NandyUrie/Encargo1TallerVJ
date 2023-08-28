using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEditor;

public class Colores : MonoBehaviour
{

    public GameObject gato;
    public GameObject pajaro;
    public GameObject lombriz;
    public Material materila;

    public Vector3 nextGato;
    public Vector3 nextPajaro;
    public Vector3 nextlombriz;

    public float gatoSpeed = 10;
    public float pajaroSpeed = 6;
    public float lombrizSpeed = 4;

    public bool caminaGato;
    public bool caminaPajaro;
    public bool caminaLombriz;

    void Start()
    {
        gato = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        pajaro = GameObject.CreatePrimitive(PrimitiveType.Cube);
        lombriz = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        gato.name = "Gato";
        pajaro.name = "Pajaro";
        lombriz.name = "Lombriz";

        nextGato = Color(gato);
        nextPajaro = Color(pajaro);
        nextlombriz = Color(lombriz);

        caminaGato = true;
        caminaPajaro = true;
        caminaPajaro = true;
    }

    void Update()
    {
        Deambulando(gato, gatoSpeed, ref nextGato, ref caminaGato);
        Deambulando(pajaro, pajaroSpeed, ref nextPajaro, ref caminaPajaro);
        Deambulando(lombriz, lombrizSpeed, ref nextlombriz, ref caminaLombriz);
    }

    private void Deambulando(GameObject elemento, float speed, ref Vector3 siguiente, ref bool isCaminando)
    {
        if (elemento.transform.position != siguiente && isCaminando)
        {
            isCaminando = false;
            elemento.transform.LookAt(siguiente);
            elemento.transform.position = Vector3.MoveTowards(elemento.transform.position, siguiente, speed * Time.deltaTime);
            //siguiente = new Vector3(Random.Range(-5, 5), .5f, Random.Range(-5, 5));
            //Debug.Log("siguiente" + siguiente);
            isCaminando = true;
        }
        
    }

    public Vector3 Color(GameObject element)
    {
        Vector3 a = new Vector3(Random.Range(-5, 5),1, Random.Range(-5, 5));
        element.transform.position = new Vector3(.5f, .5f, .5f) + a;
        element.transform.parent = this.transform;
        element.GetComponent<Renderer>().material.color = Random.ColorHSV();
        Rigidbody rig = element.AddComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeRotationZ;
        rig.constraints = RigidbodyConstraints.FreezeRotationX;
        a = new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f));
        Debug.Log("a" + a);
        return a;
    }
}

