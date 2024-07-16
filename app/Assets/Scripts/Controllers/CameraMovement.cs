using System;
using System.Collections;
using UnityEngine;

public class CameraMovement : Singleton<CameraMovement>
{

    public Transform targetPosition;
    public float speedToBattleArea = 6.0f;
    public float speedToShopArea = 20.0f;
    private Vector3 initialPosition;

    private bool moveToFight = false;
    private bool moveToShop = false;

    private TimeScaleController _timeScaleController;
    private CanvasManager _canvasManager;


    private void Start()
    {
        _timeScaleController = TimeScaleController.Instance;
        _canvasManager = CanvasManager.Instance;
        initialPosition = transform.position;
    }

    private IEnumerator MoveToFightPositionCoroutine()
    {
        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            float step = speedToBattleArea * Time.deltaTime * _timeScaleController.speedGame;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, step);
            yield return null;
        }
        moveToFight = false;
        EventManager.StartBattle();
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
        _canvasManager.ToggleCanvasInterStep();
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
