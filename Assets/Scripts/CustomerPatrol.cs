using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class CustomerPatrol : MonoBehaviour
{
    public float radius;
    public float nextTimer = 4f;
    [SerializeField] GameObject worker;

    NavMeshAgent agent;
    float timer;
    int numPoints;
    List<Vector3> points;

    int counter;
    int state;
    int layermask;
    int found;

    void Start()
    {
        //Initialize
        found = 0;
        points = new List<Vector3>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 10f;
        agent.stoppingDistance = 1f;
        timer = nextTimer;
        numPoints = Random.Range(3, 6);
        Vector3 startingPos = transform.position;
        points.Add(startingPos);

        counter = 1;
        state = 0;
        layermask = 1 << 9; //Set for mask, shifting bits because it is a bitmask

        for(int x = 0; x < numPoints + 1; x++)
        {
            Vector3 newPos = RandomNavSphere(points[x], radius);
            points.Add(newPos);
        }

    } //Sets up patrol routes and initializes values

    // Update is called once per frame
    void Update()
    {  
        Debug.DrawRay(transform.position, transform.forward * 12f, Color.red);//In place to indicate direction
        if (state == 0) //Patrolling State, sets next position after a specific timer
        {
            timer += Time.deltaTime;
            if (timer >= nextTimer)
            {
                agent.SetDestination(points[counter]);
                counter++;
                if (counter == points.Count)
                {
                    counter = 0;
                }
                timer = 0;
            }
        }
        if (state == 1) //Chase state, get remaining distance between worker and customer, slow down if the distance is small to make turns
        {
            agent.SetDestination(worker.transform.position);
        }

        if (found == 0)
        {
            RaycastHit hit; //Information on the object it hits
            if (Physics.Raycast(transform.position, transform.forward, out hit, 12f, layermask)) //If customer sees worker
            {
                found = 1;
                state = 1;
                agent.speed = 20f;
                agent.angularSpeed = 2060f; //angular speed changes
                agent.acceleration = 20f; //acceleration seems to do nothing
                agent.autoBraking = false;
                agent.SetDestination(worker.transform.position);

            } //Increase speed, acceleration
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {


        NavMeshHit navHit;

        int state = 1;
        while (state == 1)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;
            if (NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas))
            {
                return navHit.position;
                //state = 0;
            }
        }

        return new Vector3(0f, 0f, 0f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(0);
        } //have this collision call a different script that signals an end in a certain way
        //this is so that multiple things can end the game the same way
    }

}
