using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Planet[] planets;
    float physicsUpdateTimer = 0;
    public float timescale = 1;


    //Other
    public ParticleObject[] pObjs;


    void Start()
    {
        pObjs = new ParticleObject[planets.Length];
        for (int i = 0; i < planets.Length; i++) {
            pObjs[i] = planets[i].GetComponent<ParticleObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timescale;


        //Fixed Physics update timer
        physicsUpdateTimer += Time.deltaTime;
        if (physicsUpdateTimer > Constants.STEPVALUE) {
            physicsUpdateTimer -= Constants.STEPVALUE; // :/


            foreach (var planet in planets) {

                planet.CalculateForce(planets, Constants.STEPVALUE);
            }

            //Update graphic
            foreach(var planet in planets) {

                planet.UpdatePositon(Constants.STEPVALUE);
            }


            foreach (var obj in pObjs) {
                obj.CalculateCollisions(planets, Constants.STEPVALUE);
            }



        }



    }
}
