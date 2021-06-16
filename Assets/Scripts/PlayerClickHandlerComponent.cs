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

            if (Physics.Raycast(ray, out hit))
            {
                bool handled = HandleClickedEnemies(hit.transform);
                if(!handled) SingletonProvider.MainEventHandler.Publish(EventType.PlayerClickedNothing, new PlayerClickedNothingEvent());
            }

        }
    }

    bool HandleClickedEnemies(Transform enemyTransf)
    {
        print(enemyTransf.gameObject.name);
        var enemy = enemyTransf.GetComponent<VisalisedEnemyComponent>();
        if (enemy)
        {
            Debug.Log("Player clicked: " + enemy.Character.Name);
            SingletonProvider.MainEventHandler.Publish(EventType.PlayerClickedEnemy, new PlayerClickedEnemyEvent(enemy.Character));
            return true;
        }
        return false;
    }
}
