using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{
    [Serializable]
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TestingEnums", order = 2)]
    public class EnumSO : ScriptableObject
    {
        [SerializeField]
        public string enumMainName;

        [SerializeField]
        public List<string> enumNames;
    }
}
