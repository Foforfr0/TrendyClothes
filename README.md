# 👗 TrendyClothes ASP.NET

Este proyecto forma parte de un sistema web para la tienda *TrendyClothes*, utilizando ASP.NET y SQL Server.

---

## 🐳 Docker

### 🔧 Inicialización de contenedores
```bash
# Construye las imágenes sin iniciarlas
docker compose up --build --no-start
# Elimina el volumen (En caso de fallo)
docker compose down -v
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
# Copia los archivos de la base de datos (si aplica)
docker cp "C:\Archivos\Example Files\TrendyClothes\." sqlserver:/var/opt/mssql/data
# Ejecutar el script SQL desde el host
sqlcmd -S localhost,1433 -U sa -P StrongP@ssw0rd! -i "C:\Archivos\Projects Programs\Páginas web\TrendyClothes\Database.sql"
```
