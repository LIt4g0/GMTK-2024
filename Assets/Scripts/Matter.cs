using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Matter : MonoBehaviour
{
    [SerializeField] float velocity = 1;
    [SerializeField] Hole hole;
    float limit;
    Transform target;
    MatterSpawner matterSpawner;
    float swirlStrength = 5;
    float vortexStrength = 1000;
    float vortexMultiplier = 3;
    float swirlMultiplier = 3;
    float radius = 15;
    float angle = 0.0f;
    float radiusSubtractor = 1.5f;
    bool collided = false;
    float life = 0.0f;
    float vortexTime = 0.0f;
    float vortexMaxTime = 3.0f;
    
    public void SetStuff(Hole inHole, MatterSpawner inSpawner, float inSwirlStrength, float inVortexStrength, float inVortexMultiplier, float inSwirlMultiplier, float inRadius, float inRadiusSub, Material materialIn)
    {
        matterSpawner = inSpawner;
        hole = inHole;
        target = hole.GetComponent<Transform>();
        limit = hole.limit;
        swirlStrength = inSwirlStrength;
        vortexStrength = inVortexStrength;
        vortexMultiplier = inVortexMultiplier;
        swirlMultiplier = inSwirlMultiplier;
        radius = inRadius;
        radiusSubtractor = inRadiusSub;
        swirlStrength = swirlStrength * (swirlMultiplier * (hole.score*0.2f)) + 4;
        GetComponentInChildren<MeshRenderer>().material = materialIn;
        float scaler = 0.1f + (hole.score *0.035f);
        transform.localScale = new Vector3(scaler,scaler,scaler);
        GetComponent<Spin>().spinRate = Random.Range(-100.0f, 100.0f);
    }

    void Start()
    {
        Vector3 direction = hole.transform.position - transform.position;
        var tangent = Vector3.Cross(direction, Vector3.up).normalized * swirlStrength;
        GetComponent<Rigidbody>().velocity = tangent;
        float distance = Vector3.Distance(transform.position, target.position);
        radius = distance;
        Vector3 targetDir = target.position - transform.position;
        angle = Vector3.Angle(targetDir, transform.forward);
        //Debug.Log(angle);
    }

    void Update()
    {
        if (hole.score >= 40)
        {
            //WinText
            vortexStrength += 300.0f;
            // if (vortexStrength >= 3500.0f)
            // {
            //     vortexStrength = 3500.0f;
            // }

        }
        life += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance >= 100.0f)
        {
            DestroySelf();
        }
        float radiusSubberer;
        float localVortexS = vortexStrength;
        vortexTime = 1.0f;
        //transform.position = Vector3.MoveTowards(transform.position, target.position, velocity*Time.deltaTime);
        if (Input.GetKey(KeyCode.Space) && life > 2.5f)
        {
            if (vortexTime <= vortexMaxTime)
            {
                vortexTime += Time.deltaTime;
            }

            localVortexS = vortexTime * (vortexStrength * (vortexMultiplier));
        }
        //swirlStrength = swirlStrength * (swirlMultiplier * distance);
        Vector3 direction = hole.transform.position - transform.position;
        GetComponent<Rigidbody>().AddForce(direction.normalized * Time.deltaTime * localVortexS);


        if (!collided)
        {
            radiusSubberer = radiusSubtractor * distance;
        }
        else
        {
            //radiusSubberer = -radiusSubtractor * distance*5;
            radiusSubberer = 1;
        }
        //velocity = radiusSubtractor * -distance;


        radius = radius - (radiusSubberer * Time.deltaTime);

    }

    public void Collided()
    {
        collided = true;
    }

    public void DestroySelf()
    {
        matterSpawner.RemoveMatter(gameObject);
        hole.RemoveFromCollision(gameObject);
        Destroy(gameObject);
    }
}
