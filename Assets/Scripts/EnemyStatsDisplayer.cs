using UnityEngine;

public class EnemyStatsDisplayer : MonoBehaviour
{
    SimpleValueDisplayer.ValueHook displayer;
    SimpleObserver<ICombatUpdateEvent> observer;

    void Awake()
    {
        displayer = SimpleValueDisplayer.Instance.RegisterValue();
        observer = new SimpleObserver<ICombatUpdateEvent>(CombatManager.Instance, (e) =>
        {
            UpdateText(e.Combat);
        });
    }

    private void UpdateText(ICombatReader combat)
    {
        string s = "";
        if (combat != null)
        {
            foreach (var enemy in combat.GetEnemiesAttributes())
            {
                s += "Enemy HP: " + enemy.Hp + "\n";
            }
        }
        displayer.UpdateValue(s);
    }
}