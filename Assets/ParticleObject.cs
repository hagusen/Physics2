using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObject : MonoBehaviour
{

    //inte kommenterad men detta fil har inget med uppgiften att göra bara lite roligt  


    [Min(float.Epsilon)]
    public float resolution = 2;
    public Vector3 size;

    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public ParticleSystem psPrefab;

    private void Start() {
        FindAllPointsInASphere();
    }

    public List<Vector3> points = new List<Vector3>();

    void FindAllPointsInASphere() {

        Vector3Int sizes = new Vector3Int {
            x = (int)(transform.localScale.x * resolution),
            y = (int)(transform.localScale.y * resolution),
            z = (int)(transform.localScale.z * resolution)
        };


        //List<Vector3> points = new List<Vector3>();

        for (int x = -sizes.x / 2; x < sizes.x / 2; x++) {
            for (int y = -sizes.y / 2; y < sizes.y / 2; y++) {
                for (int z = -sizes.z / 2; z < sizes.z / 2; z++) {
                    if (Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2) <= Mathf.Pow(sizes.x / 2, 2)) {// if point is in the sphere

                        points.Add(Vector3.right * x + Vector3.up * y + Vector3.forward * z);
                    }
                }
            }
        }

        StartCoroutine(VisulizePoints(points));

    }


    IEnumerator VisulizePoints(List<Vector3> points) {

        int psNeeded = Mathf.CeilToInt((float)points.Count / (float)psPrefab.main.maxParticles);
        psNeeded = psNeeded - particleSystems.Count;

        for (int i = 0; i < psNeeded; i++) {
            var ins = Instantiate(psPrefab, transform, false);
            var mod = ins.main;
            mod.startSizeMultiplier = (1 / resolution) * Random.Range(.9f,1.1f);
            //
            particleSystems.Add(ins);
        }
        yield return new WaitForSeconds(1); // wait for particles to spawn

        foreach (var sys in particleSystems) {
            var d = sys.emission;
            d.enabled = false;
        }

        ParticleSystem.Particle[][] particles = new ParticleSystem.Particle[particleSystems.Count][];
        for (int i = 0; i < particles.Length; i++) { // Get Particles
            particles[i] = new ParticleSystem.Particle[psPrefab.main.maxParticles];
            particleSystems[i].GetParticles(particles[i]);
        }


        int[] psIndex = new int[particleSystems.Count];
        for (int i = 0; i < psIndex.Length; i++) {
            psIndex[i] = 0;
        }
        int psCounter = 0;
        for (int i = 0; i < points.Count; i++) {

            particles[psCounter][psIndex[psCounter]].position = points[i] / resolution;

            psIndex[psCounter]++;
            psCounter++;
            if (psCounter >= particleSystems.Count) {
                psCounter = 0;
            }
        }

        for (int i = 0; i < particles.Length; i++) { // Set Particles
            particleSystems[i].SetParticles(particles[i]);
        }



        yield return null;
    }

    public void CalculateCollisions(Planet[] planets, float TimeStep) {

        foreach (var other in planets) {
            if (other.transform != transform) {

                //Debug.LogWarning(gameObject.name + "332" + other.name);

                for (int i = 0; i < particleSystems.Count; i++) {

                    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystems[i].particleCount];
                    particleSystems[i].GetParticles(particles);

                    for (int k = 0; k < particles.Length; k++) {

                        float dist = Vector3.Distance(other.transform.position, particles[k].position + transform.position) - other.transform.localScale.x;
                        //Debug.Log(dist+ gameObject.name);
                        if (0 > dist){

                            particles[k].velocity = Vector3.Lerp(other.curVelocity.normalized, -other.curVelocity.normalized, Random.value) + new Vector3(Random.value, Random.value, Random.value) * (Random.value - .5f);


                        }
                    }

                    particleSystems[i].SetParticles(particles);
                }
            }
        }

    }







}
