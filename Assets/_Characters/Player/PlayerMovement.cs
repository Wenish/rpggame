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

        [SerializeField]
        const int walkableLayerNumber = 8;

        [SerializeField]
        const int enemyLayerNumber = 9;

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            aICharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
        }

        void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        {
            switch (layerHit)
            {
                case enemyLayerNumber:
                    //navigate to the enemy
                    GameObject enemy = raycastHit.collider.gameObject;
                    aICharacterControl.SetTarget(enemy.transform);
                    break;
                case walkableLayerNumber:
                    //navigate to point on the ground
                    walkTarget.transform.position = raycastHit.point;
                    aICharacterControl.SetTarget(walkTarget.transform);
                    break;
                default:
                    Debug.LogWarning("Don't know how to handle mouse click");
                    return;

            }

        }
    }

}
