using System.Collections.Generic;
using GameControllers.GameLogic.Abilities;
using UnityEngine;

namespace GameControllers.GameLaunch.ControllersData
{
    public class AbilitiesControllerData : MonoBehaviour
    {
        [field: Header("FirstPlayerBlock")]
        [field: SerializeField] public GameObject FirstPawnAbility { get; private set; }
        [field: SerializeField] public Transform FirstPawnAbilityParent { get; private set; }
        [field: SerializeField] public GameObject FirstBlockAbility { get; private set; }
        [field: SerializeField] public Transform FirstBlockAbilityParent { get; private set; }
        [field: SerializeField] public List<Improver> FirstImproverAbilities { get; private set; }
        [field: SerializeField] public Transform FirstImproverAbilityParent1 { get; private set; }
        [field: SerializeField] public Transform FirstImproverAbilityParent2 { get; private set; }
        
        [field: Header("SecondPlayerBlock")]
        [field: SerializeField] public GameObject SecondPawnAbility { get; private set; }
        [field: SerializeField] public Transform SecondPawnAbilityParent { get; private set; }
        [field: SerializeField] public GameObject SecondBlockAbility { get; private set; }
        [field: SerializeField] public Transform SecondBlockAbilityParent { get; private set; }
        [field: SerializeField] public List<Improver> SecondImproverAbilities { get; private set; }
        [field: SerializeField] public Transform SecondImproverAbilityParent1 { get; private set; }
        [field: SerializeField] public Transform SecondImproverAbilityParent2 { get; private set; }
    }
}
