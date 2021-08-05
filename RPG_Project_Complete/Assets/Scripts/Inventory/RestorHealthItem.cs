using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Inventory
{
    [CreateAssetMenu(menuName = ("GameDevTV/GameDevTV.UI.InventorySystem/Health Action Item"))]
    public class RestorHealthItem : ActionItem
    {
        [SerializeField] float healthAmount = 0f;

        public override void Use(GameObject user)
        {
            Debug.Log("Eating...");
            user.GetComponent<Health>().Heal(healthAmount);
        }
    }
}
