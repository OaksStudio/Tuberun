using UnityEngine;

namespace Jozi.Utilities.Primitives
{
    [CreateAssetMenu(fileName = "SO_String", menuName = "Primitives/String", order = 2)]
    public class SOString : ScriptableObject
    {
        public string Value;
    }
}