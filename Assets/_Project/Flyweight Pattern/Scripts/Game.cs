using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace FlyweightPattern{
    public class Game : MonoBehaviour{
        [SerializeField] private int _minigameTime = 60;
        [SerializeField] private Vector2 _arenaSize;
        [SerializeField] private int _totalCoins = 1000;
        [FormerlySerializedAs("_coinPrefab")] [SerializeField] private FlyweightCoin _flyweightCoinPrefab;
        [SerializeField] private UI _ui;

        private static readonly CoinTypeFlyweight[] _coinTypes = new CoinTypeFlyweight[9];
        private int _score;

        private void Start(){
            _ui.UpdateScore(0);
            _ui.UpdateTimer(_minigameTime);
            CreateCoinTypes();
        }

        private void OnDrawGizmosSelected(){
            Gizmos.color = new Color(0f, 0f, 0f, 0.25f);
            Vector3 position = transform.position + Vector3.up * 0.5f;
            Vector3 size = new(_arenaSize.x, 1f, _arenaSize.y);
            Gizmos.DrawCube(position, size);
        }

        public void StartGame(){
            AddPoints(-_score);
            CreateCoins();
            StartCoroutine(StartTimer());
        }

        public void CollectCoin(int coinTypeIndex){
            int points = _coinTypes[coinTypeIndex].Points;
            AddPoints(points);
        }

        private void CreateCoinTypes(){
            _coinTypes[0] = new CoinTypeFlyweight(CoinRareness.Common, CoinSize.Small);
            _coinTypes[1] = new CoinTypeFlyweight(CoinRareness.Common, CoinSize.Normal);
            _coinTypes[2] = new CoinTypeFlyweight(CoinRareness.Common, CoinSize.Big);
            _coinTypes[3] = new CoinTypeFlyweight(CoinRareness.Uncommon, CoinSize.Small);
            _coinTypes[4] = new CoinTypeFlyweight(CoinRareness.Uncommon, CoinSize.Normal);
            _coinTypes[5] = new CoinTypeFlyweight(CoinRareness.Uncommon, CoinSize.Big);
            _coinTypes[6] = new CoinTypeFlyweight(CoinRareness.Rare, CoinSize.Small);
            _coinTypes[7] = new CoinTypeFlyweight(CoinRareness.Rare, CoinSize.Normal);
            _coinTypes[8] = new CoinTypeFlyweight(CoinRareness.Rare, CoinSize.Big);
        }
        
        public static CoinTypeFlyweight GetCoinType(CoinRareness coinRareness, CoinSize coinSize){
            int index = GetIndexOfCoinType(coinRareness, coinSize);
            return _coinTypes[index];
        }
        
        public static CoinTypeFlyweight GetCoinType(int index){
            return _coinTypes[index];
        }

        private void AddPoints(int pointsToAdd){
            _score += pointsToAdd;
            _ui.UpdateScore(_score);
        }

        private void CreateCoins(){
            Vector3 position = transform.position;
            Vector2 minMaxX = new(position.x - _arenaSize.x / 2, position.x + _arenaSize.x / 2);
            Vector2 minMaxZ = new(position.z - _arenaSize.y / 2, position.z + _arenaSize.x / 2);
            float coinHeight = _flyweightCoinPrefab.GetComponent<SphereCollider>().radius;
            
            for (int i = 0; i < _totalCoins; i++){
                CoinRareness rareness = Random.Range(0f, 1f) switch{
                    <= 0.6f => CoinRareness.Common,
                    <= 0.9f => CoinRareness.Uncommon,
                    _ => CoinRareness.Rare
                };
                
                CoinSize size = rareness switch{
                    CoinRareness.Common => Random.Range(0f, 1f) switch{
                        <= 0.25f => CoinSize.Small,
                        <= 0.55f => CoinSize.Normal,
                        _ => CoinSize.Big
                    },
                    CoinRareness.Uncommon => Random.Range(0f, 1f) switch{
                        <= 0.40f => CoinSize.Small,
                        <= 0.85f => CoinSize.Normal,
                        _ => CoinSize.Big
                    },
                    CoinRareness.Rare => Random.Range(0f, 1f) switch{
                        <= 0.60f => CoinSize.Small,
                        <= 0.85f => CoinSize.Normal,
                        _ => CoinSize.Big
                    },
                    _ => throw new ArgumentOutOfRangeException()
                };

                Vector3 coinPosition = Vector3.zero;
                do{
                    coinPosition.x = Random.Range(minMaxX.x, minMaxX.y);
                    coinPosition.z = Random.Range(minMaxZ.x, minMaxZ.y);
                } while (Vector3.Distance(coinPosition, position) < 3);

                int index = GetIndexOfCoinType(rareness, size);
                coinPosition.y += coinHeight * _coinTypes[index].Scale;

                Instantiate(_flyweightCoinPrefab, coinPosition, Quaternion.identity).Setup(GetCoinType(index), index);
            }
        }

        private static int GetIndexOfCoinType(CoinRareness rareness, CoinSize size){
            return 3 * (int) rareness + (int) size;
        }

        private IEnumerator StartTimer(){
            int elapsedTime = 0;
            WaitForSeconds waitTime = new(1);

            while (elapsedTime <= _minigameTime){
                _ui.UpdateTimer(_minigameTime - elapsedTime);
                yield return waitTime;
                elapsedTime++;
            }
        }
    }
}