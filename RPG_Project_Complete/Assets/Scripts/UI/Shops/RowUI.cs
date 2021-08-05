using RPG.Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
    public class RowUI : MonoBehaviour
    {
        
        [SerializeField] Image iconField = null;
        [SerializeField] TextMeshProUGUI nameField = null;
        [SerializeField] TextMeshProUGUI availabilityField = null;
        [SerializeField] TextMeshProUGUI priceField = null;
        [SerializeField] TextMeshProUGUI qauntityField = null;
        
        Shop currentShop = null;
        ShopItem item = null;
        
        public void Setup(Shop currentShop, ShopItem item)
        {
            this.currentShop = currentShop;
            this.item = item;
            iconField.sprite = item.GetIcon();
            nameField.text = item.GetName();
            availabilityField.text = $"{item.GetAvailability()}";
            priceField.text = $"${item.GetPrice():N2}";
            qauntityField.text = $"{item.GetQuantityInTransaction()}";
        }

        public void Add()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), 1);
        }

        public void Remove()
        {
            currentShop.AddToTransaction(item.GetInventoryItem(), -1);
        }
    }
}
