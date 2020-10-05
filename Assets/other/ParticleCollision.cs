using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ParticleSystemJobs;

 [ExecuteInEditMode]
public class ParticleCollision : MonoBehaviour
{

    ParticleSystem ps;


    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[ps.particleCount];
        ps.GetParticles(p);


        for (int i = 0; i < p.Length; i++) {
            for (int k = 0; k < p.Length; k++) {



                float dist = Vector3.Distance(p[i].position, p[k].position) - (p[i].GetCurrentSize(ps) + p[k].GetCurrentSize(ps));

                if (0 > dist) {

                    p[i].position = p[i].position + Vector3.up * p[i].GetCurrentSize(ps) * p[k].GetCurrentSize(ps);


                }
            }
        }

        ps.SetParticles(p);
    }
}
