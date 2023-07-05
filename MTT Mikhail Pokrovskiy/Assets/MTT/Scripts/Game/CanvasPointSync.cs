using UnityEngine;

namespace MTT
{
    public class CanvasPointSync : MonoBehaviour
    {
        [SerializeField] private RectTransform canvasPoint;

        public void Sync()
        {
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, canvasPoint.position);

            Vector3 worldPoint;
            if (Camera.main != null)
            {
                worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, 0));
                worldPoint.z = 0;
            }
            else
            {
                Debug.LogWarning("Main camera not found!");
                return;
            }

            transform.position = worldPoint;
        }
    }
}