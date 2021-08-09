using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/Delayed Click", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture = null;
        [SerializeField] Vector2 cursorHotspot = default;
        [SerializeField] LayerMask layerMask = default;
        [SerializeField] float areaOfEffectRadius = 0f;
        [SerializeField] Transform targetingPrefab = null;

        Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            playerController.enabled = false;
            if (targetingPrefabInstance == null)
            {
                targetingPrefabInstance = Instantiate(targetingPrefab);
            }
            else
            {                
                targetingPrefabInstance.gameObject.SetActive(true);
            }

            targetingPrefabInstance.localScale = new Vector3(areaOfEffectRadius * 2, 1f, areaOfEffectRadius * 2);

            while (!data.IsCancelled())
            {
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Escape))
                {
                    data.Cancel();
                    break;
                }

                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
                RaycastHit raycastHit = new RaycastHit();

                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    targetingPrefabInstance.position = raycastHit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        // Wait until the player stops clicking to avoid moving.
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        data.SetTargetedPoint(raycastHit.point);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        break;
                    }
                }
                yield return null;
            }
            targetingPrefabInstance.gameObject.SetActive(false);
            playerController.enabled = true;
            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, areaOfEffectRadius, Vector3.up, 0);

            foreach (RaycastHit hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}