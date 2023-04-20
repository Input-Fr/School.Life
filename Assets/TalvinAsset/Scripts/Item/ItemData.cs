using UnityEngine;

namespace Item
{
    [CreateAssetMenu(menuName = "Scriptable object/Item2")]
    public class ItemData : ScriptableObject
    {
        [Header("Only UI")] 
        public bool stackable = true;
        public uint nbItemByStack;
        public bool multiple = true;


        [Header("Both")] 
        public string itemName;
        public string description;
        public Sprite image;
        public GameObject prefabInScene;
        public GameObject prefabInHand;

        private void Awake() {
            if (multiple || stackable) return;
            nbItemByStack = 1;
            stackable = false;

        }
    }
}
