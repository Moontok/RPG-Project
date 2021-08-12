using UnityEngine;
using UnityEngine.UI;

public class LoadButtonSetup : MonoBehaviour
{
    [SerializeField] Button loadButton = null;
    [SerializeField] Button deleteButton = null;

    public Button GetLoadButton()
    {
        return loadButton;
    }

    public Button GetDeleteButton()
    {
        return deleteButton;
    }
}
