using System;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FlyweightPattern.ECS{
    // [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial class GameSystem : SystemBase{
        private static CoinType[] _coinTypes;

        public int TotalScore{ get; private set; }
        public float ElapsedTime{ get; private set; }

        protected override void OnCreate(){
            RequireForUpdate<Player>();
            RequireForUpdate<SimulationSingleton>();
        }

        protected override void OnUpdate(){
            ElapsedTime += SystemAPI.Time.DeltaTime;
            Simulation simulation = SystemAPI.GetSingleton<SimulationSingleton>().AsSimulation();
            TriggerEvents triggerEvents = simulation.TriggerEvents;
            foreach (TriggerEvent triggerEvent in triggerEvents){
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                if (EntityManager.HasComponent<CoinPoints>(entityA)){
                    CollectCoin(EntityManager.GetSharedComponent<CoinPoints>(entityA));
                    EntityManager.AddComponent<Disabled>(entityA);
                    // EntityManager.DestroyEntity(entityA);
                }
                else if (EntityManager.HasComponent<CoinPoints>(entityB)){
                    CollectCoin(EntityManager.GetSharedComponent<CoinPoints>(entityB));
                    EntityManager.AddComponent<Disabled>(entityB);
                    // EntityManager.DestroyEntity(entityB);
                }
            }
        }

        public void StartGame(){
            var game = SystemAPI.GetSingleton<Game>();

            ElapsedTime = 0;

            GenerateCoinEntities(game);
        }

        public void EndGame(){
            TotalScore = 0;
            EntityCommandBuffer ecb = new(Allocator.TempJob);

            foreach (var (coinPoints, entity) in SystemAPI.Query<CoinPoints>().WithEntityAccess()){
                ecb.DestroyEntity(entity);
            }

            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        private void GenerateCoinEntities(Game game){
            if (_coinTypes == null){ CreateCoinTypes(game); }

            Vector3 position = game.Origin;
            Vector2 minMaxX = new(position.x - game.ArenaSize.x / 2, position.x + game.ArenaSize.x / 2);
            Vector2 minMaxZ = new(position.z - game.ArenaSize.y / 2, position.z + game.ArenaSize.y / 2);

            for (int i = 0; i < game.TotalCoins; i++){
                CoinRareness rareness;
                float rarenessRandom = Random.Range(0f, 1f);
                if (rarenessRandom <= game.RarityChance.x){ rareness = CoinRareness.Common; }
                else if (rarenessRandom <= game.RarityChance.y){ rareness = CoinRareness.Uncommon; }
                else{ rareness = CoinRareness.Rare; }
                
                float sizeRandom = Random.Range(0f, 1f);
                CoinSize size;
                switch (rareness){
                    case CoinRareness.Common:
                        if (sizeRandom <= game.SizeChance.c0.x){ size = CoinSize.Small; }
                        else if (sizeRandom <= game.SizeChance.c0.y){ size = CoinSize.Normal; }
                        else{ size = CoinSize.Big; }
                        break;
                    case CoinRareness.Uncommon:
                        if (sizeRandom <= game.SizeChance.c1.x){ size = CoinSize.Small; }
                        else if (sizeRandom <= game.SizeChance.c1.y){ size = CoinSize.Normal; }
                        else{ size = CoinSize.Big; }
                        break;
                    case CoinRareness.Rare:
                        if (sizeRandom <= game.SizeChance.c2.x){ size = CoinSize.Small; }
                        else if (sizeRandom <= game.SizeChance.c2.y){ size = CoinSize.Normal; }
                        else{ size = CoinSize.Big; }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                int typeIndex = 3 * (int) rareness + (int) size;
                CoinType coinType = _coinTypes[typeIndex];

                Vector3 coinPosition = Vector3.zero;
                do{
                    coinPosition.x = Random.Range(minMaxX.x, minMaxX.y);
                    coinPosition.z = Random.Range(minMaxZ.x, minMaxZ.y);
                } while (Vector3.Distance(coinPosition, position) < 3);

                Entity newEntity = EntityManager.Instantiate(game.CoinPrefab);

                EntityManager.AddComponentData(newEntity, coinType.Color);
                EntityManager.AddComponentData(newEntity, coinType.RotationSpeed);

                EntityManager.AddSharedComponent(newEntity, coinType.Points);

                LocalTransform transform = LocalTransform.FromPosition(coinPosition).ApplyScale(coinType.Scale);
                EntityManager.SetComponentData(newEntity, transform);
            }
        }

        private void CreateCoinTypes(Game game){
            _coinTypes = new CoinType[9];

            _coinTypes[0] = new CoinType(game.CoinScales.x, game.CoinRotationSpeeds.x, game.CommonCoinPoints.x,
                game.CommonCoinsColor); // Small Common
            _coinTypes[1] = new CoinType(game.CoinScales.y, game.CoinRotationSpeeds.y, game.CommonCoinPoints.y,
                game.CommonCoinsColor); // Normal Common
            _coinTypes[2] = new CoinType(game.CoinScales.z, game.CoinRotationSpeeds.z, game.CommonCoinPoints.z,
                game.CommonCoinsColor); // Big Common
            _coinTypes[3] = new CoinType(game.CoinScales.x, game.CoinRotationSpeeds.x, game.UncommonCoinPoints.x,
                game.UncommonCoinsColor); // Small Uncommon
            _coinTypes[4] = new CoinType(game.CoinScales.y, game.CoinRotationSpeeds.y, game.UncommonCoinPoints.y,
                game.UncommonCoinsColor); // Normal Uncommon
            _coinTypes[5] = new CoinType(game.CoinScales.z, game.CoinRotationSpeeds.z, game.UncommonCoinPoints.z,
                game.UncommonCoinsColor); // Big Uncommon
            _coinTypes[6] = new CoinType(game.CoinScales.x, game.CoinRotationSpeeds.x, game.RareCoinPoints.x,
                game.RareCoinsColor); // Small Rare
            _coinTypes[7] = new CoinType(game.CoinScales.y, game.CoinRotationSpeeds.y, game.RareCoinPoints.y,
                game.RareCoinsColor); // Normal Rare
            _coinTypes[8] = new CoinType(game.CoinScales.z, game.CoinRotationSpeeds.z, game.RareCoinPoints.z,
                game.RareCoinsColor); // Big Rare
        }

        private void CollectCoin(CoinPoints coinPoints){
            TotalScore += coinPoints.Points;
        }
    }
}