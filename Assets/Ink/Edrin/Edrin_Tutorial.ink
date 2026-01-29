EXTERNAL SetFlag(flagID)
EXTERNAL GiveItem(itemID, amount)

=== intro ===
# speaker: Edrin
…Has despertado.

# speaker: Edrin
El caos no te rechazó.
# speaker: Edrin
Eso ya es más de lo que muchos pueden decir.

~ SetFlag("TUTORIAL_STARTED")
~ met_edrin = true

# speaker: Edrin
Antes de avanzar, necesitarás provisiones.

# speaker: Edrin
Detrás de mí hay un cofre.
# speaker: Edrin
Tómalo. Luego regresa.

~ GiveItem("key_starting_chest_01", 1)
~ SetFlag("STARTING_CHEST_KEY_GIVEN")

-> END

=== wait_for_chest ===
# speaker: Edrin
No temas al cofre. 
# speaker: Edrin
El mundo rara vez ofrece regalos dos veces. 

-> END

=== after_chest ===
# speaker: Edrin
Bien.

# speaker: Edrin
Más adelante, algo ya te ha sentido.

# speaker: Edrin
No podrás evitarlo.

# speaker: Edrin
Cuando lo enfrentes…

# speaker: Edrin
recuerda volver con vida.

~ SetFlag("AFTER_CHEST_SHOWN")

-> END

-> wait_for_first_enemy

=== wait_for_first_enemy ===
# speaker: Edrin
El peligro no desaparece ignorándolo.

-> END


=== after_first_enemy ===
# speaker: Edrin
Ahora sabes cómo se siente.

# speaker: Edrin
Ese fue solo un eco.

# speaker: Edrin
Para abandonar esta isla,
debes destruir aquello que la reclama.

# speaker: Edrin
Una cueva al pie de la montaña.
Ahí duerme el guardián.


-> explain_boss


=== explain_boss ===
# speaker: Edrin
Ya has sentido su presencia.

# speaker: Edrin
Eso no era más que un aviso.

# speaker: Edrin
Al pie de la montaña hay una cueva.
Ahí descansa el guardián de la isla.

# speaker: Edrin
Mientras siga con vida,
nadie abandona este lugar.

# speaker: Edrin
No iré contigo.
Este paso no puede ser compartido.

# speaker: Edrin
Cuando caiga,
el camino se abrirá solo.

-> END


=== dock_scene ===
# speaker: Edrin
Has sobrevivido a la isla.

# speaker: Edrin
Eso no te convierte en héroe.

# speaker: Edrin
Solo en alguien con opciones.

# speaker: Edrin
La llave que portas
no abre un destino seguro.

# speaker: Edrin
Solo una elección.

# speaker: Edrin
El mundo más allá no espera.

# speaker: Edrin
Camina.

-> END
