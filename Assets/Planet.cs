using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// Av: John Boman, Martin Quach
// c18johbo, a18marqu
public class Planet : MonoBehaviour
{


    // We scale down the radius of the planets by 10^6 (m)
    // Then we can calculate the new mass with this thing: (4/3)*("insert group name")* radius^3 * density
    // based on the density of each planet




    public float mass;
    public float radius;
    public float density; // g/cm^3
    public Vector3 curVelocity;


    //Calculate all forces affecting this planet 
    public void CalculateForce(Planet[] planets, float timeStep) {
        foreach (var planet in planets) {
            if (planet != this) {


                //Get Distance from this planet to the other
                Vector3 distVector = planet.transform.position - transform.position;
                float distSquared = Vector3.SqrMagnitude(distVector);


                // The gravition law scaled by 10^7 so it's not so slow...
                // F = G*((m1*m2)/r^2)
                float forceMagnitude = Constants.G * ((this.mass * planet.mass) / distSquared) * 10000000;
                Vector3 force = distVector.normalized * forceMagnitude;

                //get acceleration from force (we could have just have removed it before, above but meh)
                Vector3 acceleration = force / mass;

                //add acceleration to current Velocity
                curVelocity += acceleration * timeStep;

            }
        }
    }


    //Update Positon of this planet
    public void UpdatePositon(float timeStep) {
        transform.position += curVelocity * timeStep;
    }


    public void SetDensity(float density) {
        this.density = density;

        //Calculate mass from the radius
        //note g/cm^3 -> kg/m^3 density (hence * 1000)
        mass = (4 / 3) * Mathf.PI * Mathf.Pow(radius, 3) * density * 1000;
    }


    private void OnValidate() {
        transform.localScale = Vector3.one * radius;

        SetDensity(density);
    }
}
