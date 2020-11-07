using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.SceneManagement;


//
// Av: John Boman, Martin Quach
// c18johbo, a18marqu
public class Controller : MonoBehaviour
{

    public Planet[] planets;
    public float timescale = 1;

    float physicsUpdateTimer = 0;

    bool gravityToggle = true;
    float gravityMultiplier = 1;

    float density;

    ////Other
    private ParticleObject[] pObjs;
    void Start()
    {
        pObjs = new ParticleObject[planets.Length];
        for (int i = 0; i < planets.Length; i++) {
            pObjs[i] = planets[i].GetComponent<ParticleObject>();
        }

        density = planets[0].density;
    }


    void Update() {

        // Check the controls 
        Controls();

        // Update Physics
        UpdatePhysics();
    }

    private void UpdatePhysics() {

        //Fixed Physics update timer
        physicsUpdateTimer += Time.deltaTime;
        if (physicsUpdateTimer > Constants.STEPVALUE) {
            physicsUpdateTimer -= Constants.STEPVALUE; // :/

            if (gravityToggle) {
                // Calculate the force for all planets (moon and earth)
                foreach (var planet in planets) {

                    planet.CalculateForce(planets, Constants.STEPVALUE);
                }
            }
            // Apply the force for all planets (moon and earth)
            foreach (var planet in planets) {

                planet.UpdatePositon(Constants.STEPVALUE);
            }

            //Other
            // Calculate the collisions for the particles
            pObjs[0].CalculateCollisions(planets, Constants.STEPVALUE); // only for the earth

        }
    }

    private void Controls() {
        // Turn off gravity
        if (Input.GetKeyDown(KeyCode.Space))
            gravityToggle = !gravityToggle;

        //Mult gravity
        if (Input.GetKeyDown(KeyCode.J))
            gravityMultiplier *= 2;

        //Divide gravity
        if (Input.GetKeyDown(KeyCode.H))
            gravityMultiplier /= 2;

        //Update density
        planets[0].SetDensity(density * gravityMultiplier);

        //Reload Scene
        if (Input.GetKeyDown(KeyCode.T))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // Turn off the program lul (Will crash unity also)
        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.I))
            Application.ForceCrash(0);

        //Change TimeScale in editor
        Time.timeScale = timescale;
    }



    [Header("Calculations")]
    public float orbitalVelocity;
    public float orbitalVelocity1;

    void OnValidate() {

        //Get Distance from this planet to the other
        Vector3 distVector = planets[0].transform.position - planets[1].transform.position;
        float distSquared = Vector3.SqrMagnitude(distVector); ;

        // Calculate the orbit velocity for the earth and moon (The velocity needed to orbit the other planet)
        //http://www.phys.ufl.edu/courses/phy2048/archives/fall06/lectures/11-08EscapeVelocity.pdf
        orbitalVelocity = Mathf.Sqrt((Constants.G * planets[0].mass * 10000000) / Mathf.Sqrt(distSquared));
        orbitalVelocity1 = Mathf.Sqrt((Constants.G * planets[1].mass * 10000000) / Mathf.Sqrt(distSquared));

    }




}
