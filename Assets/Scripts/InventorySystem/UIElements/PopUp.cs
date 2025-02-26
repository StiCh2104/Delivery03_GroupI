using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ValueText;
    private string translationKey;

    public void ChangeTransform(Vector3 transform)
    {
        this.transform.position = transform;
    }
}
