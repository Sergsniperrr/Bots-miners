using UnityEngine;

public class FlagPositionChecker : MonoBehaviour
{
    public bool CheckForNearbyBase(Vector3 mousePosition)
    {
        float radius = 10f;
        Vector3 scanSize = new(radius, 0.01f, radius);
        bool result = false;

        Collider[] hitColliders = Physics.OverlapBox(mousePosition, scanSize, Quaternion.identity);

        foreach (Collider collider in hitColliders)
        {
            if (collider.TryGetComponent(out Base _))
            {
                result = true;
                break;
            }
        }

        return result;
    }
}
