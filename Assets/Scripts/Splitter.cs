using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Enemy {
    [SerializeField]
    private GameObject splitterPrefab;

    public int splitCount = 2;

    public override void Hit() {
        if (splitCount > 0) {
            for (int i = 0; i < 2; i++) {
                GameObject newSplitter = Instantiate(splitterPrefab);
                newSplitter.transform.position = transform.position;
                newSplitter.GetComponent<Splitter>().splitCount = splitCount - 1;
                newSplitter.GetComponent<Rigidbody>().AddForce(-transform.forward * 10, ForceMode.Impulse);
            }
        }

        base.Hit();
    }
}
