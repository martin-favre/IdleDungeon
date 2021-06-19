using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClickHandlerComponent : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool handled = false;
            if (Physics.Raycast(ray, out hit))
            {
                handled = HandleClickedEnemies(hit.transform);
            }
            if (!handled) SingletonProvider.MainEventPublisher.Publish(EventType.PlayerClickedNothing, new PlayerClickedNothingEvent());
        }
    }

    bool HandleClickedEnemies(Transform enemyTransf)
    {
        print(enemyTransf.gameObject.name);
        var enemy = enemyTransf.GetComponent<VisalisedEnemyComponent>();
        if (enemy)
        {
            Debug.Log("Player clicked: " + enemy.Character.Name);
            SingletonProvider.MainEventPublisher.Publish(EventType.PlayerClickedEnemy, new PlayerClickedEnemyEvent(enemy.Character));
            return true;
        }
        return false;
    }
}
