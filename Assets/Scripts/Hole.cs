using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{
    [SerializeField] List<GameObject> colliding = new List<GameObject>();
    [SerializeField] public float limit = 2;
    HoleMesh holeMesh;
    Animator anim;
    [SerializeField] GameObject _camera;
    [SerializeField] GameObject BG;
    [SerializeField] public float score;
    [SerializeField] float camStartDist = 40.0f;
    Spin spin;
    public float scaler;
    [SerializeField] GameObject eye;
    [SerializeField] float eyeShowTime = 1.0f;
    [SerializeField] float clickCD = 1.0f;
    float eyeTimer;
    float clickCDTimer;
    [SerializeField] float suckCost = 0.2f;
    float suckCostCurrent;
    float suskCostMax = 2.0f;
    bool bWon = false;

    [SerializeField] TextMeshProUGUI textmesh;
    [SerializeField] TextMeshProUGUI winText;
    
    void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
            Gizmos.color = Color.red;

            //min radius gizmo
            Gizmos.DrawWireSphere(Vector3.zero, limit);
            //Debug.Log("Test");
        #endif
    }

    void Awake()
    {
        holeMesh = GetComponent<HoleMesh>();
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        _camera.transform.position = new Vector3(0, camStartDist,0);
        //spin = GetComponentInChildren<Spin>();
        transform.localScale = new Vector3(1,1,1);
        textmesh.text = "Mass: 0";
    }

    void Update()
    {
        //Debug.Log(eyeTimer);
        if (score >= 200 && !bWon)
        {
            bWon = true;
            Invoke("WinText", 2.0f);
        }
        textmesh.text = "Mass: " + score;
        if (score >= 40.0f)
        {
            textmesh.text = "CRITICAL MASS REACHED!!";
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }

        if (eyeTimer > 0.0f)
        {
            eye.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            eye.GetComponent<MeshRenderer>().enabled = false;
        }

        eyeTimer -= Time.deltaTime;


        float scaler = 1 + score * 0.1f;
        transform.localScale = new Vector3(scaler, scaler, scaler);
        clickCDTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {

            anim.Play("Swallow");

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {

            anim.Play("Idle");

        }
        HoldSpace();



    }

    public void WinText()
    {
        textmesh.text = "Mass: 99999999999999999999999";
        winText.gameObject.SetActive(true);
        //Invoke("MenuSpawn",);
    }

    void MenuSpawn()
    {

    }

    private void HoldSpace()
    {

        suckCostCurrent = suckCost;
        if (Input.GetKey(KeyCode.Space))
        {
            
            if (score > 0.1f)
            {
                score -= suckCostCurrent * Time.deltaTime;
            }
            Debug.Log("Button Clicked");
            if (suckCostCurrent <= suskCostMax)
            {
                suckCostCurrent += Time.deltaTime;
            }
            if (colliding[0].gameObject != null)
            {
                clickCDTimer = clickCD;
                GameObject temp = colliding[0];
                colliding.RemoveAt(0);
                temp.GetComponent<Matter>().DestroySelf();
                score = score + 1;
                eyeTimer = eyeShowTime;
            }


            

        }
        if (colliding[0].gameObject != null)
        {
            clickCDTimer = clickCD;
            GameObject temp = colliding[0];
            colliding.RemoveAt(0);
            temp.GetComponent<Matter>().DestroySelf();
            score = score + 1;
            eyeTimer = eyeShowTime;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        
        //Debug.Log(other);
        colliding.Add(other.gameObject);
        other.GetComponent<Matter>().Collided();
    }

        void OnTriggerExit(Collider other)
    {
        
        //Debug.Log(other);
        colliding.Remove(other.gameObject);
        //other.GetComponent<Matter>().Collided();
    }

    public void RemoveFromCollision(GameObject gameObject)
    {
        Debug.Log("Remove self");
        colliding.Remove(gameObject);
        //eyeTimer = eyeShowTime;
    }
}
