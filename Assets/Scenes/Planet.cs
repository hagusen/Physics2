using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{


    // We scale down the radius of the planets by 10^6
    // Then we can calculate the new mass with this thing: (4/3)*("insert group name")* radius^3 * density  




    public float mass;
    public float radius;
    public float density; // g/cm^3








    private void OnValidate() {
        transform.localScale = Vector3.one * radius;

        //note g/cm^3 -> kg/m^3 density 
        mass = (4 / 3) * Mathf.PI * Mathf.Pow(radius, 3) * density * 1000;
    }
}
