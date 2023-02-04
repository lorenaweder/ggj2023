using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    public bool isAngry;

    [SerializeField] Material safeMat;
    [SerializeField] Material angryMat;

    [SerializeField] ObstacleParent obstacleParent;

    [SerializeField] bool catTrigger;
    [SerializeField] ParticleSystem activeRoots;

    [SerializeField] GameObject mushroomHat;
    [SerializeField] GameObject angryDots;

    private Vector3 angryScale = new Vector3 (1, 1, 1);
    private Vector3 safeScale = new Vector3 (0.5f, 0.5f, 0.5f);

    void Start()
    {
        angryDots.transform.DOScale(safeScale, 1);
    }


    void OnCollisionEnter(Collision collision)
    {
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
        mushroomHat.GetComponent<MeshRenderer> ().material = angryMat;
        angryDots.transform.DOScale(angryScale, 1);
    }

    public void NotAngry(){
        isAngry = false;
        mushroomHat.GetComponent<MeshRenderer> ().material = safeMat;
        angryDots.transform.DOScale(safeScale, 1);
    }
}
