using UnityEngine;
using UnityEngine.UI;



namespace UnityStandardAssets.Characters.FirstPerson
{
    internal class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image _aim;
        [SerializeField] private Color _normalAimColor;
        [SerializeField] private Color _interactiveAimColor;


        private void Awake()
        {
            _aim = GetComponent<Image>();
        }

        public void CanInteract(bool IsCan)
        {
            _aim.color = IsCan ? _interactiveAimColor : _normalAimColor; 
        }
    }
}