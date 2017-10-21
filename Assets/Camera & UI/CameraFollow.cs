using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    GameObject player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        print(player.ToString());
	}

    private void LateUpdate()
    {
        this.transform.position = player.transform.position;
    }
}
