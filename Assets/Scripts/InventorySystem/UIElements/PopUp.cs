using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ValueText;

    public void ChangeTransform(Vector3 transform)
    {
        this.transform.position = transform;
    }
}
