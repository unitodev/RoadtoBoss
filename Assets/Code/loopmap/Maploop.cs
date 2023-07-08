using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maploop : MonoBehaviour
{
    private Camera maincam;

    private float startpos;
    // Start is called before the first frame update
    void Start()
    {
        maincam=Camera.main;
        startpos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (startpos - maincam.transform.position.x < -120)
        {
            transform.position = new Vector3(startpos+180, 0, 0);
            startpos = transform.position.x;
        }
        if (startpos - maincam.transform.position.x > 120)
        {
            transform.position = new Vector3(startpos-180, 0, 0);
            startpos = transform.position.x;
        }
       

      
        
    }
}
