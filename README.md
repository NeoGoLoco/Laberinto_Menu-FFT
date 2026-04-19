# 🎱 Laberinto con funciones divertidas enfocadas en la reactividad musical, junto a la conexión en el controlador de PS4 y PS5 que su led retroiluminado reacciona a la Música (FFT + New Input).

Bienvenido a un experimento de **Game Feel** y **Audio-Reactividad** desarrollado en Unity. Este no es tu típico tutorial de "Rueda y salta"; aquí el escenario tiene vida propia, los controles vibran cuando te estrellas contra el piso, y el código base está modernizado para no depender de sistemas anteriores. Además de que los Leds del control
brillan al ritmo de la música.

## Features

* Utilizando análisis de espectro de audio (Fast Fourier Transform / FFT), el menú [0] y el laberinto [1] lee las frecuencias bajas (Bass) de la música de fondo, leds del control y texturas del entorno.
* Este proyecto utiliza el sistema de inputs moderno de Unity basado en eventos. Listo para teclado, mouse, controles de PlayStation y Xbox sin tocar una sola línea de lógica. 
* Porque la gravedad duele. Implementé un sistema de vibración asimétrica para los controles que reacciona exactamente en el fotograma en el que la bolita impacta contra el suelo tras un salto.
* Nada rompe más la inmersión que ver el interior hueco de una pared. La cámara sigue a la bolita de forma relativa y utiliza **Raycasts** para ignorar objetos con el Tag `Default` o reposicionarse sin atravesar la geometría del nivel.
* Sistema de **Velocity Clamping** para que la bolita no rompa la barrera del sonido, y detección de suelo por Raycast (porque todos sabemos que el `OnCollisionEnter` te traiciona cuando menos lo esperas).

## Tecnologías:

* **Motor:** Unity 3D
* **Paquetes Core:** `Input System` (El nuevo, el bueno).
* **Físicas:** Rigidbody 3D.
* **Audio:** `AudioSource.GetSpectrumData` para la magia de Fourier.

## ¿Cómo probarlo?

1. Clona este repositorio en tu máquina.
2. Ábrelo con Unity (Asegúrate de tener instalado el paquete `Input System` desde el Package Manager, aunque Unity te lo debería pedir al abrir).
3. Conecta tu control favorito de Xbox o PlayStation (altamente recomendado mantenerlo conectado).
4. Abre la escena principal, dale al Play, y sube el volumen. 
