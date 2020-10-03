using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Planet[] planets;
    float physicsUpdateTimer = 0;

    public float timescale = 1;

    // Start is called before the first frame update
    void Start()
    {
        
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



        }



    }
}
