using UnityEngine;

namespace ZGameLib.UnityRuntime
{
    public class UnityBehaviour : MonoBehaviour
    {
        public static UnityBehaviour Get(GameObject obj)
        {
            UnityBehaviour behaviour = obj.GetComponent<UnityBehaviour>();
            if (behaviour == null) behaviour = obj.AddComponent<UnityBehaviour>();
            return behaviour;
        }
    }
}
