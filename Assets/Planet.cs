using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{


    // We scale down the radius of the planets by 10^6
    // Then we can calculate the new mass with this thing: (4/3)*("insert group name")* radius^3 * density
    // based on the density of the planet




    public float mass;
    public float radius;
    public float density; // g/cm^3
    public Vector3 curVelocity;


    public float orbitalSpeed;

    public void CalculateForce(Planet[] planets, float timeStep) {
        foreach (var planet in planets) {
            if (planet != this) {

                

                Vector3 distVector = planet.transform.position - transform.position;
                float distSquared = Vector3.SqrMagnitude(distVector); ;

                // F = G*((m1*m2)/r^2)
                float forceMagnitude = Constants.G * ((this.mass * planet.mass) / distSquared) * 10000000;
                Vector3 force = distVector.normalized * forceMagnitude;

                //get acceleration from force (we could just removed mass above but meh)
                Vector3 acceleration = force / mass;
                //add acceleration to current Velocity
                curVelocity += acceleration * timeStep;

                //http://www.phys.ufl.edu/courses/phy2048/archives/fall06/lectures/11-08EscapeVelocity.pdf
                orbitalSpeed = Mathf.Sqrt((Constants.G * planet.mass * 10000000) / Mathf.Sqrt(distSquared));

            }
        }
    }



    public void UpdatePositon(float timeStep) {

        transform.position += curVelocity * timeStep;
    }





    private void OnValidate() {
        transform.localScale = Vector3.one * radius;

        

        //note g/cm^3 -> kg/m^3 density 
        mass = (4 / 3) * Mathf.PI * Mathf.Pow(radius, 3) * density * 1000;
    }
}
