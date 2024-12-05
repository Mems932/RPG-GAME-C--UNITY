using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // La cible que la caméra doit suivre.

    public float ymin, ymax, xmin, xmax; // Limites du mouvement de la caméra.

    // Update est appelé à chaque frame.
    void Update()
    {
        // Récupère la position de la cible.
        Vector3 targetPos = target.transform.position;

        // Fixe la position Z de la caméra (utile pour 2D, éviter qu'elle soit affectée).
        targetPos.z = -10;
        transform.position = new Vector3(Mathf.Clamp(targetPos.x, xmin, xmax), Mathf.Clamp(targetPos.y, ymin, ymax), targetPos.z);
        // Déplace la caméra à la position calculée.
        // transform.position = targetPos;
    }
}
