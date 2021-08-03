using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceField = null;
        
        Purse playerPurse = null;

        void Start() 
        {
            playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();

            if (playerPurse != null)
            {
                playerPurse.onChange += RefreshUI;
            }

            RefreshUI();
        }

        void RefreshUI()
        {
            balanceField.text = $"${playerPurse.GetBalance():N2}";
        }
    }
}
