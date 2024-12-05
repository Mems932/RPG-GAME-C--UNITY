using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // La cible que la cam�ra doit suivre.

    public float ymin, ymax, xmin, xmax; // Limites du mouvement de la cam�ra.

    // Update est appel� � chaque frame.
    void Update()
    {
        // R�cup�re la position de la cible.
        Vector3 targetPos = target.transform.position;

        // Fixe la position Z de la cam�ra (utile pour 2D, �viter qu'elle soit affect�e).
        targetPos.z = -10;
        transform.position = new Vector3(Mathf.Clamp(targetPos.x, xmin, xmax), Mathf.Clamp(targetPos.y, ymin, ymax), targetPos.z);
        // D�place la cam�ra � la position calcul�e.
        // transform.position = targetPos;
    }
}
