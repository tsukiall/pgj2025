using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveTracker : MonoBehaviour {
    [SerializeField]
    private float rotation;
    [SerializeField]
    private float distanceScale;

    [SerializeField]
    private Image right;
    [SerializeField]
    private Image left;

    private Transform player;
    private GameManager gameManager;
    private float closestPickupDistance;
    private SmallObjective closestPickup;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update() {
        transform.position = player.position;

        foreach (SmallObjective objective in gameManager.GetSmallObjectives()) {
            float pickupDistance = (objective.transform.position - transform.position).sqrMagnitude;

            if (closestPickup == null || pickupDistance <= closestPickupDistance) {
                closestPickup = objective;
            }

            if (objective == closestPickup) {
                closestPickupDistance = pickupDistance;
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(closestPickup.transform.position - transform.position), Time.deltaTime * rotation);
        right.fillAmount = 0.7f - closestPickupDistance * distanceScale;
        left.fillAmount = 0.7f - closestPickupDistance * distanceScale;
    }
}
