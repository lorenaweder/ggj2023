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

    void Start()
    {
        print("start");
    }


    void OnCollisionEnter(Collision other)
    {
        print("collision enter");
        if(other.gameObject.tag == "Player"){

            print("other is player");

            if(isAngry){
                //hurt player;
            }

            else{
                // turn angry
                obstacleParent.GetAngry();
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
