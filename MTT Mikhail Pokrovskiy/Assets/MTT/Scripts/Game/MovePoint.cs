using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MTT
{
    public class MovePoint : MonoBehaviour
    {
        [SerializeField] private Vector2Int _pos;
        [SerializeField] private bool _block;

        public void SetBlock(bool enable) => _block = enable;
        
        public void Instantiate(Vector2 pos, Vector2Int index, Transform parent, bool block)
        {
            MovePoint point = Instantiate(this, pos, Quaternion.identity);
            point.transform.parent = parent;
            point._pos = index;
            point._block = block;
        }
    }
}