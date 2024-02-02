namespace CardBuilder.Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    /// <summary>
    ///Data that a card exists from (saved and used for editor side) 
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Testing", order = 1), Serializable]
    public class SO_CardData : ScriptableObject
    {
        [SerializeField, HideInInspector] private List<IntProperty> m_intPropertyList = new();
        [SerializeField, HideInInspector] private List<StringProperty> m_stringPropertyList = new();
        [SerializeField, HideInInspector] private List<SpriteProperty> m_spritePropertyList = new();
        [SerializeField, HideInInspector] private List<EnumProperty> m_enumPropertyList = new();

        #region Getters
        public List<IntProperty> IntPropertyList { get => m_intPropertyList; }
        public List<StringProperty> StringPropertyList { get => m_stringPropertyList; }
        public List<SpriteProperty> SpritePropertyList { get => m_spritePropertyList; }
        public List<EnumProperty> EnumPropertyList { get => m_enumPropertyList; }
        #endregion

        //Clears all lists (usefull for temporary file, or reseting data)
        public void ClearAllLists()
        {
            m_intPropertyList.Clear();
            m_stringPropertyList.Clear();
            m_spritePropertyList.Clear();
            m_enumPropertyList.Clear();
        }
    }

}
