using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleParent : MonoBehaviour
{


    public void GetAngry(){

        print("parent is angry");
        //get kids
        //make them all angry

        foreach(var Obstacle in GetComponentsInChildren<Obstacle>()){
        Obstacle.Angry();
        }

        //wait, then make them not angry

        StartCoroutine(AngryRoutine());

    }

    IEnumerator AngryRoutine()
    {
        
        yield return new WaitForSeconds(5);

        foreach(var Obstacle in GetComponentsInChildren<Obstacle>()){
        Obstacle.NotAngry();
        }

    }
}
