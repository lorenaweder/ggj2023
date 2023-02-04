using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool isAngry;

    [SerializeField] Material safeMat;
    [SerializeField] Material angryMat;

    [SerializeField] ObstacleParent obstacleParent;

    [SerializeField] bool catTrigger;
    [SerializeField] ParticleSystem activeRoots;

    void Start()
    {
        print("start");
    }


    void OnCollisionEnter(Collision collision)
    {
        print("collision enter");
        var wiggleCat = collision.gameObject.GetComponent<WiggleCat>();
        if(wiggleCat != null)
        {
            wiggleCat.TakeHit(new CatHitInfo() { Collision = collision, IsDangerousHit = isAngry });

            if (!isAngry)
            {
                // turn angry
                obstacleParent.GetAngry();
                activeRoots.Play();
            }
        }
    }

    public void Angry(){
        isAngry = true;
        gameObject.GetComponent<MeshRenderer> ().material = angryMat;
    }

    public void NotAngry(){
        isAngry = false;
        gameObject.GetComponent<MeshRenderer> ().material = safeMat;
    }
}
