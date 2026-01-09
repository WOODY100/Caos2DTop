using UnityEngine;

public static class CameraSnapper
{
    /// <summary>
    /// Fuerza a la cámara a actualizar su posición inmediatamente.
    /// Funciona con Cinemachine sin depender de él.
    /// </summary>
    public static void Snap()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Buscar cualquier "brain" sin depender de namespace
        Component brain = cam.GetComponent("CinemachineBrain");
        if (brain == null) return;

        Behaviour behaviour = brain as Behaviour;
        if (behaviour == null) return;

        behaviour.enabled = false;
        behaviour.enabled = true;
    }
}
