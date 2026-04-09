using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using LaExpedicion.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LaExpedicion.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public BlobStorageService(IConfiguration configuration)
    {
        string connectionString = configuration.GetSection("AzureBlobStorage:ConnectionString").Value!;
        _containerName = configuration.GetSection("AzureBlobStorage:ContainerName").Value!;
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> SubirArchivo(IFormFile? file, string nombreCarpeta, string nombreArchivo)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("El archivo está vacío.");
        
        // 1. Obtenemos el contenedor y lo creamos si no existe
        // (con acceso público para leer imágenes)
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob); 
        
        // 2. Generamos un nombre único para el archivo
        string extension = Path.GetExtension(file.FileName);
        
        string nombreArchivoFinal = $"{nombreCarpeta}/{nombreArchivo}-{Guid.NewGuid()}{extension}";
        
        // Usamos BlockBlobClient para chunks
        BlockBlobClient blockBlobClient = containerClient.GetBlockBlobClient(nombreArchivoFinal);
        
        // 3. Configuracion del Chunk (Tamaño)
        int chunkSize = 2 * 1024 * 1024;
        byte[] buffer = new byte[chunkSize];

        List<string> blockIds = new List<string>();
        int blockNumber = 0;
        
        // 4. Procesar los chinks (para no saturar la RAM)
        using (var stream = file.OpenReadStream())
        {
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                // Azure exige que el ID del chunk sea un string en Base64 de longitud fija
                string blockId = Convert.ToBase64String(BitConverter.GetBytes(blockNumber).Reverse().ToArray());
                blockIds.Add(blockId);
                
                // Tomamos solo los bytes que se leyeron (el último chunk puede ser menor a 2MB)
                using (var chunkStream = new MemoryStream(buffer, 0, bytesRead))
                {
                    // Subimos la pieza del Lego a Azure (Aún no se arma)
                    await blockBlobClient.StageBlockAsync(blockId, chunkStream);
                }
                blockNumber++;
            }
        }
        
        // 5. Hacer que Azure una todas las piezas en orden 
        await blockBlobClient.CommitBlockListAsync(blockIds);
        
        // Retornar la URL
        return blockBlobClient.Uri.ToString();
    }
}