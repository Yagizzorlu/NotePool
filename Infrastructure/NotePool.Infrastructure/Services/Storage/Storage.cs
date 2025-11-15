using NotePool.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Infrastructure.Services.Storage
{

public class Storage
    {
        // Aynen istediğin imza:
        protected delegate bool HasFile(string pathOrContainerName, string fileName);

        // Aynen istediğin imza ve adlar:
        protected async Task<string> FileRenameAsync(
            string pathOrContainerName,
            string fileName,
            HasFile hasFileMethod,
            bool first = true)
        {
            return await Task.Run(() =>
            {
                var ext = Path.GetExtension(fileName);
                var baseNameOriginal = Path.GetFileNameWithoutExtension(fileName);

                // Mevcut regulasyon fonksiyonun
                var baseName = NameOperation.CharacterRegulatory(baseNameOriginal);

                var candidate = $"{baseName}{ext}";
                int counter = 2;

                // DİKKAT: Artık File.Exists değil, DELEGATE kullanıyoruz.
                while (hasFileMethod(pathOrContainerName, candidate))
                {
                    candidate = $"{baseName}-{counter}{ext}";
                    counter++;
                }

                return candidate;
            });
        }
    }
}

