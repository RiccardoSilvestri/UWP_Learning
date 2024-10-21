using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRegistry.Models;
using Windows.Storage;

namespace UserRegistry.BLogic
{
    public static class FileManager
    {
        //public void WriteToTxtFile(string fileName, string text, bool fileOwerride = false)
        //{
        //    CreationCollisionOption creationCollisionOption
        //        = fileOwerride ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.OpenIfExists;


        //    StorageFile storageFile = ApplicationData.Current.LocalFolder
        //        .CreateFileAsync($"{fileName}", creationCollisionOption).GetAwaiter().GetResult();

        //    if (fileOwerride)
        //        FileIO.WriteTextAsync(storageFile, text).GetAwaiter().GetResult();
        //    else
        //        FileIO.AppendTextAsync(storageFile, text).GetAwaiter().GetResult();
        //}

        public async static Task<List<T>> ReadJsonFile<T>(string fileName) where T : new()
        {
            List<T> listObjects = [];

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder
                    .CreateFileAsync($"{fileName}", CreationCollisionOption.OpenIfExists);

                using var stream = await storageFile.OpenStreamForReadAsync();
                using var reader = new StreamReader(stream);
                string text = await reader.ReadToEndAsync();

                var items = JsonConvert.DeserializeObject<List<T>>(text);
                if (items != null)
                {
                    listObjects.AddRange(items);
                }
            }
            catch (FileNotFoundException)
            {
                // Gestisce il caso in cui il file non esiste
                Console.WriteLine($"ATTENZIONE: Il file {fileName} non è stato trovato.");
            }
            catch (JsonException ex)
            {
                // Gestisce errori di deserializzazione JSON
                Console.WriteLine($"ATTENZIONE: Errore nella deserializzazione del JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Gestisce altre eccezioni impreviste
                Console.WriteLine($"ATTENZIONE: Si è verificato un errore imprevisto: {ex.Message}");
            }

            return listObjects;
        }

        public async static Task WriteToJsonFile<T>(List<T> listObjects, string fileName, bool fileOwerride = false) where T : new()
        {
            CreationCollisionOption creationCollisionOption
                = fileOwerride ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.OpenIfExists;

            StorageFile storageFile = await ApplicationData.Current.LocalFolder
                .CreateFileAsync($"{fileName}", creationCollisionOption);

            if (fileOwerride)
            {
                FileIO.WriteTextAsync(storageFile, JsonConvert
                   .SerializeObject(new List<T>(listObjects)))
                   .GetAwaiter().GetResult();
            }
            else
            {
                FileIO.AppendTextAsync(storageFile, JsonConvert
                   .SerializeObject(new List<T>(listObjects)))
                   .GetAwaiter().GetResult();
            }
        }
    }
}
