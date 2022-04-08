using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public class ImportPlugin
    {
        public DirectoryCatalog Catalog { get; }
        public CompositionContainer Container { get; }
        public CompositionBatch Batch { get; }

        private List<Func<string, string>> FolderMethods = new List<Func<string, string>>
        {
            (folder) => folder,
            (folder) =>
            {
                var location = Assembly.GetExecutingAssembly().Location;
                var fileInfo = new FileInfo(location);
                var path = fileInfo.Directory != null ? fileInfo.Directory.FullName : "";
                return Path.Combine(path, folder);
            },
            (folder) =>  Path.Combine(Directory.GetCurrentDirectory(), folder)
        };

        public ImportPlugin(string folder, object reference = null)
        {
            Catalog = new DirectoryCatalog(BuildPath(folder));
            Container = new CompositionContainer(Catalog);
            Batch = new CompositionBatch();
            if (reference != null)
                Batch.AddPart(reference);
            else
                Batch.AddPart(this);

            Container.Compose(Batch);
        }

        private string BuildPath(string folder)
        {
            foreach (var folderMethod in FolderMethods)
            {
                var foundFolder = folderMethod.Invoke(folder);
                if (Directory.Exists(foundFolder))
                    return foundFolder;
            }

            throw new DirectoryNotFoundException("pasta " + folder + " não encontrada");
        }
    }

    public class ImportPlugin<T> : ImportPlugin
    {
        [ImportMany]
        public IEnumerable<Lazy<T>> Plugins { get; set; }

        public ImportPlugin(string folder) : base(folder) { }
    }
}
