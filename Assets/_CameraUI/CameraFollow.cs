﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour {

        GameObject player;
	
        void Start ()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            print(player);
        }

	    void LateUpdate () {
            transform.position = player.transform.position;
	    }
    }
}
