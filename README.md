# 👗 TrendyClothes ASP.NET

Este proyecto forma parte de un sistema web para la tienda *TrendyClothes*, utilizando ASP.NET y SQL Server.

---
### Por realizar...
- Subasta / Subastador
	- Consultar mis subastas
	- Consultar detalles de subasta en tiempo real
	- Crear subasta
	- Cambiar estado de subasta (Cancaler, pausar/activar)
- Subasta / Partipante
	- Consultar subastas (solo activas o pausadas)
	- Ver detalles de subasta
	- Realizar puja

### Casos de uso
| # |Caso de uso                            |Estado de API|Estado de WebApp|Estado de AndroidApp|Estado de WpfApp|
|:-:|:--------------------------------------|:-----------:|:--------------:|:------------------:|:--------------:|
|01 |Registrarse                            |      ✔️     |        ✔️       |         ✔️         |       ✔️       |
|02 |Iniciar sesión                         |      ✔️     |        ✔️       |         ✔️         |       ✔️       |
|03 |Registrar productos                    |      ✔️     |        ✔️       |         ✔️         |       ✔️       |
|04 |Buscar productos                       |      ✔️     |        ✔️       |         ✔️         |       ✔️       |
|05 |Editar publicación                     |      ✔️     |        ✔️       |         ✔️         |       ✔️       |
|06 |Eliminar publicación                   |      ✔️     |        ✔️       |         ✔️         |       ✔️       |
|07 |Realizar subasta                       |      ✔️     |       ❌       |         ❌         |       ❌      |
|08 |Eliminar subasta                       |      ❌     |       ❌       |         ❌        |       ❌      |
|09 |Gestionar reportes de usuarios         |      ❌     |       ❌       |         ❌        |       ❌      |


## 🐳 Docker
### 🔧 Inicialización de contenedores
```bash
# Construye las imágenes sin iniciarlas
docker compose up --build --no-start
# Elimina el volumen (En caso de en SQL o tokenantiforgery)
	docker compose down -v
# Detener contenedores en ejecución
docker compose stop
# Inicia los contenedores ya construidos
docker compose start
```

### Inicialización de base de datos (SQL Server)
```bash
# Inicia el contenedor de SQL Server.
# Instala sqlcmd en el host si aún no lo tienes.
# Verifica la conexión al servidor SQL:
sqlcmd -S localhost,1433 -U SA -P StrongP@ssw0rd!
```

### 📂 Copiar archivos necesarios al contenedor
```bash
# Copia el script SQL de creación de base de datos
docker cp ./Database.sql sqlserver:/tmp/Database.sql
# Copia los archivos de la base de datos
docker cp "./Frontend/Example Files/." sqlserver:/var/opt/mssql/data
# Ejecutar el script SQL desde el host
sqlcmd -S localhost,1433 -U sa -P StrongP@ssw0rd! -i "./Database.sql"
```