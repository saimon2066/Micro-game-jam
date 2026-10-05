using System;
using UnityEngine;

namespace Game.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        [NonSerialized]
        public IAAMain Inputs;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            Inputs = new IAAMain();
        }

        private void OnEnable()
        {
            Inputs.Enable();
        }

        private void OnDisable()
        {
            Inputs.Disable();
        }
    }
}
