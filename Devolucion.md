# Devolucion o feedback del profesor

5 Especies propias OK (1.0 / 1.0)

El código efectivamente limita los parámetros a los valores de la descripción (Como los por defecto), pero no hay pruebas que verifiquen esto. Esas pruebas deberian e intentarían construir Pokémon con atributos inválidos y las instancias deberían salir correctas. Así mismo, tampoco hay pruebas que verifiquen que un Pokémon no pueda tener dos tipos iguales (0.0 / 0.5)

Pruebas modificadores OK (1.5 / 1.5)

Pruebas de combate (0.85 / 2.0)
Tienen éxito 12/40. El problema es la precisión: Vienes operando con doubles (que tienen una mayor precisión), y solo redondeás al final.

Total: 3.35 / 5.0

su nota no fue especialmente alta porque las pruebas de daño en combate de los Pokémon fallaron por precisión en el cálculo o depronto la formula estaba realizando operaciones sin orden jerarquico con dice la formula es decir no es lo mismo hacer 2*(8+5)/2 = 13 a 2*8+5/2 = 18.5 a 2*((8+5)/2)= 6.5 por lo que se debe revisar esa formula como se esta escribiendo en el codigo

## Correo posterior al examen en general para informar a todos los estudiantes del curso

1. Casi ninguno validó tipos dobles. Los códigos presentados permiten que un Pokémon tenga de tipo Electric/Electric, por ejemplo.

2. Al parecer, ninguno leyó el enunciado completo. Casi ninguno utilizó la tabla de tipos que entregué en el enunciado completa, y se limitaron a probar los tipos de sus especies propias o los tipos que quisieron. Uno de ustedes, inclusive, introdujo el tipo Normal, que no estaba definido en el ejercicio, pero pasó por completo de los 5 tipos restantes de la tabla.

3. vamos a los fallos salvables... No, en realidad solo hubo un fallo salvable:
Eligieron mal el tipo de datos. Prácticamente todos se fueron con double. Por qué? Todos los parámetros y los resultados esperados son enteros, no hay decimales. Ahora, float y double tienen unas diferencias marcadas. Computacionalmente, pueden arrojar resultados diferentes según los resultados de las divisiones, por el nivel de precisión - esto deberían saberlo desde Fundamentos de Programación. El problema grande con esto es que, al redondear el resultado final, ya arrastran los datos de cálculos previos; en especial, los compañeros que partieron la fórmula (elección inteligente, debo decir). Dicho esto, me pregunto cómo llegaron al tema de redondear hacia abajo al aplicar sus fórmulas y no pensaron en que era mejor operar todo con enteros. Estaba el modificador 0.5; pero a estas alturas deberían saber que multiplicar por 0.5 es igual a dividir por 2, entero.