using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameObject _parent;

    private void Awake()
    {
        _parent = gameObject.transform.parent.gameObject;
    }

    public void DestroyPowerUp()
    {
        Destroy(_parent);
    }
}
