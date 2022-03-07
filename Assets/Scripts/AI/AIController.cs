using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Dictionary<Unit, AIPlan> enemyPlans = new Dictionary<Unit, AIPlan>();

    public void ClearPlans()
    {
        enemyPlans.Clear();
    }
}
