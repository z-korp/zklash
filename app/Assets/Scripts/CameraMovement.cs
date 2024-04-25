using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;

    public Transform targetPosition;
    public float speedToBattleArea = 6.0f;
    public float speedToShopArea = 20.0f;
    private Vector3 initialPosition;

    private bool moveToFight = false;
    private bool moveToShop = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DroppableManager found!");
            return;
        }
        instance = this;

    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private IEnumerator MoveToFightPositionCoroutine()
    {
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            float step = speedToBattleArea * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, step);
            yield return null;
        }
        moveToFight = false;
    }

    private IEnumerator MoveToShopPositionCoroutine()
    {
        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            float step = speedToShopArea * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);
            yield return null;
        }
        moveToShop = false;
    }

    public void MoveCameraToFight()
    {
        if (!moveToFight)
        {
            moveToFight = true;
            moveToShop = false;
            StartCoroutine(MoveToFightPositionCoroutine());
        }
    }

    public void MoveCameraToShop()
    {
        if (!moveToShop)
        {
            moveToShop = true;
            moveToFight = false;
            StartCoroutine(MoveToShopPositionCoroutine());
        }
    }
}
