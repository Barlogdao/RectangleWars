using UnityEngine;

namespace RB.Graphics
{
    public class MouseParallax : MonoBehaviour
    {
        [SerializeField] private float _parallaxStrenght = 1f;
        [SerializeField] private Vector2 _parallaxClamp = Vector2.one;
        [SerializeField] private bool _invertAxis = false;
        private Vector2 _startPosition;
        void Start()
        {
            _startPosition = transform.position;
        }

        void Update()
        {
            Vector2 mousePos = Camera.main.ScreenToViewportPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
            mousePos = _invertAxis ? -mousePos : mousePos;
            float posX = Mathf.Lerp(transform.position.x, _startPosition.x + (mousePos.x * _parallaxStrenght), 2f * Time.deltaTime);
            float posY = Mathf.Lerp(transform.position.y, _startPosition.y + (mousePos.y * _parallaxStrenght), 2f * Time.deltaTime);

            posX = Mathf.Clamp(posX, _startPosition.x - _parallaxClamp.x, _startPosition.x + _parallaxClamp.x);
            posY = Mathf.Clamp(posY, _startPosition.y - _parallaxClamp.y, _startPosition.y + _parallaxClamp.y);

            transform.position = new Vector3(posX, posY, transform.position.z);
        }
    }
}
