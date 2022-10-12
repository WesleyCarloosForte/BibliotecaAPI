# ApiBiblioteca
API
URLBASE = http://localhost:5006/
Estructura =http://localhost:5006/api/rutaControlador

Cómo ejecutar el proyecto:
Abrir el el archivo (Solucion) ApiBiblioteca.sln en VIsual studio (2022)
![image](https://user-images.githubusercontent.com/54281827/195437936-73cc1f9c-9826-4eae-879a-047c3bb45df4.png)


Esta api simula el funcionamiento de una biblioteca y cuenta con autenticación y autorización mediante roles (usuarios y administradores).
Permite hacer todas las acciones básicas de un CRUD en todas las entidades:

Entidades (Todas las entidades tienen sus DTOS)
Libros
Autores
Usuarios
Géneros

Tecnologías 
-.NET 6 (Ultima version)
- EF CORE Code First
- AutoMapper
- SqlIte
- C#
-Data annotation
-Linq
-Inyección de dependencia 

IDE
visual studio community 2022

Estructura del proyecto: 

![image](https://user-images.githubusercontent.com/54281827/195429768-d4963081-8ee1-48ae-80db-89c5e8734ecb.png)

Documentacion completa (Swagger)

La documentación en swagger se abrirá automáticamente cuando ejecutes el proyecto en visual studio 

![image](https://user-images.githubusercontent.com/54281827/195430987-916f6aa5-6adf-4948-a0b5-3d79ddbe880d.png)

Postman Collection
![image](https://user-images.githubusercontent.com/54281827/195432557-446780e4-d3e6-4952-a30a-8f57cdc6bbcc.png)


Swagger
![image](https://user-images.githubusercontent.com/54281827/195430522-1287f54c-d393-470b-932e-f13cc293650b.png)
![image](https://user-images.githubusercontent.com/54281827/195430694-5386b973-d406-4139-aae9-7da2ef7e8862.png)
![image](https://user-images.githubusercontent.com/54281827/195430758-99d0003d-a4e3-4fc2-918e-238d7ad959bd.png)

Postaman

PARÁMETRO CABECERA:
KEY:Authorization
VALUE: Bearer “TOKEN” (siempre poner Bear ”espacio” token)
![image](https://user-images.githubusercontent.com/54281827/195431144-dbbe71d0-49fc-4fbb-9a76-427b19c137cb.png)

