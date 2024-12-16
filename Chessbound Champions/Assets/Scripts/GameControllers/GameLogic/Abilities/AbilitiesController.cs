using System.Collections.Generic;
using GameControllers.GameBoard;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.ControllersData;
using GameControllers.GameLaunch.Properties;
using GameControllers.Players;
using UnityEngine;

namespace GameControllers.GameLogic.Abilities
{
    public class AbilitiesController : IGameController
    {
        private readonly GameObject _firstPawnAbility;
        private readonly Transform _firstPawnAbilityParent;
        private readonly GameObject _firstBlockAbility;
        private readonly Transform _firstBlockAbilityParent;
        private readonly List<Improver> _firstImproverAbilities;
        private readonly Transform _firstImproverAbilityParent1;
        private readonly Transform _firstImproverAbilityParent2;
        
        private readonly GameObject _secondPawnAbility;
        private readonly Transform _secondPawnAbilityParent;
        private readonly GameObject _secondBlockAbility;
        private readonly Transform _secondBlockAbilityParent;
        private readonly List<Improver> _secondImproverAbilities;
        private readonly Transform _secondImproverAbilityParent1;
        private readonly Transform _secondImproverAbilityParent2;
        
        private static Improver _improver1;
        private static Improver _improver2;

        public AbilitiesController(AbilitiesControllerData abilitiesControllerData)
        {
            SubscribeToActions();
                
            _firstPawnAbility = abilitiesControllerData.FirstPawnAbility;
            _firstPawnAbilityParent = abilitiesControllerData.FirstPawnAbilityParent;
            _firstBlockAbility = abilitiesControllerData.FirstBlockAbility;
            _firstBlockAbilityParent = abilitiesControllerData.FirstBlockAbilityParent;
            _firstImproverAbilities = abilitiesControllerData.FirstImproverAbilities;
            _firstImproverAbilityParent1 = abilitiesControllerData.FirstImproverAbilityParent1;
            _firstImproverAbilityParent2 = abilitiesControllerData.FirstImproverAbilityParent2;
            
            _secondPawnAbility = abilitiesControllerData.SecondPawnAbility;
            _secondPawnAbilityParent = abilitiesControllerData.SecondPawnAbilityParent;
            _secondBlockAbility = abilitiesControllerData.SecondBlockAbility;
            _secondBlockAbilityParent = abilitiesControllerData.SecondBlockAbilityParent;
            _secondImproverAbilities = abilitiesControllerData.SecondImproverAbilities;
            _secondImproverAbilityParent1 = abilitiesControllerData.SecondImproverAbilityParent1;
            _secondImproverAbilityParent2 = abilitiesControllerData.SecondImproverAbilityParent2;
            
            CreateImproverAbility();
        }

        private void SubscribeToActions()
        {
            FigureSpawner.OnDropAbility += SpawnAbility;
            AbilityImprove.OnRespawnImproverAbility += RespawnImproverAbility;
        }
        
        private void CreateImproverAbility()
        {
            GetInstantiateImproverAbility(_firstImproverAbilityParent1, _firstImproverAbilities, 0);
            GetInstantiateImproverAbility(_firstImproverAbilityParent2, _firstImproverAbilities, 1);
            
            _improver1 = GetInstantiateImproverAbility(_secondImproverAbilityParent1, _secondImproverAbilities, 0);
            _improver2 = GetInstantiateImproverAbility(_secondImproverAbilityParent2, _secondImproverAbilities, 1);
        }

        private void RespawnImproverAbility(int numberSlot, PlayersTypes playerType)
        {
            if (playerType == PlayersTypes.FirstPlayer)
            {
                if (numberSlot == 0)
                    GetInstantiateImproverAbility(
                        _firstImproverAbilityParent1, _firstImproverAbilities, numberSlot);
                else
                    GetInstantiateImproverAbility(
                        _firstImproverAbilityParent2, _firstImproverAbilities, numberSlot);
            }
            else
            {
                if (numberSlot == 0)
                    _improver1 = GetInstantiateImproverAbility(
                        _secondImproverAbilityParent1, _secondImproverAbilities, numberSlot);
                else
                    _improver2 = GetInstantiateImproverAbility(
                        _secondImproverAbilityParent2, _secondImproverAbilities, numberSlot);
                
            }
        }

        private Improver GetInstantiateImproverAbility(
            Transform improverAbilityParent, 
            List<Improver> improverAbilities,
            int numberSlot)
        {
            var indexAbility = Random.Range(0, improverAbilities.Count);
            var improver = Object.Instantiate(improverAbilities[indexAbility], improverAbilityParent);
            improver.InitNumberSlot(numberSlot);
            
            return improver;
        }

        private void SpawnAbility(FiguresTypes figureType, PlayersTypes playerType)
        {
            switch (figureType)
            {
                case FiguresTypes.Pawn:
                    if (playerType == PlayersTypes.FirstPlayer)
                        Object.Instantiate(_firstPawnAbility, _firstPawnAbilityParent);
                    else
                        Object.Instantiate(_secondPawnAbility, _secondPawnAbilityParent);
                    break;
                case FiguresTypes.Block:
                    if (playerType == PlayersTypes.FirstPlayer)
                        Object.Instantiate(_firstBlockAbility, _firstBlockAbilityParent);
                    else
                        Object.Instantiate(_secondBlockAbility, _secondBlockAbilityParent);
                    break;
            }
        }

        public static List<Improver> GetImprovers()
        {
            return new List<Improver> {_improver1, _improver2};
        }

        public void KillController()
        {
            FigureSpawner.OnDropAbility -= SpawnAbility;
            AbilityImprove.OnRespawnImproverAbility -= RespawnImproverAbility;
        }
    }
}

