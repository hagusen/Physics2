using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Planet[] planets;
    float physicsUpdateTimer = 0;
    public float timescale = 1;


    //Other
    private ParticleObject[] pObjs;


    void Start()
    {
        pObjs = new ParticleObject[planets.Length];
        for (int i = 0; i < planets.Length; i++) {
            pObjs[i] = planets[i].GetComponent<ParticleObject>();
        }
    }

    // Update is called once per frame
    void Update() {
        Time.timeScale = timescale;


        //Fixed Physics update timer
        physicsUpdateTimer += Time.deltaTime;
        if (physicsUpdateTimer > Constants.STEPVALUE) {
            physicsUpdateTimer -= Constants.STEPVALUE; // :/


            foreach (var planet in planets) {

                planet.CalculateForce(planets, Constants.STEPVALUE);
            }

            //Update graphic
            foreach (var planet in planets) {

                planet.UpdatePositon(Constants.STEPVALUE);
            }



            // Calculate the collisions for the particles
            foreach (var obj in pObjs) {
                obj.CalculateCollisions(planets, Constants.STEPVALUE);
            }



        }
    }
    [Header("Calculations")]
        public float orbitalVelocity;
        public float orbitalVelocity1;

    void OnValidate() {


        Vector3 distVector = planets[0].transform.position - planets[1].transform.position;
        float distSquared = Vector3.SqrMagnitude(distVector); ;

        // F = G*((m1*m2)/r^2)
        float forceMagnitude = Constants.G * ((planets[1].mass * planets[0].mass) / distSquared) * 10000000;
        Vector3 force = distVector.normalized * forceMagnitude;

        //get acceleration from force (we could just removed mass above but meh)
        Vector3 acceleration = force / planets[1].mass;


        //http://www.phys.ufl.edu/courses/phy2048/archives/fall06/lectures/11-08EscapeVelocity.pdf
        orbitalVelocity = Mathf.Sqrt((Constants.G * planets[0].mass) / Mathf.Sqrt(distSquared));
        orbitalVelocity1 = Mathf.Sqrt((Constants.G * planets[1].mass) / Mathf.Sqrt(distSquared));


        // bättre kamera
        // interaktivt!
        // stäng av gravitions kraften
        // stäng av månens velocity?
        // öka gravitions kraften?
    }
}
