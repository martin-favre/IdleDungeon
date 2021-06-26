using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerClickHandlerComponent : MonoBehaviour
{
    public EventSystem eventSystem;
    [SerializeField]  GraphicRaycaster raycaster;
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
            if(!handled) handled = HandleClickUI();
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

    bool HandleClickUI() {
            PointerEventData  pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            if(results.Count > 0) {
                foreach(var result in results){
                    var statBox = result.gameObject.GetComponent<CharacterStatBoxComponent>();
                    if(statBox != null){
                        if(!statBox.IsPlayer() && SingletonProvider.MainCombatManager.InCombat()){
                            var index = statBox.TargetIndex;
                            var combat = SingletonProvider.MainCombatManager.CombatReader;
                            if(index < combat.GetEnemies().Length){
                                var enemy = combat.GetEnemies()[index];
                                Debug.Log("Player clicked statbox: " + enemy.Name);
                                SingletonProvider.MainEventPublisher.Publish(EventType.PlayerClickedEnemy, new PlayerClickedEnemyEvent(enemy));
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
    }
}
