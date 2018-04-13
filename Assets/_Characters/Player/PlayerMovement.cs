using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI; //TODO consider re-wiring

namespace RPG.Characters
{

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        CameraRaycaster cameraRaycaster = null;
        AICharacterControl aICharacterControl = null;
        GameObject walkTarget = null;

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            aICharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");
            
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                aICharacterControl.SetTarget(enemy.transform);
            }
        }

        private void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                //navigate to point on the ground
                walkTarget.transform.position = destination;
                aICharacterControl.SetTarget(walkTarget.transform);
            }
        }
    }

}
