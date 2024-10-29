using UnityEngine;
using UnityEngine.UI;

public class BackGroundMove : MonoBehaviour
{
    RawImage image;
    [SerializeField]
    float x;
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, 0) * Time.deltaTime, image.uvRect.size);
    }
}
