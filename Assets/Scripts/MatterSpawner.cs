using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatterSpawner : MonoBehaviour
{
    //[SerializeField] Transform placementTransform;
    [SerializeField] float radius = 15.0f;
    [SerializeField] Matter[] toSpawn;
    [SerializeField] float safeDistance = 10f;
    [SerializeField] List<GameObject> allMatter = new List<GameObject>();
    [SerializeField] Hole hole;
    [SerializeField] float swirlStrength = 1;
    [SerializeField] float vortexStrength = 1000;
    [SerializeField] float vortexMultiplier = 3;
    [SerializeField] float swirlMultiplier = 3;
    [SerializeField] float radiusSubtractor = 3;
    [SerializeField] Material[] materials;
    

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //Spawn();
        }

        if (allMatter.Count < 5 && hole.score <= 300.0f)
        {
            Spawn();
        }
    }

    public void RemoveMatter(GameObject toRemove)
    {
        allMatter.Remove(toRemove);
    }

    void Spawn()
     {
        bool posOK = false;
        Transform spawnPoint = transform;

        for (int j = 0; j < 5; j++)
        {
            spawnPoint.position = RandomCircle(new Vector3(0, 0, 0), radius);

            posOK = CheckDistance(spawnPoint);
            if (!posOK)
            {
                // trying again until max attempts reached
            }
            else
            {
                break;
            }
        }

        if (posOK)
        {
            Matter randomAsteroid = toSpawn[Random.Range(0, toSpawn.Length)];
            var newMatter = Instantiate(randomAsteroid, spawnPoint.position, transform.rotation).gameObject;
            allMatter.Add(newMatter);
            newMatter.GetComponent<Matter>().SetStuff(hole, this, swirlStrength,vortexStrength,vortexMultiplier,swirlMultiplier, radius, radiusSubtractor, materials[Random.Range(0,9)]);
        }
        else
        {
            print("Failed to spawn nr: " );
        }
     }


    Vector3 RandomCircle(Vector3 center, float _radius)
    {
        float angWidth = 360;
        float ang = Random.value * angWidth;
        float twistAng = -(angWidth / 2);
        Vector3 pos;
        pos.x = center.x + _radius * Mathf.Sin((ang + twistAng) * Mathf.Deg2Rad);
        pos.y = center.y; 
        pos.z = center.z +_radius * Mathf.Cos((ang + twistAng) * Mathf.Deg2Rad);

        return pos;
    }

    private bool CheckDistance(Transform spawnPoint)
    {
        if (allMatter.Count >= 0)
        {
            foreach (GameObject aster in allMatter)
            {
                if (aster != null)
                {
                    if (Vector3.Distance(spawnPoint.position, aster.transform.position) < safeDistance)
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            return true;
        }
        return true;
    }
}
