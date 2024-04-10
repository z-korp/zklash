using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepPatrol : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;
    public SpriteRenderer sheep;
    private Transform target;
    private int destinationPoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        target = waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        // Position cible -  Position actuelle, donc on sait la direction dans laquelle se déplacer pour se rendre au prochain waypoint
        Vector3 dir = target.position - transform.position;

        // On déplace le personnage en direction de la cible, Space.world permet de faire le mouvement dans le monde entier, Space.self permet de faire le mouvement dans le même plan que le personnage
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // On vérifie si le personnage est arrivé au prochain waypoint, si oui on passe au prochain waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            // Si le personnage est arrivé au dernier waypoint, on le remet au début du tableau % permet de reccupérer le reste de la division ex: 5 % 2 = 1
            destinationPoint = (destinationPoint + 1) % waypoints.Length;
            target = waypoints[destinationPoint];
            sheep.flipX = !sheep.flipX;
        }
    }

}