* Ejecutar Script para la generación de la BD, se encuentra en la carpeta Data/DELTAS

Que se puede mejorar/pulir:
- Centralizar el manejo de errores en un Middleware. Serviría para capturar y formatear errores en un solo lugar, y asi dejar de utilizar try-catch en los servicios y repositorios.
- Implementar Logs para monitoreo de errores de sistema y negocio. Tambien puede servir para monitoreo de procesos de negocio.
- Se puede crear una clase genérica en la capa de repositorio para contener los metodos comunes a todas las clases repositorios.
- Generar interfaces para los servicios para desacoplar sus implementaciones.
- Realizar una eliminación efectiva de un registro de la base de datos puede no ser conveniente en el negocio, se podría mejorar a un atributo que represente una "desactivación" de la wallet en el ámbito de éste negocio.

No se ha implementado un sistema de autenticación por falta de tiempo, pero en mi caso utilizaría JWT.
Se generaria toda una estructura para el login (controlador, service, repositorio), configuraria en el Program.cs la generación del token y agregaria los middleware de autenticación y autorización.
Generaria el token, y siempre seria necesario enviarlo por el header para las solicitudes necesarias, el middleware se encargaría de pasar la solicitud o no.

La documentación de la API se realizó con la dependencia de Swagger.

Por falta de tiempo no generé las clases de test integración, pero para ello seguiria los pasos:
- Configurar el entorno de prueba, levantando la api y una base de datos en memoria.
- Simular peticiones HTTP a la API.
- Comprobar en la respuesta los códigos y datos del JSON que se espera.

Preguntas a contestar:

1. ¿Cómo tu implementación puede ser escalable a miles de transacciones?

La estrategia más importante que se me ocurre, sería de implementar multiples instancias usando tecnología como Kubernetes e implementando un balanceador de carga.
Otras menores sería implementar indices en la base de datos y/o utilización de cache con tecnologías como Redis.

2. ¿Cómo tu implementación asegura el principio de idempotencia?

Para asegurar se debe generar una clave única por transacción para evitar registros duplicados y la respuesta sea la misma siempre.
El método POST del TransactionController no es idempotente actualmente, la solución que implementaría sería de agregar un atributo a Transaction de tipo GUID. Y modificaria el servicio y repositorio para tratar peticiones duplicadas.

3. ¿Cómo protegerías tus servicios para evitar ataques de Denegación de Servicio, SQL Injection y CSRF?

Para DDoS usaria AspNetCoreRateLimit o Firewall como Cloudflare.
Para sql injection, si necesitamos tener sql crudo en el código, siempre hay que utilizar parametros en las consultas. O utilizar en su totalidad un ORM.
Para CSRF, utilizar CORS para dominios especificos permitidos. Y usar tokens CSRF para validar solicitudes sensibles.

4. ¿Cuál sería tu estrategia para migrar un monolito a microservicios?

Iniciaria identificando todos los módulos del sistema que podemos llegar a convertirlos en independientes.
Empezar a desacoplar servicios, generando nuevos microservicios para cada uno, desplegarlos y utilizar un API Gateway para gestionar la autenticación y la dirección de las peticiones.

5. ¿Qué alternativas a la solución requerida propondrías para una solución escalable?

Se podria usar arquitectura de eventos, para que transacciones se procesen de manera asíncronas.
Si sabemos de antemano que el proyecto va a ser necesario escalar en gran medida por la cantidad de tráfico que tendrá, podriamos pensar en descomponerlo desde el inicio en microservicios. Asi reducimos a largo plazo la dificultad en el despliegue y escalabilidad.
Se puede utilizar funciones serverless para manejar creaciones y consultas de transacciones bajo demanda.

