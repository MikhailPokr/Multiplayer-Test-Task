using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MTT
{
    internal class UIModeMark : MonoBehaviour
    {
        [SerializeField] private Image _mark;
        [SerializeField] private GameObject _pointJoin;
        [SerializeField] private GameObject _pointCreate;

        public void ChangePosition(bool itsCreateMode)
        {
            if (itsCreateMode)
            {
                _mark.transform.position = _pointCreate.transform.position;
            }
            else
            {
                _mark.transform.position = _pointJoin.transform.position;
            }
        }


        
    }
}