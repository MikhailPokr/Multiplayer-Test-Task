using Photon.Pun;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    [Singleton]
    internal class LevelManager : MonoBehaviour, ISingleton
    {
        [SerializeField] private GamePointsManager _gamePointManager;
        [Space]
        [SerializeField] private CanvasPointSync _cornerTopLeft;
        [SerializeField] private CanvasPointSync _cornerBottomRight;
        [Space]
        [SerializeField] private string _pattern;
        [Space]
        [SerializeField] private Vector2Int _size;
        [Space]
        [SerializeField] private GamePoint _pointPrefab;
        [SerializeField] private Transform _pointParent;

        private void Awake()
        {
            PointsSync();

            List<GamePoint> points = GenerateLevel();

            if (PhotonNetwork.LocalPlayer.ActorNumber != 1)
            {
                _gamePointManager.SetPoints(_size);
            }
            else
            {
                _gamePointManager.SetNewPoints(points, _size);
            }

            
        }

        private bool[,] GetMap()
        {
            bool[,] mapResult;

            int length = _size.y * _size.x;
            bool[] map = new bool[length];

            string[] pattern = _pattern.Split('.');
            if (string.IsNullOrEmpty(_pattern))
            {
                mapResult = new bool[_size.x, _size.y];
                for (int i = 0; i < _size.x; i++)
                    for (int j = 0; j < _size.y; j++)
                    {
                        mapResult[i, j] = true;
                    }
                return mapResult;
            }


            bool hasCorrectPart = false;

            int patternIndex = 0;
            int patternLength = -1;
            int patternRepeat = -1;
            int patternLengthCurrent = -1;
            int patternRepeatCurrent = -1;
            for (int i = 0; i < length; i++)
            {
                map[i] = false;

                if (patternIndex >= pattern.Length)
                {
                    patternIndex = 0;
                }
                
                if (patternLength == -1)
                {
                    int pLenght = 0;
                    int pRepeat = 0;

                    string[] p = pattern[patternIndex].Split(':');
                    if (p.Length == 1)
                    {
                        if (!int.TryParse(p[0], out pLenght))
                        {
                            patternIndex++;
                            if (!hasCorrectPart)
                                break;
                            i--;
                            continue;
                        }
                    }
                    if (p.Length == 2)
                    {
                        if (!int.TryParse(p[0], out pLenght) || !int.TryParse(p[1], out pRepeat))
                        {
                            patternIndex++;
                            if (!hasCorrectPart)
                                break;
                            i--;
                            continue;
                        }    
                    }

                    hasCorrectPart = true;

                    patternLength = pLenght;
                    patternRepeat = pRepeat;
                    patternLengthCurrent = patternLength;
                    patternRepeatCurrent = patternRepeat;
                }
                if (patternLengthCurrent == 0)
                {
                    map[i] = true;
                    if (patternRepeatCurrent > 0)
                    {
                        patternRepeatCurrent--;
                        patternLengthCurrent = patternLength;
                    }
                    else
                    {
                        patternLength = -1;
                        patternIndex++;
                    }
                }
                else
                {
                    patternLengthCurrent--;
                }

            }

            if (!hasCorrectPart)
            {
                if (string.IsNullOrEmpty(_pattern))
                {
                    mapResult = new bool[_size.x, _size.y];
                    for (int i = 0; i < _size.x; i++)
                        for (int j = 0; j < _size.y; j++)
                        {
                            mapResult[i, j] = true;
                        }
                    return mapResult;
                }
            }

            mapResult = new bool[_size.x, _size.y]; 
            for (int i = 0; i < _size.x; i++)
                for (int j = 0; j < _size.y; j++)
                {
                    mapResult[i, j] = map[j * _size.x + i];
                }
            return mapResult;
        }

        private List<GamePoint> GenerateLevel()
        {
            List<GamePoint> points = new();

            bool[,] map = GetMap();

            for (int i = 0; i < _size.x; i++)
                for (int j = 0; j < _size.y; j++)
                {
                    GamePoint point = PhotonNetwork.Instantiate(_pointPrefab.name, GetPointPos(i, j), Quaternion.identity).GetComponent<GamePoint>();
                    point.Enable();
                    point.SetData(new() { IndexX = i, IndexY = j, Block = map[i, j], PlayerIndex = -1, Projectile = false, ProjectileIndex = -1});
                    points.Add(point);
                }

            return points;
        }

        public void PointsSync()
        {
            _cornerTopLeft.Sync();
            _cornerBottomRight.Sync();
        }
        private Vector2 GetPointPos(int i, int j)
        {
            Vector2 gaps = GetGaps();

            Vector2 pos = new Vector2(_cornerTopLeft.transform.position.x + i * gaps.x + gaps.x / 2,
                             _cornerTopLeft.transform.position.y - j * gaps.y - gaps.y / 2);

            return pos;
        }
        public Vector2 GetGaps()
        {
            if (_cornerTopLeft == null || _cornerBottomRight == null)
                return Vector2.zero;
            
            float x = Mathf.Abs(_cornerTopLeft.transform.position.x - _cornerBottomRight.transform.position.x) / _size.x;
            float y = Mathf.Abs(_cornerTopLeft.transform.position.y - _cornerBottomRight.transform.position.y) / _size.y;

            return new Vector2(x, y);
        }

#if UNITY_EDITOR
        [Space]
        [SerializeField] private float _gizmosRadius;
        private void OnDrawGizmos()
        {
            if (_cornerTopLeft == null || _cornerBottomRight == null)
                return;

            bool[,] map = GetMap();
        
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            for (int i = 0; i < _size.x; i++)
                for (int j = 0; j < _size.y; j++)
                {
                    if (!map[i, j])
                        Gizmos.DrawWireSphere(GetPointPos(i, j), _gizmosRadius);
                    else
                        Gizmos.DrawSphere(GetPointPos(i, j), _gizmosRadius);
                }
        }
#endif
    }
}