using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsEnemy;

    private playercontroller _playercontroller;
    private float timerespawn;
    [SerializeField] private float starttimerespawn;
    // Start is called before the first frame update
    void Start()
    {
        _playercontroller = GetComponent<playercontroller>();
        timerespawn = starttimerespawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerespawn <= 0)
        {
            timerespawn = starttimerespawn-_playercontroller.getLevel()/10;
            spawner();

        }
        else
        {
            timerespawn -= Time.deltaTime;
        }
    }

    void spawner()
    {
        int r = Random.Range(0, prefabsEnemy.Length);
        var playerpos = _playercontroller.transform.position;
        float pos;
      
       if( Random.Range(0,2)==0)
          pos  = Random.Range(playerpos.x+60, playerpos.x+100);
           else
       {
          pos = Random.Range(playerpos.x-60, playerpos.x-100);
       }

       Instantiate(prefabsEnemy[r], new Vector3(pos, 5, 0), quaternion.identity);
    }
}
