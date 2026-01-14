public enum GameState
{
    Boot,        // Juego recién iniciado
    Menu,        // Menú principal
    Playing,     // Gameplay normal
    Paused,      // PauseMenu
    Inventory,   // Inventario abierto
    Loot,        // Ventana de loot
    LevelUp,     // Selección de nivel
    Dialogue,    // NPC / Cutscene
    Transition,  // Cambio de escena
    GameOver
}
